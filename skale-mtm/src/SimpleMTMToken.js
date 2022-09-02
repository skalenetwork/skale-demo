const contract = require("../abi/"+process.env.ABI_NAME);
const endpoint = process.env.ENDPOINT;
const privateKey = process.env.PRIVATE_KEY;
const account = process.env.ACCOUNT;

const Web3 = require('web3');
const web3Provider = new Web3(new Web3.providers.HttpProvider(endpoint));
const delay = ms => new Promise(res => setTimeout(res, ms));


async function mtm_query() {
    let nonce = await web3Provider.eth.getTransactionCount(account);

    console.log('CURRENT NONCE', nonce);
    console.log('');
    await delay(5000);
    let chainId = await web3Provider.eth.getChainId();
    let tx = {
        from: account,
        gas: 21001,
        to: '0x3760dc9594ccac0f33ade5cc5371402131696341',
        nonce: nonce + 4,
        chainId: chainId
    };
    let signedTx = await web3Provider.eth.accounts.signTransaction(tx, privateKey);
    console.log('NONCE: ', tx.nonce);
    web3Provider.eth.sendSignedTransaction(signedTx.rawTransaction)
        .once('transactionHash', function(hash) {
            console.log('HASH: ', hash);
            let tx = {
                from: account,
                gas: 21001,
                to: '0x3760dc9594ccac0f33ade5cc5371402131696341',
                nonce: nonce + 2,
                chainId: chainId
            };
            console.log('NONCE: ', tx.nonce);
            web3Provider.eth.accounts.signTransaction(tx, privateKey).then(function (signedTx) {
                web3Provider.eth.sendSignedTransaction(signedTx.rawTransaction)
                    .once('transactionHash', function (hash) {
                        console.log('HASH: ', hash);
                        delay(1000).then(function () {
                            tx = {
                                from: account,
                                gas: 21001,
                                to: '0x3760dc9594ccac0f33ade5cc5371402131696341',
                                nonce: nonce + 1,
                                chainId: chainId
                            };
                            console.log('NONCE: ', tx.nonce);
                            web3Provider.eth.accounts.signTransaction(tx, privateKey).then(function (signedTx) {
                                web3Provider.eth.sendSignedTransaction(signedTx.rawTransaction)
                                    .once('transactionHash', function (hash) {
                                        console.log('HASH: ', hash);
                                        let tx = {
                                            from: account,
                                            gas: 21001,
                                            to: '0x3760dc9594ccac0f33ade5cc5371402131696341',
                                            nonce: nonce,
                                            chainId: chainId
                                        };
                                        console.log('NONCE: ', tx.nonce);
                                        web3Provider.eth.accounts.signTransaction(tx, privateKey).then(function (signedTx) {
                                            web3Provider.eth.sendSignedTransaction(signedTx.rawTransaction).then(function (receipt) {
                                                console.log('HASH: ', receipt.transactionHash);
                                                console.log('---------BLOCK-----------');
                                                web3Provider.eth.getBlock(receipt.blockNumber).then(function (block) {
                                                    console.log(block);
                                                    console.log('========================');
                                                    console.log('');
                                                    let tx = {
                                                        from: account,
                                                        gas: 21001,
                                                        to: '0x3760dc9594ccac0f33ade5cc5371402131696341',
                                                        nonce: nonce + 3,
                                                        chainId: chainId
                                                    };
                                                    console.log('NONCE: ', tx.nonce);
                                                    web3Provider.eth.accounts.signTransaction(tx, privateKey).then(function (signedTx) {
                                                        web3Provider.eth.sendSignedTransaction(signedTx.rawTransaction).then(function (receipt) {
                                                            console.log('HASH: ', receipt.transactionHash);
                                                            console.log('---------BLOCK 2-----------');
                                                            web3Provider.eth.getBlock(receipt.blockNumber).then(function (block) {
                                                                console.log(block);
                                                            });
                                                        });
                                                    });
                                                });
                                            });
                                        });
                                    });
                            });
                        });
                    });
            });
        });
}

async function getRevertReason(txHash){
    try {
        const tx = await web3Provider.eth.getTransaction(txHash)

        var result = await web3Provider.eth.call(tx, tx.blockNumber)

        result = result.startsWith('0x') ? result : `0x${result}`

        if (result && result.substr(138)) {

            const reason = web3Provider.utils.toAscii(result.substr(138))
            console.log('Revert reason:', reason)
            return reason

        } else {
            console.log('Cannot get reason - No return value')
        }
    }
    catch (err)
    {
        console.log('Error occured with return reason',txHash , err)
    }

}
async function getCurrentTokenId() {
    let StrangeToken = new web3Provider.eth.Contract(contract.erc721_abi, contract.erc721_address);
    return parseInt(await StrangeToken.methods.getCurrentTokenId().call());
}

async function getTransactionCount() {
    let tx = await web3Provider.eth.getTransactionCount(account);
    let pending = await web3Provider.eth.getTransactionCount(account, "pending");
    if (tx > pending) {
        return tx;
    } else {
        return pending
    }
}

module.exports = {
    mtm_query,getRevertReason,
    getTransactionCount,
    getCurrentTokenId
};
