import React, { Component } from 'react'
import ValidatorTransferContainer from '../../components/validatorTransfer/ValidatorTransferContainer'
import StatusContainer from '../../components/status/StatusContainer'

class Validators extends Component {
  render() {
    return(
      <main className="container-fluid pt-2">
        <ValidatorTransferContainer/>
        <StatusContainer/>
      </main>
    )
  }
}

export default Validators
