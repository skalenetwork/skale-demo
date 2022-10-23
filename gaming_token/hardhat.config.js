require("@nomicfoundation/hardhat-toolbox");
require('dotenv').config();

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
