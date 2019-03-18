import React, { Component } from 'react'
import { Link } from 'react-router'
import { getWeb3 } from './reducers/web3/getWeb3'
import { getFiles } from './components/makeCake/MakeCakeActions'
import store from './store'

// Images
import skale_logo from './assets/Skale_Logo_White.png'
import bg from './assets/background.png'

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
  getFiles();
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
      <div className="App" style={{backgroundImage: 'url(' + bg + ')'}}>
        <div className="content">
        <div className="header">
          <nav className="navbar navbar-expand-lg navbar-dark">
            <Link to="/" className="navbar-brand">
              <img className="logo" alt="SKALE" src={skale_logo}/>
            </Link>
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
