import { combineReducers } from 'redux'
import { routerReducer } from 'react-router-redux'
import gameReducer from './util/game/gameReducer'
import web3Reducer from './util/web3/web3Reducer'

const reducer = combineReducers({
  routing: routerReducer,
  web3: web3Reducer,
  game: gameReducer
})

export default reducer
