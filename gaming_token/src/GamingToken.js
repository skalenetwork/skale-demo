require('dotenv').config()

const contract = require("../abi/"+process.env.ABI_NAME);
const ethers = require("ethers");
const endpoint = process.env.ENDPOINT;
const pk = process.env.PRIVATE_KEY;

const web3Provider = new ethers.providers.JsonRpcProvider(endpoint)
const signer = new ethers.Wallet(pk, web3Provider);

async function mint(uri, nonce) {

    let GamingToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi, web3Provider);

    const res = GamingToken.connect(signer).mint(uri, { nonce });
    return res;
}

async function tokenURI(tokenId) {

    let GamingToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi, web3Provider);

    const res = GamingToken.connect(signer).tokenURI(tokenId);
    return res;
}

async function getTransactionCount() {
    let tx = await signer.getTransactionCount();
    return tx;
    // let pending = await web3Provider.getTransactionCount(account, "pending");
    // if (tx > pending) {
    //     return tx;
    // } else {
    //     return pending
    // }
    // ;
}

module.exports = {
    mint,
    getTransactionCount,
    tokenURI
};
