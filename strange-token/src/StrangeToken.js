const contract = require("../abi/"+process.env.REACT_APP_ABI_NAME);
const ethers = require("ethers");
const endpoint = process.env.REACT_APP_ENDPOINT;
const pk = process.env.REACT_APP_PRIVATE_KEY;
const account = process.env.REACT_APP_ACCOUNT;

const web3Provider = new ethers.providers.JsonRpcProvider(endpoint)
const signer = new ethers.Wallet(pk, web3Provider);

async function mint(tokenId, nonce) {

    const overrides = {
        nonce:nonce,
        gasLimit:5000000
    }
    console.log("nonce",nonce);
    let StrangeToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi, web3Provider);

    const estimation = await StrangeToken.estimateGas.mint(tokenId, overrides);

    console.log("token starting to mint... gas fee ", ethers.utils.formatUnits(estimation, "wei"))
    const res = StrangeToken.connect(signer).mint(tokenId, overrides);
    return res;
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
    ;
}




//
// async function addressHasSFUEL(address) {
//     const web3 = new Web3(web3Provider);
//     let addressBalance = web3.eth.getBalance(address, function(err, result) {
//         if (err) {
//             console.log(err)
//         } else {
//             console.log((web3.utils.fromWei(result, "ether")));
//         }
//     })
//
//     if (addressBalance < 0.1) {
//         console.log("Address Balance is low")
//         return false;
//     }
//     console.log("Wallet balance", addressBalance)
//     return true;
// }
//
// async function transfer_sFUEL(receiverAddress) {
//     // Create a wallet instance
//     console.log("amountInETH");
//     let wallet = new ethers.Wallet(process.env.REACT_APP_PRIVATE_KEY, web3Provider)
//     // Ether amount to send
//     let amountInEther = '0.01'
//     // Create a transaction object
//     let tx = {
//         to: receiverAddress,
//         // Convert currency unit from ether to wei
//         value: ethers.utils.parseEther(amountInEther)
//     }
//     // Send a transaction
//     await wallet.sendTransaction(tx)
//         .then((txObj) => {
//             console.log('txHash', txObj.hash)
//         })
// }


module.exports = {
    mint,getRevertReason,
    getTransactionCount,
    getCurrentTokenId
};
