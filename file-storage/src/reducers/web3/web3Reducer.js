const initialState = {
  web3Instance: null,
  account: null,
  filestorage: null,
  files: [],
}

const web3Reducer = (state = initialState, action) => {
  if (action.type === 'WEB3_INITIALIZED')
  {
    return Object.assign({}, state, {
      web3Instance: action.payload.web3,
      account: action.payload.account,
      filestorage: action.payload.filestorage,
    })
  }
  if (action.type === 'UPDATE_FILES')
  {
    return Object.assign({}, state, {
      files: action.payload
    })
  }

  return state
}

export default web3Reducer
