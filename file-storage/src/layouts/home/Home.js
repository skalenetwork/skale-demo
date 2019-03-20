import React, { Component } from 'react'
import { browserHistory } from 'react-router'
import FileStorageContainer from '../../components/fileStorage/FileStorageContainer'
import StatusContainer from '../../components/status/StatusContainer'

class Home extends Component {

  handleSubmit(event, choice) {
    event.preventDefault()

    return browserHistory.push('/signup')
  }

  render() {
    return(
      <main className="container-fluid pt-5">
        <div className="margin50 pt-5">
          <FileStorageContainer/>
          <StatusContainer/>
        </div>
      </main>
    )
  }
}

export default Home
