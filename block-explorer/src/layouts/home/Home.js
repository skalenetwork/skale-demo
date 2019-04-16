import React, { Component } from 'react'
import { browserHistory } from 'react-router'
import ExplorerContainer from '../../components/explorer/ExplorerContainer'

class Home extends Component {

  handleSubmit(event, choice) {
    event.preventDefault()

    return browserHistory.push('/signup')
  }

  render() {
    return(
      <main className="container-fluid pt-5">
        <ExplorerContainer/>
      </main>
    )
  }
}

export default Home
