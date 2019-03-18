require('dotenv').config();
let HDWalletProvider = require("truffle-hdwallet-provider");

//https://developers.skalelabs.com for SKALE documentation
//update the mnemonic in the .env file
let mnemonic = process.env.MNEMONIC;
//update your SKALE_CHAIN in .env file
let skale = process.env.SKALE_CHAIN;
let ganache = "http://127.0.0.1:8545";

module.exports = {
    networks: {
        ganache: {
            provider: () => new HDWalletProvider(mnemonic, ganache),
            gasPrice: 0,
            network_id: "*"
        },
        skale: {
            provider: () => new HDWalletProvider(mnemonic, skale),
            gasPrice: 0,
            network_id: "*"
        }
    }
}

