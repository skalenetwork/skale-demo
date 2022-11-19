// require('dotenv').config()
// const hre = require('hardhat');

// const abi = require("../abi/"+process.env.ABI_NAME);
// async function mint(nonce) {
//     const GamingToken = await hre.ethers.getContractAt(abi.erc721_address, abi.erc721_abi);

//     const res = GamingToken.connect(signer).mint({ nonce });
//     return res;
// }

// async function tokenURI(tokenId) {

//     let GamingToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi, web3Provider);

//     const res = GamingToken.connect(signer).tokenURI(tokenId);
//     return res;
// }

// async function getTransactionCount() {
//     let tx = await signer.getTransactionCount();
//     return tx;
// }

// module.exports = {
//     mint,
//     getTransactionCount,
//     tokenURI
// };
