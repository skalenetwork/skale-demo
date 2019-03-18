import store from '../../store'
import { PortisProvider } from 'portis';
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


  const web3 = new Web3(new PortisProvider({
   apiKey: 'bc85bd61-88da-4f5f-86df-21370919a269',
   providerNodeUrl: "http://104.248.242.35:8014"
  }));

  console.log(web3);

  let filestorage = new FilestorageClient(web3SKALE);
    
  resolve(store.dispatch(web3Initialized({
    web3: web3, 
    web3SKALE: web3SKALE,
    web3Mainnet: web3,
    account: process.env.ACCOUNT, 
    filestorage: filestorage
  })));
})