import store from '../../store'
import Web3 from 'web3'
import { Bitski } from 'bitski'
import Portis from "@portis/web3";

import FilestorageClient from '@skalenetwork/filestorage.js';

export const WEB3_INITIALIZED = 'WEB3_INITIALIZED'
function web3Initialized(results) {
  return {
    type: WEB3_INITIALIZED,
    payload: results
  }
}

export let getWeb3 = new Promise(function(resolve, reject) {
  //add SKALE
  var web3SKALE = new Web3(process.env.SKALE_CHAIN);

  //add Bitski
  const bitski = new Bitski(process.env.BITSKI_ID, 'http://localhost:3001/callback.html');

  //add Portis
  const mySKALEChain = {
    nodeUrl: process.env.SKALE_CHAIN,
    chainId: 4,
    nodeProtocol: 'rpc',
  };
  const portis = new Portis(process.env.PORTIS_ID, mySKALEChain);

  //used for MetaMask
  let web3 = window.web3;
  web3 = new Web3(web3.currentProvider);

  //fix sendAsync issue
  web3.currentProvider.sendAsync = 
    web3.currentProvider.send;

  let filestorage = new FilestorageClient(process.env.SKALE_CHAIN, true);

  resolve(store.dispatch(web3Initialized({
    web3: web3, 
    web3SKALE: web3SKALE,
    web3Mainnet: web3,
    account: process.env.ACCOUNT, 
    filestorage: filestorage,
    bitski: bitski, 
    portis: portis, 
  })));
})