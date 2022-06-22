import { task, HardhatUserConfig } from "hardhat/config";
import "@nomiclabs/hardhat-etherscan";
import "@nomiclabs/hardhat-web3";
import "@nomiclabs/hardhat-ethers";
import { getAbi } from './tools/abi';
import * as dotenv from "dotenv"
dotenv.config();

import { promises as fs } from "fs";

task("erc721", "Deploy ERC721 Token sample to chain")
    .setAction(async (taskArgs: any, { ethers }) => {
        const contractName = "StrangeToken";
        const erc721Factory = await ethers.getContractFactory(contractName);
        const erc721 = await erc721Factory.deploy();
        await erc721.deployTransaction.wait();
        console.log("ERC721 Token StrangeToken was deployed");

        console.log("Address:", erc721.address);
        const jsonObj: {[str: string]: any} = {};
        jsonObj.erc721_address = erc721.address;
        jsonObj.erc721_abi = getAbi(erc721.interface);
        await fs.writeFile("abi/" + contractName + "-WithAddress.json", JSON.stringify(jsonObj, null, 4));
        await fs.writeFile("abi/" + contractName + ".json", JSON.stringify(jsonObj.erc721_abi, null, 4));

    }
);

function getCustomUrl(url: string | undefined) {
  if (url) {
    return url;
  } else {
    return "http://127.0.0.1:8545"
  }
}

function getCustomPrivateKey(privateKey: string | undefined) {
  if (privateKey) {
    return [privateKey];
  } else {
    return [];
  }
}

function getGasPrice(gasPrice: string | undefined) {
  if (gasPrice) {
    return parseInt(gasPrice, 10);
  } else {
    return "auto";
  }
}

const config: HardhatUserConfig = {
  defaultNetwork: "hardhat",
  solidity: {
      version: '0.8.14',
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
        url: getCustomUrl(process.env.REACT_APP_ENDPOINT),
        accounts: getCustomPrivateKey(process.env.REACT_APP_PRIVATE_KEY),
        gasPrice: getGasPrice(process.env.GASPRICE)
    }
  },
  etherscan: {
    apiKey: "QSW5NZN9RCYXSZWVB32DMUN83UZ5EJUREI"
  }
};

export default config;
