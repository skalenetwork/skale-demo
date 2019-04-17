import store from '../../store'
import Web3 from 'web3'

export const WEB3_INITIALIZED = 'WEB3_INITIALIZED'
function web3Initialized(results) {
  return {
    type: WEB3_INITIALIZED,
    payload: results
  }
}

export const WEB3_SKALE = 'WEB3_SKALE'
function web3Skale() {
  return {
    type: WEB3_SKALE
  }
}

export const WEB3_MAINNET = 'WEB3_MAINNET'
function web3Mainnet() {
  return {
    type: WEB3_MAINNET
  }
}

export function setWeb3Skale() { 
  store.dispatch(web3Skale());
}

export function setWeb3Mainnet() { 
  store.dispatch(web3Mainnet());
}

export let getWeb3 = new Promise(function(resolve, reject) {
  // Wait for loading completion to avoid race conditions with web3 injection timing.
  window.addEventListener('load', function(dispatch) {
    var web3 = window.web3;

    // Checking if Web3 has been injected by the browser (Mist/MetaMask)
    if (typeof web3 !== 'undefined') {

      //fix sendAsync issue
      web3.providers.HttpProvider.prototype.sendAsync = 
        web3.providers.HttpProvider.prototype.send;

      web3 = new Web3(web3.currentProvider);

      resolve(store.dispatch(web3Initialized({web3Instance: web3})));

    } 
  })
})