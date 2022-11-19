require('dotenv').config()
const { ethers } = require('hardhat');

const abi = require("../abi/"+process.env.ABI_NAME);

async function mint(uri, nonce) {
    const GamingToken = await ethers.getContractAt('GamingToken', abi.erc721_address);

    const res = GamingToken.mint({ nonce });
    return res;
}

async function tokenURI(tokenId) {

    const GamingToken = await ethers.getContractAt('GamingToken', abi.erc721_address);

    const res = GamingToken.tokenURI(tokenId);
    return res;
}

async function getTransactionCount() {
    const [ signer ] = await ethers.getSigners();
    let tx = await signer.getTransactionCount();
    return tx;
}

module.exports = {
    mint,
    getTransactionCount,
    tokenURI
};
