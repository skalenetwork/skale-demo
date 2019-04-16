/*
 * This truffle script will deploy your smart contracts to your new S-Chain.
 *
 *  @param {String} mnemonic - Provide your MetaMask seed words.
 *  @param {String} provider - Provide your SKALE endpoint address.
 */

require('dotenv').config();
let HDWalletProvider = require("truffle-hdwallet-provider");

//https://developers.skalelabs.com for SKALE documentation
//update the mnemonic in the .env file
let mnemonic = process.env.MNEMONIC;

//update your SKALE_CHAIN in .env file
let skale = process.env.SKALE_CHAIN;

module.exports = {
    networks: {
        ganache: {
            host: "127.0.0.1",
            port: 8545,
            network_id: "*"
        },
        skale: {
            provider: () => new HDWalletProvider(mnemonic, skale),
            gasPrice: 0,
            network_id: "*"
        }
    }
}





