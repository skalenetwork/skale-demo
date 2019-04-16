const initialState = {
  blockData: [],
  lastBlock: -1,
  transactionData: []
}

const transactionsReducer = (state = initialState, action) => {
  if (action.type === 'ADD_TRANSACTIONS')
  {
      console.log(action.payload)
    return Object.assign({}, state, {
      transactionData: action.payload.concat(state.transactionData)
    })
  }
  if (action.type === 'ADD_BLOCKS')
  {
    return Object.assign({}, state, {
      blockData: action.payload.blocks.concat(state.blockData),
      lastBlock: action.payload.lastBlock
    })
  }
  
  return state
}

export default transactionsReducer
