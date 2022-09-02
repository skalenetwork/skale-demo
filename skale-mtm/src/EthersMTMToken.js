const contract = require("../abi/"+process.env.ABI_NAME);
const ethers = require("ethers");
const endpoint = process.env.ENDPOINT;
const pk = process.env.PRIVATE_KEY;
const account = process.env.ACCOUNT;

const web3Provider = new ethers.providers.JsonRpcProvider(endpoint)
const signer = new ethers.Wallet(pk, web3Provider);

async function mint(tokenId, nonce) {

    const overrides = {
        nonce:nonce,
        gasLimit:10000000
    }
    console.log("nonce",nonce);
    let StrangeToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi, web3Provider);
    return StrangeToken.connect(signer).mint(tokenId, overrides);
}

async function getRevertReason(txHash){
    try {
        const tx = await web3Provider.getTransaction(txHash)

        var result = await web3Provider.call(tx, tx.blockNumber)

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
    let StrangeToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi);
    return parseInt(await StrangeToken.connect(signer).getCurrentTokenId());
}

async function getTransactionCount() {
    let tx = await web3Provider.getTransactionCount(account);
    let pending = await web3Provider.getTransactionCount(account, "pending");
    if (tx > pending) {
        return tx;
    } else {
        return pending
    }
}

module.exports = {
    mint,getRevertReason,
    getTransactionCount,
    getCurrentTokenId
};
