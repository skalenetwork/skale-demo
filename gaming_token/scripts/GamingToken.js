require('dotenv').config()
const hre = require('hardhat');

const abi = require("../abi/"+process.env.ABI_NAME);
// const ethers = require("ethers");
// const endpoint = process.env.ENDPOINT;
// const pk = process.env.PRIVATE_KEY;


async function mint(uri, nonce) {


    // TODO: how do we get the network URL and private key here if not taking that from the .env?
    const endpoint = 'https://testnet-proxy.skalenodes.com/v1/roasted-thankful-unukalhai'
    const pk = '0x16afab4288620045d84475c940204c152b295ee37ca4c8aa67d7259d033e3501'
    const web3Provider = new ethers.providers.JsonRpcProvider(endpoint)
    const signer = new ethers.Wallet(pk, web3Provider);
    const GamingToken = await hre.ethers.getContractAt(abi.erc721_address, abi.erc721_abi)
    // let GamingToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi, web3Provider);

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
