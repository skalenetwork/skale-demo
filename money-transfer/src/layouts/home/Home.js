import React, { Component } from 'react'
import { browserHistory } from 'react-router'
import MoneyTransferContainer from '../../components/moneyTransfer/MoneyTransferContainer'
import StatusContainer from '../../components/status/StatusContainer'

class Home extends Component {

  handleSubmit(event, choice) {
    event.preventDefault()

    return browserHistory.push('/signup')
  }

  render() {
    return(
      <main className="container-fluid pt-2">
        <MoneyTransferContainer/>
        <StatusContainer/>
      </main>
    )
  }
}

export default Home
