var faucetsRouter = require('express').Router();
var Web3 = require('web3');
require('dotenv').config();

const Tx = require('ethereumjs-tx');
const privateTestnetJson = require("./contracts/private_testnet.json");

faucetsRouter.get('/sip/:account', function (req, res) {
  let endpoint = decodeURI(req.query.endpoint);
  let account = req.params.account;
  
  let privateKey = new Buffer(process.env.PRIVATE_KEY, 'hex');

  const web3 = new Web3(endpoint);

  web3.eth.getTransactionCount(process.env.ACCOUNT).then(nonce => {
    const rawTx = {
      from: process.env.ACCOUNT, 
      nonce: "0x" + nonce.toString(16),
      to: account,
      gas: 8000000,
      gasPrice: 100000000000,
      value: web3.utils.toHex(web3.utils.toWei("0.5", 'ether'))
    }

    const tx = new Tx(rawTx);
    tx.sign(privateKey);

    const serializedTx = tx.serialize();

    //send signed transaction
    web3.eth.sendSignedTransaction('0x' + serializedTx.toString('hex'));
  });

  res.json("0.5 ETH sent");

});

faucetsRouter.get('/validator/eth/:account', function (req, res) {
  let account = req.params.account;
  
  let privateKey = new Buffer(process.env.PRIVATE_KEY_VALIDATOR, 'hex');

  const web3 = new Web3(process.env.PRIVATE_MAINNET);
  
  web3.eth.getTransactionCount(process.env.ACCOUNT_VALIDATOR).then(nonce => {
    
    const rawTx = {
      from: process.env.ACCOUNT_VALIDATOR,
      nonce: "0x" + nonce.toString(16),
      to: account,
      gas: 8000000,
      gasPrice: 100000000000,
      value: web3.utils.toHex(web3.utils.toWei("3.5", 'ether'))
    }

    const tx = new Tx(rawTx);
    tx.sign(privateKey);

    const serializedTx = tx.serialize();

    //send signed transaction
    web3.eth.sendSignedTransaction('0x' + serializedTx.toString('hex'));
  });

  res.json("3.5 ETH sent");

});

faucetsRouter.get('/validator/skl/:account', function (req, res) {
  let account = req.params.account;

  let privateKey = new Buffer(process.env.PRIVATE_KEY_VALIDATOR, 'hex');

  const web3 = new Web3(process.env.PRIVATE_MAINNET);

  const amount = web3.utils.toHex(web3.utils.toWei("100", 'ether'));
  const erc20ABI = privateTestnetJson.skale_token_abi;
  const erc20Address = privateTestnetJson.skale_token_address;

  let contract = new web3.eth.Contract(erc20ABI, erc20Address);

  web3.eth.getTransactionCount(process.env.ACCOUNT_VALIDATOR).then(nonce => {
    
    const rawTx = {
      from: process.env.ACCOUNT_VALIDATOR,
      nonce: "0x" + nonce.toString(16),
      gas: 8000000,
      gasPrice: 100000000000,
      to: erc20Address,
      value: "0x0",
      data: contract.methods.transfer(account, amount).encodeABI(),
    };

    const tx = new Tx(rawTx);
    tx.sign(privateKey);

    const serializedTx = tx.serialize();

    //send signed transaction
    web3.eth.sendSignedTransaction('0x' + serializedTx.toString('hex'));
  });

  res.json("100 SKL sent");

});

// Error handler
faucetsRouter.use(function (err, req, res, next) {

    if (err) {
        res.status(500).send(err);
    }

});

module.exports = faucetsRouter;