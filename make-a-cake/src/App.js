import React, { Component } from 'react'
import { Link } from 'react-router'
import { getWeb3 } from './reducers/web3/getWeb3'
import { HiddenOnlyAuth, VisibleOnlyAuth } from './utils/wrappers.js'
import { getFiles } from './components/makeCake/MakeCakeActions'
import store from './store'

// UI Components
import LoginButtonContainer from './user/ui/loginbutton/LoginButtonContainer'
import LogoutButtonContainer from './user/ui/logoutbutton/LogoutButtonContainer'

// Images
import skale_logo from './assets/Skale_Logo_White.png'
import bg from './assets/background.png'

// Styles
import './App.scss'

// Initialize web3 and set in Redux.
sessionStorage.removeItem('skale_user');

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
    const OnlyAuthLinks = VisibleOnlyAuth(() =>
      <div className="navbar-nav d-block ml-auto">
        <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarSupportedContent">
          <ul className="navbar-nav ml-auto justify-content-end">
            <li className="nav-item">
              <Link to="/demo" activeClassName="active" className="nav-link">Games</Link>
            </li>
            <LogoutButtonContainer />
          </ul>            
        </div>
      </div>
    )

    const OnlyGuestLinks = HiddenOnlyAuth(() =>
      <ul className="navbar-nav ml-auto justify-content-end">
        <LoginButtonContainer />
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
            <button type="button" onClick={(event) => this.handleClick(event)} className={"btn btn-sm btn-toggle skaled mt-4 " + (skaled ? "active" : "")} id="skaledOn" data-toggle="button"aria-pressed={skaled ? "true" : "false"} autoComplete="off">
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
