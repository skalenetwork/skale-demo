require('dotenv').config()
const { ethers } = require('hardhat');

const abi = require("../abi/"+process.env.ABI_NAME);

async function mint(uri, nonce) {

    const GamingToken = await ethers.getContractAt(abi.erc721_address, abi.erc721_abi);

    const res = GamingToken.mint(uri, { nonce });
    return res;
}

async function tokenURI(tokenId) {

    const GamingToken = await ethers.getContractAt(abi.erc721_address, abi.erc721_abi);

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
