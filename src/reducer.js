import { combineReducers } from 'redux'
import { routerReducer } from 'react-router-redux'
import userReducer from './user/userReducer'
import gameReducer from './util/game/gameReducer'
import transactionReducer from './util/transaction/transactionReducer'
import web3Reducer from './util/web3/web3Reducer'

const reducer = combineReducers({
  routing: routerReducer,
  user: userReducer,
  web3: web3Reducer,
  game: gameReducer,
  transactions: transactionReducer
})

export default reducer
