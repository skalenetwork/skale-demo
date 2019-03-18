import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import React from 'react';
import ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, browserHistory } from 'react-router'
import { Provider } from 'react-redux'
import { syncHistoryWithStore } from 'react-router-redux'
import { UserIsAuthenticated, UserIsNotAuthenticated } from './util/wrappers.js'

// Layouts
import App from './App'
import Home from './layouts/home/Home'
import Demo from './layouts/demo/Demo'
import Explorer from './layouts/explorer/Explorer'
import Payment from './layouts/payment/Payment'
import RockPaperScissors from './layouts/demo/rps/RockPaperScissors'
import SignUp from './user/layouts/signup/SignUp'
import Profile from './user/layouts/profile/Profile'

// Redux Store
import store from './store'

// Initialize react-router-redux.
const history = syncHistoryWithStore(browserHistory, store)

ReactDOM.render((
    <Provider store={store}>
      <Router history={history}>
        <Route path="/" component={App}>
          <IndexRoute component={Home} />
          <Route path="explorer" component={Explorer} />
          <Route path="payments" component={Payment} />
          <Route path="demo" component={UserIsAuthenticated(Demo)} />
          <Route path="demo/rps" component={UserIsAuthenticated(RockPaperScissors)} />
          <Route path="signup" component={UserIsNotAuthenticated(SignUp)} />
          <Route path="profile" component={UserIsAuthenticated(Profile)} />
        </Route>
      </Router>
    </Provider>
  ),
  document.getElementById('root')
)
