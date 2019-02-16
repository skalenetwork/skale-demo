const initialState = {
  winner: "",
  displayWinner: "",
}

const gameReducer = (state = initialState, action) => {
  if (action.type === 'DISPLAY_WINNER')
  {
    return Object.assign({}, state, {
      winner: action.payload
    })
  }

  if (action.type === 'SHOW_WINNER_SCREEN')
  {
    return Object.assign({}, state, {
      displayWinner: action.payload
    })
  }

  return state
}

export default gameReducer
