const initialState = {
  skale: false,
  web3Instance: null,
  depositBoxBalance: null,
  tokenManagerBalance: null,
  mainnetBalance: null,
  schainBalance: null,
  account: process.env.ACCOUNT,
  endpoint: process.env.PRIVATE_MAINNET,
  endpointSkale: process.env.SKALE_CHAIN,
  privateKey: process.env.PRIVATE_KEY,
  skaleId: process.env.SKALE_ID,
  tokenManagerAddress: process.env.TOKEN_MANAGER_ADDRESS,
  privateTestnetJson: null,
  schainJson: null
}

const web3Reducer = (state = initialState, action) => {
  if (action.type === 'WEB3_INITIALIZED')
  {
    return Object.assign({}, state, {
      web3Instance: action.payload.web3Instance,
      privateTestnetJson: action.payload.privateTestnetJson,
      schainJson: action.payload.schainJson
    })
  }
  if (action.type === 'UPDATE_BALANCES')
  {
    return Object.assign({}, state, {
      depositBoxBalance: action.payload.depositBoxBalance,
      tokenManagerBalance: action.payload.tokenManagerBalance,
      mainnetBalance: action.payload.mainnetBalance,
      schainBalance: action.payload.schainBalance,
    })
  }
  if (action.type === 'UPDATE_ACCOUNT')
  {
    return Object.assign({}, state, {
      account: action.payload
    })
  }
  if (action.type === 'UPDATE_ENDPOINT')
  {
    return Object.assign({}, state, {
      endpoint: action.payload,
    })
  }
  if (action.type === 'UPDATE_ENDPOINT_SKALE')
  {
    return Object.assign({}, state, {
      endpointSkale: action.payload,
    })
  }
  if (action.type === 'UPDATE_PRIVATE_KEY')
  {
    return Object.assign({}, state, {
      privateKey: action.payload,
    })
  }
  if (action.type === 'UPDATE_SKALE_ID')
  {
    return Object.assign({}, state, {
      skaleId: action.payload,
    })
  }
  if (action.type === 'UPDATE_TOKEN_ADDRESS')
  {
    return Object.assign({}, state, {
      tokenManagerAddress: action.payload,
    })
  }

  return state
}

export default web3Reducer
