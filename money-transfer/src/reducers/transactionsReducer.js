const initialState = {
  transactionDataMainnet: [],
  transactionDataSchain: []
}

const transactionsReducer = (state = initialState, action) => {
  if (action.type === 'ADD_TRANSACTIONS')
  {
    return Object.assign({}, state, {
      transactionDataMainnet: action.payload.transactionMainnet.concat(state.transactionDataMainnet),
      transactionDataSchain: action.payload.transactionSchain.concat(state.transactionDataSchain),
    })
  }
  
  return state
}

export default transactionsReducer
