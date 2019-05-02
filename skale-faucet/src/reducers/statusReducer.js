const initialState = {
  message: "",
  show: false,
}

const statusReducer = (state = initialState, action) => {
  if (action.type === 'SHOW_STATUS')
  {
    return Object.assign({}, state, {
      message: action.payload,
      show: true,
    })
  }
    if (action.type === 'HIDE_STATUS')
  {
    return Object.assign({}, state, initialState)
  }

  return state
}

export default statusReducer
