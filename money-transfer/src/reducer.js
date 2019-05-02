import { combineReducers } from 'redux'
import { routerReducer } from 'react-router-redux'
import web3Reducer from './reducers/web3/web3Reducer'
import statusReducer from './reducers/statusReducer'
import transactionsReducer from './reducers/transactionsReducer'


const reducer = combineReducers({
  routing: routerReducer,
  transactions: transactionsReducer,
  status: statusReducer,
  web3: web3Reducer,
})

export default reducer
