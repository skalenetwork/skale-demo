// We require the Hardhat Runtime Environment explicitly here. This is optional
// but useful for running the script in a standalone fashion through `node <script>`.
//
// You can also run a script with `npx hardhat run <script>`. If you do that, Hardhat
// will compile your contracts, add the Hardhat Runtime Environment's members to the
// global scope, and execute the script.
const { ethers, hre } = require("hardhat");
const { getAbi } = require('../tools/abi');
const  { promises } = require("fs");
const fs = promises;

async function main() {
  const contractName = "GamingToken";
  const erc721Factory = await ethers.getContractFactory(contractName);
  const erc721 = await erc721Factory.deploy();
  await erc721.deployTransaction.wait();
  console.log("ERC721 Token GamingToken was deployed");

  console.log("Address:", erc721.address);
  console.log('Please, add this contract address to .env file as CONTRACT_ADDRESS!')
  const jsonObj = {};
  jsonObj.erc721_address = erc721.address;
  jsonObj.erc721_abi = getAbi(erc721.interface);
  await fs.writeFile("abi/" + contractName + "-WithAddress.json", JSON.stringify(jsonObj, null, 4));
  await fs.writeFile("abi/" + contractName + ".json", JSON.stringify(jsonObj.erc721_abi, null, 4));
}

// We recommend this pattern to be able to use async/await everywhere
// and properly handle errors.
main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
