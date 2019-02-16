const initialState = {
  data: [],
  lastBlock: 0,
  dataSkale: [],
  lastBlockSkale: 0
}

const transactionReducer = (state = initialState, action) => {
  if (action.type === 'DISPLAY_TRANSACTIONS')
  {
    const startLength = state.data.length > 43 ? state.data.length - 44 : 0;
    state.data = state.data.slice(startLength, state.data.length);
    return Object.assign({}, state, {
      data: action.payload.blocks !== null ? 
        state.data.concat(action.payload.blocks) : state.data,
      lastBlock: action.payload.lastBlock
    })
  }
  if (action.type === 'DISPLAY_SKALE_TRANSACTIONS')
  {
    return Object.assign({}, state, {
      dataSkale: action.payload.blocks !== null ? 
        state.data.concat(action.payload.blocks) : state.data,
      lastBlockSkale: action.payload.lastBlock
    })
  }
  
  return state
}

export default transactionReducer
