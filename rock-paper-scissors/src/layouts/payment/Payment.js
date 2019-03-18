import React, { Component } from 'react'
import PaymentContainer from '../../components/payment/PaymentContainer'

class Payment extends Component {

  render() {
    return(
      <main className="container-fluid pt-5">
        <div className="center margin50">
          <h1 className="underline-yellow mb-5">SKALE Blockchain Payments</h1>
        </div>
        <PaymentContainer />
      </main>
    )
  }
}

export default Payment
