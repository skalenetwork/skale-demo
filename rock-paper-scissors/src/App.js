import React, { Component } from 'react'
import { Link } from 'react-router'
import { HiddenOnlyAuth, VisibleOnlyAuth } from './util/wrappers.js'
import {getWeb3, setWeb3Mainnet, setWeb3Skale } from './util/web3/getWeb3'
import store from './store'

// UI Components
import LoginButtonContainer from './user/ui/loginbutton/LoginButtonContainer'
import LogoutButtonContainer from './user/ui/logoutbutton/LogoutButtonContainer'

//Transaction Window
import Transactions from './layouts/transactions/Transactions'

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
    const OnlyAuthLinks = VisibleOnlyAuth(() =>
      <div className="navbar-nav d-block ml-auto">
        <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarSupportedContent">
          <ul className="navbar-nav ml-auto justify-content-end">
            <li className="nav-item">
              <Link to="/explorer" activeClassName="active" className="nav-link">Block Explorer</Link>
            </li>
            <li className="nav-item">
              <Link to="/payments" activeClassName="active" className="nav-link">Payments</Link>
            </li>
            <li className="nav-item">
              <Link to="/demo" activeClassName="active" className="nav-link">Games</Link>
            </li>
            <li className="nav-item">
              <Link to="/profile" activeClassName="active" className="nav-link">Profile</Link>
            </li>
            <LogoutButtonContainer />
            <li className="nav-item">
              <button type="button" onClick={(event) => this.handleClick(event)} className={"btn btn-sm btn-toggle skaled " + (skaled ? "active" : "")} id="skaledOn" data-toggle="button"aria-pressed={skaled ? "true" : "false"} autoComplete="off">
                  <div className="handle"></div>
              </button>
            </li>
          </ul>            
        </div>
      </div>
    )

    const OnlyGuestLinks = HiddenOnlyAuth(() =>
      <ul className="navbar-nav ml-auto justify-content-end">
        <li className="nav-item">
          <Link to="/explorer" activeClassName="active" className="nav-link">Block Explorer</Link>
        </li>
        <li className="nav-item">
          <Link to="/payments" activeClassName="active" className="nav-link">Payments</Link>
        </li>
        <li className="nav-item">
          <Link to="/signup" activeClassName="active" className="nav-link">Sign Up</Link>
        </li>
        <LoginButtonContainer />
        <li className="nav-item">
          <button type="button" onClick={(event) => this.handleClick(event)} className={"btn btn-sm btn-toggle skaled " + (skaled ? "active" : "")} id="skaledOn" data-toggle="button" aria-pressed={skaled ? "true" : "false"} autoComplete="off"> 
              <div className="handle"></div>
          </button>
        </li>
      </ul>
    )

    return (
      <div className="App" style={{backgroundImage: 'url(' + bg + ')'}}>
        <div className="background-blur"></div>
        <div className="content">
        <div className="header">
          <nav className="navbar navbar-expand-lg navbar-dark">
            <Link to="/" className="navbar-brand">
              <img className="logo" alt="SKALE" src={skale_logo}/>
            </Link>
            <OnlyGuestLinks />
            <OnlyAuthLinks />            
          </nav>
        </div>
          <div className="contentWindow h-100">
          {this.props.children}
          </div>
          <Transactions />
        </div>
      </div>
    );
  }
}

export default App
