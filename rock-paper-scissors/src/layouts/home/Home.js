import React, { Component } from 'react'
import { browserHistory } from 'react-router'

class Home extends Component {

  handleSubmit(event) {
    event.preventDefault()

    return browserHistory.push('/demo')
  }

  render() {
    return(
      <main className="container-fluid pt-5">
        <div className="center margin50">
          <h1>Easy. Fast. Secure.</h1>
          <br/>
          <p>Easy to deploy contracts onto your SKALE chain!</p>
          <p>Fast execution of transaction for your blockchain application!</p>
          <p>Secure platform for sending, storing, and maintaining your user data!</p>
          <br/>
          <button onClick={(event) => this.handleSubmit(event)} 
            className="btn btn-outline-primary yellow-button">Start Demo</button>
        </div>
      </main>
    )
  }
}

export default Home
