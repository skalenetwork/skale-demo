const initialState = {
  skale: false,
  web3Instance: null,
  web3Mainnet: null,
  web3SKALE: null,
  depositBoxBalance: null,
  mainnetBalance: null,
  schainBalance: null,
}

const web3Reducer = (state = initialState, action) => {
  if (action.type === 'WEB3_INITIALIZED')
  {
    return Object.assign({}, state, {
      web3Instance: action.payload.web3Instance,
      web3Mainnet: action.payload.web3Instance,
      web3SKALE: action.payload.web3SKALE,
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
      files: [],
    })
  }
  if (action.type === 'UPDATE_BALANCES')
  {
    return Object.assign({}, state, {
      depositBoxBalance: action.payload.depositBoxBalance,
      mainnetBalance: action.payload.mainnetBalance,
      schainBalance: action.payload.schainBalance,
    })
  }

  return state
}

export default web3Reducer
