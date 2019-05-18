/*
 * This truffle script will deploy your smart contracts to your SKALE Chain.
 *
 *  @param {String} privateKey - Provide your wallet private key.
 *  @param {String} provider - Provide your SKALE endpoint address.
 */
require('dotenv').config();
let HDWalletProvider = require("truffle-hdwallet-provider");

//https://developers.skalelabs.com for SKALE documentation
//Provide your wallet private key
let privateKey = process.env.PRIVATE_KEY;

let rinkeby = process.env.RINKEBY;

let skale = "http://159.65.106.215:8035/";

module.exports = {
    networks: {
        ganache: {
            host: "127.0.0.1",
            port: 8545,
            network_id: "*"
        },
        rinkeby: {
            provider: () => new HDWalletProvider(privateKey, rinkeby),
            gasPrice: 0,
            network_id: "*"
        },
        skale: {
            provider: () => new HDWalletProvider(privateKey, skale),
            gasPrice: 0,
            network_id: "*"
        }
    }
}