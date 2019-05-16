import React, { Component } from 'react'
import { Link } from 'react-router'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faSearch } from '@fortawesome/free-solid-svg-icons'
import store from './store'

library.add(faSearch)

// Images
import skale_logo from './assets/SKALE_block_explorer.png'

// Styles
import './App.scss'

const FILTER = 'FILTER'
function filter(data) {
  return {
    type: FILTER,
    payload: data
  }
}

function setFilter(data) { 
  store.dispatch(filter(data));
}

class App extends Component {

  handleClick (event) {
    event.preventDefault();
      var filter = document.getElementById("filter").getAttribute('aria-pressed');
    if (filter === "true"){
      setFilter(true);
    } else {
      setFilter(false);
    } 
  }

  render() {
    const {filter} = store.getState().transactions;
    return (
      <div className="App">
        <div className="content">
        <div className="header">
          <nav className="navbar navbar-expand-lg navbar-dark">
            <Link to="/" className="navbar-brand">
              <img className="logo" alt="SKALE" src={skale_logo}/>
            </Link>
              <div className="d-inline text-muted ml-auto">Filter Empty Blocks <button type="button" onClick={(event) => this.handleClick(event)} className={"ml-2 btn btn-sm btn-toggle ml-auto skaled mt-4 " + (filter ? "active" : "")} id="filter" data-toggle="button"aria-pressed={filter ? "true" : "false"} autoComplete="off">
                  <div className="handle"></div>
              </button>
              </div>
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
