import React, { Component } from 'react'
import { Link } from 'react-router'
import {getWeb3} from './util/web3/getWeb3'
// Images
import skale_logo from './assets/Skale_Logo_White.png'
import bg from './assets/demo-background.png'

// Styles
import './css/button-toggle.scss'
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
      <div className="App" style={{backgroundImage: 'url(' + bg + ')'}}>
        <div className="background-blur"></div>
        <div className="content">
        <div className="header">
          <nav className="navbar navbar-expand-lg navbar-dark">
            <Link to="/" className="navbar-brand">
              <img className="logo" alt="SKALE" src={skale_logo}/>
            </Link>           
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
