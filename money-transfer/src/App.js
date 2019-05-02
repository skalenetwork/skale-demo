import React, { Component } from 'react'
import {getWeb3} from './reducers/web3/getWeb3'
import store from './store'
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

const WEB3_SKALE = 'WEB3_SKALE'
function web3Skale() {
  return {
    type: WEB3_SKALE
  }
}

const WEB3_MAINNET = 'WEB3_MAINNET'
function web3Mainnet() {
  return {
    type: WEB3_MAINNET
  }
}

function setWeb3Skale() { 
  store.dispatch(web3Skale());
}

function setWeb3Mainnet() { 
  store.dispatch(web3Mainnet());
}

class App extends Component {

  handleClick (event) {
    event.preventDefault();
      var skaleOn = document.getElementById("skaledOn").getAttribute('aria-pressed');
    if (skaleOn === "true"){
      setWeb3Skale();
    } else {
      setWeb3Mainnet();
    }
    
  }

  render() {
    const skaled = store.getState().web3.skale;
    return (
      <div className="App">
        <div className="content">
        <div className="header">
          <nav className="navbar navbar-expand-lg navbar-dark">
            <a href="/" className="navbar-brand">
              <img className="logo" alt="SKALE" src={skale_logo}/>
            </a>
            <button type="button" onClick={(event) => this.handleClick(event)} className={"btn btn-sm btn-toggle ml-auto skaled mt-4 " + (skaled ? "active" : "")} id="skaledOn" data-toggle="button"aria-pressed={skaled ? "true" : "false"} autoComplete="off">
                  <div className="handle"></div>
              </button>
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
