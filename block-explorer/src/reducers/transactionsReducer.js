const initialState = {
  blockData: [],
  lastBlock: -1,
  transactionData: [],
  filter: true
}

const transactionsReducer = (state = initialState, action) => {
  if (action.type === 'ADD_TRANSACTIONS')
  {
    return Object.assign({}, state, {
      transactionData: action.payload.reverse ? action.payload.transactionData.concat(state.transactionData) : state.transactionData.concat(action.payload.transactionData)
    })
  }
  if (action.type === 'ADD_BLOCKS')
  {
    return Object.assign({}, state, {
      blockData: action.payload.reverse ? action.payload.blocks.concat(state.blockData) : state.blockData.concat(action.payload.blocks),
      lastBlock: action.payload.lastBlock !== "" ? action.payload.lastBlock : state.lastBlock
    })
  } 

  if (action.type === 'FILTER')
  {
    return Object.assign({}, state, {
      filter: action.payload
    })
  } 
  
  return state
}

export default transactionsReducer
