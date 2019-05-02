import store from '../../store'
import Web3 from 'web3'

import { showMessage, hideMessage } from './../status/StatusActions'

const Tx = require('ethereumjs-tx');

export const UPDATE_BALANCES = 'UPDATE_BALANCES'
export function updateBalances(balances) {
  return {
    type: UPDATE_BALANCES,
    payload: balances
  }
}

export const ADD_TRANSACTIONS = 'ADD_TRANSACTIONS'
export function addTransactions(transaction) {
  return {
    type: ADD_TRANSACTIONS,
    payload: transaction
  }
}

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

export const UPDATE_ENDPOINT_SKALE = 'UPDATE_ENDPOINT_SKALE'
export function endpointSkaleUpdate(endpointSkale) {
  return {
    type: UPDATE_ENDPOINT_SKALE,
    payload: endpointSkale
  }
}

export const UPDATE_PRIVATE_KEY = 'UPDATE_PRIVATE_KEY'
export function privateKeyUpdate(privateKey) {
  return {
    type: UPDATE_PRIVATE_KEY,
    payload: privateKey
  }
}

export const UPDATE_SKALE_ID = 'UPDATE_SKALE_ID'
export function skaleIdUpdate(skaleId) {
  return {
    type: UPDATE_SKALE_ID,
    payload: skaleId
  }
}

export const UPDATE_TOKEN_ADDRESS = 'UPDATE_TOKEN_ADDRESS'
export function tokenAddressUpdate(tokenAddress) {
  return {
    type: UPDATE_TOKEN_ADDRESS,
    payload: tokenAddress
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

export function updateEndpointSkale(endpointSkale) {
  return function(dispatch) {
    dispatch(endpointSkaleUpdate(endpointSkale));
  }
}

export function updatePrivateKey(privateKey) {
  return function(dispatch) {
    dispatch(privateKeyUpdate(privateKey));
  }
}

export function updateSkaleId(skaleId) {
  return function(dispatch) {
    dispatch(skaleIdUpdate(skaleId));
  }
}

export function updateTokenAddress(tokenAddress) {
  return function(dispatch) {
    dispatch(tokenAddressUpdate(tokenAddress));
  }
}

async function getBalances(dispatch) {
  let {web3Instance, account, endpointSkale, privateTestnetJson, tokenManagerAddress} = store.getState().web3;

  const web3Provider = new Web3.providers.HttpProvider(endpointSkale);
  const web3SKALE = new Web3(web3Provider);

  const depositBoxAddress = privateTestnetJson.deposit_box_address;

  let schainBalance = await web3SKALE.eth.getBalance(account);
  let mainnetBalance = await web3Instance.eth.getBalance(account);
  let depositBoxBalance = await web3Instance.eth.getBalance(depositBoxAddress);
  let tokenManagerBalance = await web3SKALE.eth.getBalance(tokenManagerAddress);
  dispatch(updateBalances({
    depositBoxBalance: web3Instance.utils.fromWei(depositBoxBalance, 'ether'),
    mainnetBalance: web3Instance.utils.fromWei(mainnetBalance, 'ether'),
    schainBalance: web3Instance.utils.fromWei(schainBalance, 'ether'),
    tokenManagerBalance: web3Instance.utils.fromWei(tokenManagerBalance, 'ether'),    
  }));

}

export function refreshBalances() {
  getBalances(store.dispatch);
  setTimeout(function() {
    refreshBalances(store.dispatch);
  }, 2000);
}

export function deposit(amount) {
  let {privateKey, endpoint, skaleId, account, privateTestnetJson} = store.getState().web3;
  
  privateKey = new Buffer(privateKey, 'hex')
  let privateSkaleTestnetEndpoint = endpoint;
  let schainID = skaleId;

  const depositBoxAddress = privateTestnetJson.deposit_box_address;
  const abi = privateTestnetJson.deposit_box_abi;

  const web3 = new Web3(new Web3.providers.HttpProvider(privateSkaleTestnetEndpoint));

  let contract = new web3.eth.Contract(abi, depositBoxAddress);

  //prepare the smart contract function deposit(string schainID, address to)
  let deposit = contract.methods.deposit(schainID, account).encodeABI();  
  showMessage("Depositing Funds.");
  //get nonce
  web3.eth.getTransactionCount(account).then(nonce => {
    
    //create raw transaction
    const rawTx = {
      from: account, 
      nonce: "0x" + nonce.toString(16),
      data : deposit,
      to: depositBoxAddress,
      gasPrice: 0,
      gas: 8000000,
      value: web3.utils.toHex(web3.utils.toWei(amount, 'ether'))
    }

    //sign transaction
    const tx = new Tx(rawTx);
    tx.sign(privateKey);
    const serializedTx = tx.serialize();

    let recorded = false;

    //send signed transaction
    web3.eth.sendSignedTransaction('0x' + serializedTx.toString('hex'))
      .on('receipt',  receipt => {
        if(!recorded){
          hideMessage();
          recorded = true;
          let transactions = [];
          receipt.amount = amount;
          receipt.from = account;
          transactions.push(receipt);
          store.dispatch(addTransactions({
            transactionMainnet: transactions,
            transactionSchain: []
          }));
          getBalances(store.dispatch);
        }
     })
      .catch(console.error);
  });

}

export function exit(amount) {
  let {privateKey, endpointSkale, skaleId, account, schainJson, tokenManagerAddress} = store.getState().web3;
  
  privateKey = new Buffer(privateKey, 'hex');
  let schainEndpoint = endpointSkale;

  const abi = schainJson.token_manager_abi;

  const web3 = new Web3(new Web3.providers.HttpProvider(schainEndpoint));

  let contract = new web3.eth.Contract(abi, tokenManagerAddress);

  let exitToMain = contract.methods
    .exitToMain(account, web3.utils.fromAscii("[YOUR_MESSAGE]")).encodeABI();  

  showMessage("Exiting to Mainnet.");

  web3.eth.getTransactionCount(account).then(nonce => {
    const rawTx = {
      from: account, 
      nonce: "0x" + nonce.toString(16),
      data : exitToMain,
      to: tokenManagerAddress,
      gasPrice: 0,
      gas: 8000000,
      value: web3.utils.toHex(web3.utils.toWei(amount, 'ether'))
    }

    const tx = new Tx(rawTx);
    tx.sign(privateKey);

    const serializedTx = tx.serialize();

    let recorded = false;

    //send signed transaction
    web3.eth.sendSignedTransaction('0x' + serializedTx.toString('hex'))
      .on('receipt', receipt => {
        if(!recorded){
          hideMessage();
          recorded = true;
          let transactions = [];
          receipt.amount = amount;
          receipt.from = account;
          transactions.push(receipt);
          store.dispatch(addTransactions({
            transactionMainnet: [],
            transactionSchain: transactions
          }));
          getBalances(store.dispatch);
        }
          
     })
      .catch(console.error);
  });
}


