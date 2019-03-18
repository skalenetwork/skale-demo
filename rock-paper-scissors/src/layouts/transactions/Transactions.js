import React, { Component } from 'react'
import TransactionsWindowContainer from '../../components/transactions/TransactionsWindowContainer'

class Transactions extends Component {

  render() {
    return(
      <div className="transactions">
        <TransactionsWindowContainer />
      </div>
    )
  }
}

export default Transactions
