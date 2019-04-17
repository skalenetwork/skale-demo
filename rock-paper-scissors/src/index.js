import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import React from 'react';
import ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, browserHistory } from 'react-router'
import { Provider } from 'react-redux'
import { syncHistoryWithStore } from 'react-router-redux'

// Layouts
import App from './App'
import Home from './layouts/home/Home'
import Demo from './layouts/demo/Demo'
import RockPaperScissors from './layouts/demo/rps/RockPaperScissors'

// Redux Store
import store from './store'

// Initialize react-router-redux.
const history = syncHistoryWithStore(browserHistory, store)

ReactDOM.render((
    <Provider store={store}>
      <Router history={history}>
        <Route path="/" component={App}>
          <IndexRoute component={Home} />
          <Route path="demo" component={Demo} />
          <Route path="demo/rps" component={RockPaperScissors} />
        </Route>
      </Router>
    </Provider>
  ),
  document.getElementById('root')
)
