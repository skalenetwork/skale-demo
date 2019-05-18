import { browserHistory } from 'react-router'
import store from '../../../store'
import Web3 from 'web3'

export const USER_LOGGED_IN = 'USER_LOGGED_IN'
function userLoggedIn(user) {
  return {
    type: USER_LOGGED_IN,
    payload: user
  }
}

export const USE_BITSKI = 'USE_BITSKI'
function useBitski(bitski) {
  return {
    type: USE_BITSKI,
    payload: bitski
  }
}
export const USE_PORTIS = 'USE_PORTIS'
function usePortis(portis) {
  return {
    type: USE_PORTIS,
    payload: portis
  }
}

export function loginUser() {
  let {bitski} = store.getState().web3;
  return function(dispatch) {
    bitski.signIn().then(() => {
      //signed in!
      const provider = bitski.getProvider({rpcUrl: process.env.SKALE_CHAIN_TRUFFLE});
      let web3 = new Web3(provider);
      dispatch(useBitski(web3));
      browserHistory.push('/');
      return dispatch(userLoggedIn({walletAddress: "yes"}));
    });
  }
  /*let {portis} = store.getState().web3;
  return function(dispatch) {
    portis.onLogin((walletAddress, email) => {
      let web3 = new Web3(portis.provider);
      console.log(portis.provider)
      dispatch(usePortis(web3));
      browserHistory.push('/');
      return dispatch(userLoggedIn({walletAddress: walletAddress}));
    });
    portis.showPortis();
  }*/
}