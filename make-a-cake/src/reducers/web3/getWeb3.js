import store from '../../store'
import Web3 from 'web3'

import FilestorageClient from '@skalenetwork/filestorage-js';

export const WEB3_INITIALIZED = 'WEB3_INITIALIZED'
function web3Initialized(results) {
  return {
    type: WEB3_INITIALIZED,
    payload: results
  }
}

export let getWeb3 = new Promise(function(resolve, reject) {
  const web3Provider = new Web3.providers.HttpProvider(process.env.SKALE_CHAIN);
  var web3SKALE = new Web3(web3Provider);

  let web3 = window.web3;

  web3 = new Web3(web3.currentProvider);

  //fix sendAsync issue
  web3.currentProvider.sendAsync = 
    web3.currentProvider.send;

  let filestorage = new FilestorageClient(web3SKALE);

  resolve(store.dispatch(web3Initialized({
    web3: web3, 
    web3SKALE: web3SKALE,
    web3Mainnet: web3,
    account: process.env.ACCOUNT, 
    filestorage: filestorage
  })));
})