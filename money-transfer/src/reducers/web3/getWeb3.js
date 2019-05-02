import store from '../../store'
import Web3 from 'web3'
import privateTestnetJson from "./contracts/private_skale_testnet_proxy.json"
import schainJson from "./contracts/schain_proxy.json"

export const WEB3_INITIALIZED = 'WEB3_INITIALIZED'
function web3Initialized(results) {
  return {
    type: WEB3_INITIALIZED,
    payload: results
  }
}

export let getWeb3 = new Promise(function(resolve, reject) {
  // Wait for loading completion to avoid race conditions with web3 injection timing.
  window.addEventListener('load', function(dispatch) {

    let web3 = new Web3.providers.HttpProvider(process.env.PRIVATE_MAINNET);

    // Checking if Web3 has been injected by the browser (Mist/MetaMask)
    if (typeof web3 !== 'undefined') {

      web3 = new Web3(web3);

      resolve(store.dispatch(web3Initialized({
        web3Instance: web3,
        privateTestnetJson: privateTestnetJson,
        schainJson: schainJson
      })));

    } 
  })
})