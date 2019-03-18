import React, { Component } from 'react'
import ExplorerContainer from '../../components/explorer/ExplorerContainer'

class Explorer extends Component {

  render() {
    return(
      <main className="container-fluid pt-5 h-100">
        <div className="center margin50">
          <h1 className="underline-yellow">SKALE Blockchain Explorer</h1>
        </div>
        <ExplorerContainer />
      </main>
    )
  }
}

export default Explorer
