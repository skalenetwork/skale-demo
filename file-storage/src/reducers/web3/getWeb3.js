import store from '../../store'
import Web3 from 'web3'

import FilestorageClient from '@skalenetwork/filestorage.js';

export const WEB3_INITIALIZED = 'WEB3_INITIALIZED'
function web3Initialized(results) {
  return {
    type: WEB3_INITIALIZED,
    payload: results
  }
}

export let getWeb3 = new Promise(function(resolve, reject) {
  let web3 = new Web3(process.env.SKALE_CHAIN);

  let filestorage = new FilestorageClient(process.env.SKALE_CHAIN, true);
    
  resolve(store.dispatch(web3Initialized({
    web3: web3, 
    account: process.env.ACCOUNT, 
    filestorage: filestorage
  })));
})