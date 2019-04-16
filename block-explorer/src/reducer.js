import { combineReducers } from 'redux'
import { routerReducer } from 'react-router-redux'
import transactionsReducer from './reducers/transactionsReducer'


const reducer = combineReducers({
  routing: routerReducer,
  transactions: transactionsReducer,
})

export default reducer
