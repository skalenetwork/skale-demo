require("@nomicfoundation/hardhat-toolbox");
const { task } = require("hardhat/config");
const { getAbi } = require('./tools/abi');
const  { promises } = require("fs");
const fs = promises;
require('dotenv').config()

task("deploy-token", "Deploy ERC-721 token").setAction(async (args, {ethers}) => {
  const contractName = "GamingToken";
  const erc721Factory = await ethers.getContractFactory(contractName);
  const erc721 = await erc721Factory.deploy();
  await erc721.deployTransaction.wait();
  console.log("ERC721 Token GamingToken was deployed");

  console.log("Address:", erc721.address);
  const jsonObj = {};
  jsonObj.erc721_address = erc721.address;
  jsonObj.erc721_abi = getAbi(erc721.interface);
  await fs.writeFile("abi/" + contractName + "-WithAddress.json", JSON.stringify(jsonObj, null, 4));
  await fs.writeFile("abi/" + contractName + ".json", JSON.stringify(jsonObj.erc721_abi, null, 4));
});

function getCustomUrl(url) {
  if (url) {
    return url;
  } else {
    return "http://127.0.0.1:8545"
  }
}

function getCustomPrivateKey(privateKey) {
  if (privateKey) {
    return [privateKey];
  } else {
    return [];
  }
}

function getGasPrice(gasPrice) {
  if (gasPrice) {
    return parseInt(gasPrice, 10);
  } else {
    return "auto";
  }
}

/** @type import('hardhat/config').HardhatUserConfig */
module.exports = {
  solidity: "0.8.17",
  defaultNetwork: "hardhat",
  solidity: {
      version: '0.8.17',
      settings: {
          optimizer: {
              enabled: true,
              runs: 200,
              details: {
                  yul: false
              }
          }
      }
  },
  mocha: {
    timeout: 1000000
  },
  networks: {
    hardhat: {
      blockGasLimit: 12000000
    },
    custom: {
        url: getCustomUrl(process.env.ENDPOINT),
        accounts: getCustomPrivateKey(process.env.PRIVATE_KEY),
        gasPrice: getGasPrice(process.env.GASPRICE)
    }
  },
  etherscan: {
    apiKey: "QSW5NZN9RCYXSZWVB32DMUN83UZ5EJUREI"
  }
};
