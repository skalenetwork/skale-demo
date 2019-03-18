const initialState = {
  skale: false,
  web3Instance: null,
  web3Mainnet: null,
  web3SKALE: null,
  web3KTM: null,
  account: {address: null, balance: 0},
  ktmAccount: {address: null, balance: 0},
}

const web3Reducer = (state = initialState, action) => {
  if (action.type === 'WEB3_INITIALIZED')
  {
    return Object.assign({}, state, {
      web3Instance: state.skale ? 
        action.payload.web3SKALE : action.payload.web3Instance,
      web3Mainnet: action.payload.web3Instance,
      web3SKALE: action.payload.web3SKALE,
      web3KTM: action.payload.web3KTM,
      account: action.payload.account,
      ktmAccount: action.payload.ktmAccount,
    })
  }
  if (action.type === 'WEB3_SKALE')
  {
    return Object.assign({}, state, {
      web3Instance: state.web3SKALE,
      skale: true,
    })
  }
  if (action.type === 'WEB3_MAINNET')
  {
    return Object.assign({}, state, {
      web3Instance: state.web3Mainnet,
      skale: false,
    })
  }

  return state
}

export default web3Reducer
