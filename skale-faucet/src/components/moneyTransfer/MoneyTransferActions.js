import store from "../../store";
import Web3 from "web3";

import { showMessage, hideMessage } from "./../status/StatusActions";

const Tx = require("ethereumjs-tx");

export const UPDATE_BALANCE = "UPDATE_BALANCE";
export function updateBalance(balance) {
  return {
    type: UPDATE_BALANCE,
    payload: balance
  };
}

export const UPDATE_ACCOUNT = "UPDATE_ACCOUNT";
export function accountUpdate(account) {
  return {
    type: UPDATE_ACCOUNT,
    payload: account
  };
}

export const UPDATE_ENDPOINT = "UPDATE_ENDPOINT";
export function endpointUpdate(endpoint) {
  return {
    type: UPDATE_ENDPOINT,
    payload: endpoint
  };
}

export const UPDATE_SKALE_ID = "UPDATE_SKALE_ID";
export function skaleIdUpdate(skaleId) {
  return {
    type: UPDATE_SKALE_ID,
    payload: skaleId
  };
}

export function updateAccount(account) {
  return function(dispatch) {
    dispatch(accountUpdate(account));
  };
}

export function updateEndpoint(endpoint) {
  return function(dispatch) {
    dispatch(endpointUpdate(endpoint));
  };
}

export function updateSkaleId(skaleId) {
  return function(dispatch) {
    dispatch(skaleIdUpdate(skaleId));
  };
}

async function getBalance(dispatch) {
  let { endpoint, account } = store.getState().web3;

  const web3 = new Web3(endpoint);
  if (typeof web3 !== "undefined" && account !== "") {
    const balance = await web3.eth.getBalance(account);
    dispatch(updateBalance({ balance: web3.utils.fromWei(balance, "ether") }));
  }
}

export function refreshBalance() {
  getBalance(store.dispatch);
  setTimeout(function() {
    refreshBalance(store.dispatch);
  }, 2000);
}

export function sendETH() {
  let { endpoint, account } = store.getState().web3;

  showMessage("Transfering ETH.");

  fetch(
    "http://se-api.skale.network/faucet/sip/" +
      account +
      "/?" +
      "endpoint=" +
      encodeURIComponent(endpoint)
  ).then(response =>
    setTimeout(function() {
      hideMessage();
    }, 2000)
  );
}
