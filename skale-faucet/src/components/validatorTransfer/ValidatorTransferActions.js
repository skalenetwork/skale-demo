import store from '../../store'
import Web3 from 'web3'

import { showMessage, hideMessage } from './../status/StatusActions'

const Tx = require('ethereumjs-tx');
const privateTestnetJson = require("./contracts/private_testnet.json");

export const UPDATE_BALANCE = 'UPDATE_BALANCE'
export function updateBalance(balance) {
  return {
    type: UPDATE_BALANCE,
    payload: balance
  }
}

export const UPDATE_ACCOUNT = 'UPDATE_ACCOUNT'
export function accountUpdate(account) {
  return {
    type: UPDATE_ACCOUNT,
    payload: account
  }
}

export const UPDATE_SKALE_ID = 'UPDATE_SKALE_ID'
export function skaleIdUpdate(skaleId) {
  return {
    type: UPDATE_SKALE_ID,
    payload: skaleId
  }
}

export function updateAccount(account) {
  return function(dispatch) {
    dispatch(accountUpdate(account));
  }
}

export function updateSkaleId(skaleId) {
  return function(dispatch) {
    dispatch(skaleIdUpdate(skaleId));
  }
}

async function getBalance(dispatch) {
  let {account} = store.getState().web3;

  const erc20ABI = privateTestnetJson.skale_token_abi;
  const erc20Address = privateTestnetJson.skale_token_address;

  const web3 = new Web3(process.env.PRIVATE_MAINNET);

  let contract = new web3.eth.Contract(erc20ABI, erc20Address);

  if(typeof web3 !== 'undefined' && account !== ""){
    const balance = await web3.eth.getBalance(account);

    let balanceSKL = await contract.methods.balanceOf(account).call();
    balanceSKL = web3.utils.hexToNumberString(web3.utils.numberToHex(balanceSKL));

    dispatch(updateBalance({balance: web3.utils.fromWei(balance, 'ether'), balanceSKL: web3.utils.fromWei(balanceSKL, 'ether')}));
  }
}

export function refreshBalance() {
  getBalance(store.dispatch);
  setTimeout(function() {
    refreshBalance(store.dispatch);
  }, 2000);
}

export function sendETH() {
  let {account, balance} = store.getState().web3;
  
  if(balance > 0) {
    showMessage("You already have ETH tokens.");
    setTimeout(function() {
    hideMessage();
  }, 3000);
  } else {
    let privateKey = new Buffer(process.env.PRIVATE_KEY_VALIDATOR, 'hex');

    const web3 = new Web3(process.env.PRIVATE_MAINNET);
    
    showMessage("Transfering ETH.");

    web3.eth.getTransactionCount(process.env.ACCOUNT_VALIDATOR).then(nonce => {
      
      const rawTx = {
        from: process.env.ACCOUNT_VALIDATOR,
        nonce: "0x" + nonce.toString(16),
        to: account,
        gasPrice: 0,
        gas: 8000000,
        value: web3.utils.toHex(web3.utils.toWei("0.2", 'ether'))
      }

      const tx = new Tx(rawTx);
      tx.sign(privateKey);

      const serializedTx = tx.serialize();

      //send signed transaction
      web3.eth.sendSignedTransaction('0x' + serializedTx.toString('hex'))
        .on('receipt', receipt => {
          console.log(receipt);
          hideMessage();

       })
        .catch(console.error);
    });
  }
}

export function sendSKL() {
  let {account, balanceSKL} = store.getState().web3;
  
  if(balanceSKL > 0) {
    showMessage("You already have SKALE tokens.");
    setTimeout(function() {
    hideMessage();
  }, 3000);
  } else {
    let privateKey = new Buffer(process.env.PRIVATE_KEY_VALIDATOR, 'hex');

    const web3 = new Web3(process.env.PRIVATE_MAINNET);

    const amount = web3.utils.toHex(web3.utils.toWei("100", 'ether'));
    const erc20ABI = privateTestnetJson.skale_token_abi;
    const erc20Address = privateTestnetJson.skale_token_address;

    let contract = new web3.eth.Contract(erc20ABI, erc20Address);

    showMessage("Transfering Tokens.");

    web3.eth.getTransactionCount(process.env.ACCOUNT_VALIDATOR).then(nonce => {
      
      const rawTx = {
        from: process.env.ACCOUNT_VALIDATOR,
        nonce: "0x" + nonce.toString(16),
        gasPrice: 0,
        gas: 8000000,
        to: erc20Address,
        value: "0x0",
        data: contract.methods.transfer(account, amount).encodeABI(),
      };

      const tx = new Tx(rawTx);
      tx.sign(privateKey);

      const serializedTx = tx.serialize();

      //send signed transaction
      web3.eth.sendSignedTransaction('0x' + serializedTx.toString('hex'))
        .on('receipt', receipt => {
          console.log(receipt);
          hideMessage();

       })
        .catch(console.error);
    });
  }
}





