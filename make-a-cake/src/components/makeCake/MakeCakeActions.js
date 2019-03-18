import BakeryContract from '../../../build/contracts/Bakery.json'

import store from '../../store'
import path from 'path'
import { showMessage, hideMessage } from './../status/StatusActions'
import contract from 'truffle-contract'
const Tx = require('ethereumjs-tx');

import cheesecake from './../../assets/cheesecake.png'
import fruitcake from './../../assets/fruitcake.png'
import tiramisu from './../../assets/tiramisu.png'


export async function makeCake(name){
  let {account, web3Instance, skale} = store.getState().web3;

  if(skale) {
    makeCakeSkaled(name);
  } 
  else {
    showMessage("Making Your Cake.");

    const cake = contract(BakeryContract);
    cake.setProvider(web3Instance.currentProvider);

    // Get current ethereum wallet.
    web3Instance.eth.getCoinbase((error, coinbase) => {
      // Log errors, if any.
      if (error) {
        console.error(error);
      }

      cake.deployed().then( async function(instance) {
        instance.newCake(name, {from: coinbase})
        .then(function(result) {
          updateCake();
          hideMessage();
        })
        .catch(function(result) {
          // If error...
          console.log(result);
          console.error('Wallet ' + coinbase + ' does not have enough money!')
          hideMessage();
        })
      })
    })
  }
}

export async function makeCakeSkaled(name){
  let {account, web3Instance} = store.getState().web3;
  let privateKey = new Buffer(process.env.PRIVATE_KEY, 'hex');

  showMessage("Making Your Cake.");
  const accounts = await web3Instance.eth.getAccounts()
  const [address] = accounts;

  let contract = new web3Instance.eth.Contract(BakeryContract.abi, "0x7CFd21fefb89DABCB60983c8A00665Da8654DD5C");
  let makeCake = contract.methods.newCake(name).encodeABI();  
  console.log(contract)
  //get nonce
  web3Instance.eth.getTransactionCount(account).then(nonce => {
    
    //create raw transaction
    const rawTx = {
      from: accounts[0], 
      nonce: "0x" + nonce.toString(16),
      data : makeCake,
      to: "0x7CFd21fefb89DABCB60983c8A00665Da8654DD5C",
      gasPrice: 0,
      gas: 8000000
    };

    //sign transaction
    const tx = new Tx(rawTx);
    tx.sign(privateKey);
    const serializedTx = tx.serialize();

    //send signed transaction
    web3Instance.eth.sendTransaction(rawTx).
      on('receipt', receipt => {
        console.log(receipt);
        updateCakeAgain();
        hideMessage();
     }).
      catch(console.error);
  });
}


export async function upload(fileName, fileSize, fileData){
  let {web3Instance, account, filestorage} = store.getState().web3;
  let privateKey = "0x" + process.env.PRIVATE_KEY;
  showMessage("Adding new ingredient.");

  await filestorage.uploadFile(account, fileName, fileSize, fileData, true, privateKey);

  hideMessage();
}

export async function deleteFile(address, fileName) {
  let filestorage = store.getState().web3.filestorage;
  let privateKey = "0x" + process.env.PRIVATE_KEY;
  showMessage("Deleting your ingredient.");
  await filestorage.deleteFile(address, fileName, true, privateKey);
  getFiles();
  hideMessage();
}

export async function download(link, index) {
  let filestorage = store.getState().web3.filestorage;
  showMessage("Downloading your file.");
  await filestorage.downloadFileIntoBrowser(link, true);
  hideMessage();
}

export async function preLoad(link) {
  let {ingredients} = store.getState().web3;
  let filestorage = store.getState().web3.filestorage;
  showMessage("Adding your ingredient.");
  let file = await filestorage.downloadFileIntoBuffer(link, true);
  document.getElementById("ingredient_" + ingredients).src = 'data:image/png;base64,' + file.toString('base64');
  store.dispatch(useIngredient("1"));
  hideMessage();
}

export async function addIngredient(link) {
  let {ingredients} = store.getState().web3;
  document.getElementById("ingredient_" + ingredients).src = link;
  store.dispatch(useIngredient("1"));
  hideMessage();
}

export async function getFiles(){
  let {account, filestorage } = store.getState().web3;
  let files = await filestorage.getFileInfoByAddress(account);
  let newFiles = files.filter(function (file) {
  let ingredient = file.name.match(/(cheese|fruit|chocolate)/i) ? true : false;
    return ingredient;
  });
  store.dispatch(updateFiles(newFiles));
}


export const UPDATE_FILES = 'UPDATE_FILES'

function updateFiles(results) {
  return {
    type: UPDATE_FILES,
    payload: results
  }
}

export const USE_INGREDIENT = 'USE_INGREDIENT'

function useIngredient(results) {
  return {
    type: USE_INGREDIENT,
    payload: results
  }
}

function updateCake() {
  if (document.getElementById("ingredient_3").src.match(/(cheese)/i)){
    document.getElementById("cakeMade").src = cheesecake;
  } else {
    document.getElementById("cakeMade").src = fruitcake;
  }
  document.getElementById("cakeShow").classList.remove("disable");
}

function updateCakeAgain() {
  document.getElementById("cakeMade").src = tiramisu;
  document.getElementById("cakeShow").classList.remove("disable");
}





