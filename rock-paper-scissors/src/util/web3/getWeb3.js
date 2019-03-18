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

function getKTMAccount(web3, results, dispatch, resolve) {
  var account = {address: "0x936Fd804FEDf1cd42606b8ab72Ee2C86895C974b"};
  web3.eth.getBalance("0x936Fd804FEDf1cd42606b8ab72Ee2C86895C974b")
    .then(function(balance) {
      account.balance = parseFloat(web3.utils.fromWei(balance, 'ether'));
      results.ktmAccount = account;
      console.log(results);
      resolve(store.dispatch(web3Initialized(results)));
  }).catch(function(err) {
    console.log(err.message);
    results.ktmAccount = account;

    console.log('Injected web3 detected.');
    resolve(store.dispatch(web3Initialized(results)));
  }); 
}

export let getWeb3 = new Promise(function(resolve, reject) {
  // Wait for loading completion to avoid race conditions with web3 injection timing.
  window.addEventListener('load', function(dispatch) {
    var results, account;
    var web3 = window.web3;

    var providerSKALE = new Web3.providers.HttpProvider(process.env.SKALE_CHAIN)
    var web3SKALE = new Web3(providerSKALE);

    var providerKTM = new Web3.providers.HttpProvider(process.env.KTM)
    var web3KTM = new Web3(providerKTM);


    // Checking if Web3 has been injected by the browser (Mist/MetaMask)
    if (typeof web3 !== 'undefined') {
      // Use Mist/MetaMask's provider.
      web3 = new Web3(web3.currentProvider);
       // Get current ethereum wallet.
      web3.eth.getCoinbase((error, coinbase) => {
        // Log errors, if any.
        if (error) {
          console.error(error);
        }
        account = {address: coinbase};
        web3.eth.getBalance(account.address)
          .then(function(balance) {
            account.balance = parseFloat(web3.utils.fromWei(balance, 'ether'));
            results = {
              web3Instance: web3,
              web3SKALE: web3SKALE,
              web3KTM: web3KTM,
              account: account
            }
            getKTMAccount(web3KTM, results, dispatch, resolve);
        }).catch(function(err) {
          console.log(err.message);
          results = {
            web3Instance: web3,
            web3SKALE: web3SKALE,
            web3KTM: web3KTM,
            account: account
          }
          getKTMAccount(web3KTM, results, dispatch, resolve);
        }); 
      });
    } 
  })
})