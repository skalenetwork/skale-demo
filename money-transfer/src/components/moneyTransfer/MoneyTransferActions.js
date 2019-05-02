import store from '../../store'
import Web3 from 'web3'
import privateTestnetJson from "./contracts/private_skale_testnet_proxy.json"
import schainJson from "./contracts/schain_proxy.json"

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

async function getBalances(dispatch) {
  let {web3Instance, web3SKALE} = store.getState().web3;

  let account = process.env.ACCOUNT;

  const depositBoxAddress = privateTestnetJson.deposit_box_address;
  const tokenManagerAddress = schainJson.token_manager_address;

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
  let privateKey = new Buffer(process.env.PRIVATE_KEY, 'hex')
  let account = process.env.ACCOUNT;
  let privateSkaleTestnetEndpoint = process.env.PRIVATE_MAINNET;
  let schainID = process.env.SKALE_ID;

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
  let privateKey = new Buffer(process.env.PRIVATE_KEY, 'hex')
  let account = process.env.ACCOUNT;
  let schainEndpoint = process.env.SKALE_CHAIN;

  const tokenManagerAddress = schainJson.token_manager_address;
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


