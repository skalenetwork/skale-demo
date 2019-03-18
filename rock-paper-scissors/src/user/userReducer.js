function getSessionUserData() {
  var session = sessionStorage.getItem("skale_user");
  if(session){
    return {
      data: JSON.parse(session)
    }
  } else {
    return {
      data: null
    }
  }
}

const initialState = getSessionUserData()

const userReducer = (state = initialState, action) => {
  if (action.type === 'USER_LOGGED_IN' || action.type === 'USER_UPDATED')
  {
    //persist user login
    sessionStorage.setItem('skale_user', JSON.stringify(action.payload));

    return Object.assign({}, state, {
      data: action.payload
    })
  }

  if (action.type === 'USER_LOGGED_OUT')
  {
    sessionStorage.removeItem('skale_user');
    return Object.assign({}, state, {
      data: null
    })
  }

  return state
}

export default userReducer
