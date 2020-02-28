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

  if(typeof web3 !== 'undefined' && account !== "" && account > 39){
    const balance = await web3.eth.getBalance(account);

    let balanceSKL = await contract.methods.balanceOf(account).call();
    balanceSKL = web3.utils.hexToNumberString(web3.utils.numberToHex(balanceSKL));

    if(web3.utils.fromWei(balanceSKL, 'ether') >= 100){
      hideMessage();
    }

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
    sendSKL();
  } else {
    showMessage("Transfering ETH.");

    fetch(
      "https://se-api.skale.network/faucet/validator/eth/" +
        account
    ).then(response => {
        showMessage("Please wait...")
        setTimeout(function() {
          sendSKL();
        }, 10000);
      }
    );
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
    showMessage("Transfering SKL Tokens.");

    fetch(
      "https://se-api.skale.network/faucet/validator/skl/" +
        account
    ).then(response => {
        showMessage("Please wait...");
        setTimeout(function() {
          sendSKL();
        }, 10000);
      }
    );
  }
}





