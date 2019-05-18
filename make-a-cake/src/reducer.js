import { combineReducers } from 'redux'
import { routerReducer } from 'react-router-redux'
import web3Reducer from './reducers/web3/web3Reducer'
import statusReducer from './reducers/statusReducer'
import userReducer from './user/userReducer'


const reducer = combineReducers({
  routing: routerReducer,
  web3: web3Reducer,
  user: userReducer,
  status: statusReducer,
})

export default reducer
