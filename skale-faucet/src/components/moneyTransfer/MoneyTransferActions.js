import store from '../../store'
import Web3 from 'web3'

import { showMessage, hideMessage } from './../status/StatusActions'

const Tx = require('ethereumjs-tx');

export const UPDATE_ACCOUNT = 'UPDATE_ACCOUNT'
export function accountUpdate(account) {
  return {
    type: UPDATE_ACCOUNT,
    payload: account
  }
}

export const UPDATE_ENDPOINT = 'UPDATE_ENDPOINT'
export function endpointUpdate(endpoint) {
  return {
    type: UPDATE_ENDPOINT,
    payload: endpoint
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

export function updateEndpoint(endpoint) {
  return function(dispatch) {
    dispatch(endpointUpdate(endpoint));
  }
}

export function updateSkaleId(skaleId) {
  return function(dispatch) {
    dispatch(skaleIdUpdate(skaleId));
  }
}

export function sendETH() {
  let {endpoint, account} = store.getState().web3;
  
  let privateKey = new Buffer(process.env.PRIVATE_KEY, 'hex');

  const web3 = new Web3(new Web3.providers.HttpProvider(endpoint));

  showMessage("Transfering ETH.");

  web3.eth.getTransactionCount(process.env.ACCOUNT).then(nonce => {
    const rawTx = {
      from: process.env.ACCOUNT, 
      nonce: "0x" + nonce.toString(16),
      to: account,
      gasPrice: 0,
      gas: 8000000,
      value: web3.utils.toHex(web3.utils.toWei("10", 'ether'))
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





