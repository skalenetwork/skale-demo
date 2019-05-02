const initialState = {
  account: "",
  endpoint: "",
  skaleId: "",
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

  return state
}

export default web3Reducer
