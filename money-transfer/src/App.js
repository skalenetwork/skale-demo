import React, { Component } from 'react'
import {getWeb3} from './reducers/web3/getWeb3'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faSearch } from '@fortawesome/free-solid-svg-icons'

library.add(faSearch)

// Images
import skale_logo from './assets/SKALE_money-transfer.png'

// Styles
import './App.scss'

// Initialize web3 and set in Redux.
getWeb3
.then(results => {
  console.log('Web3 initialized!')
})
.catch(() => {
  console.log('Error in web3 initialization.')
})

class App extends Component {
  render() {
    return (
      <div className="App">
        <div className="content">
        <div className="header">
          <nav className="navbar navbar-expand-lg navbar-dark">
            <a href="/" className="navbar-brand">
              <img className="logo" alt="SKALE" src={skale_logo}/>
            </a>
          </nav>
        </div>
          <div className="contentWindow h-100">
          {this.props.children}
          </div>
        </div>
      </div>
    );
  }
}

export default App
