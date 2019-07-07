const initialState = {
  account: "",
  endpoint: "",
  skaleId: "",
  balance: "",
  balanceSKL: "",
}

const web3Reducer = (state = initialState, action) => {
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
  if (action.type === 'UPDATE_SKALE_ID')
  {
    return Object.assign({}, state, {
      skaleId: action.payload,
    })
  }
  if (action.type === 'UPDATE_BALANCE')
  {
    return Object.assign({}, state, {
      balance: action.payload.balance,
      balanceSKL: action.payload.balanceSKL,
    })
  }

  return state
}

export default web3Reducer
