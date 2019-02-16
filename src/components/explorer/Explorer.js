import React, { Component } from 'react'
import Cube from '../blockAnimations/cube'

class Explorer extends Component {
  constructor(props) {
    super(props)
    this.state = {
      transactions: this.props.transactions,
      skaleTransactions: this.props.skaleTransactions,
      isSkale: this.props.isSkale
    }
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevState.transactions !== prevProps.transactions) {
      this.setState({ transactions: prevProps.transactions });
    }
    if (prevState.skaleTransactions !== prevProps.skaleTransactions) {
      this.setState({ skaleTransactions: prevProps.skaleTransactions });
    }
    if (prevState.isSkale !== prevProps.isSkale) {
      this.setState({ isSkale: prevProps.isSkale });
    }
  } 

  render() {
    const transactions = this.state.isSkale ? 
      this.state.skaleTransactions : this.state.transactions;
    const reverseData = transactions.map((v, k) => (Object.assign({}, v, v)));
    const indexName = this.state.isSkale ? "skale_" : "mainnet_"; 
    return(
      <div className="row h-100">
        <div className="col-12  h-100">
          <div className="py-5 h-100">
            {reverseData.reverse().map((data, index) => {
              return (
                <div className="d-inline-block" key={indexName + index} data-id={indexName + index}>
                  <Cube blockNumber={data.number}
                        date={new Date(data.timestamp * 1000).toString()
                          .split(' ').slice(1,5).join(' ')}
                        count={data.cntTransactions}
                        gasUsed={data.gasUsed}
                        hash={data.hash}
                        parentHash={data.parentHash}
                  />
                </div>
                )
              })}
          </div>
        </div>  
      </div>
    )
  }
}

export default Explorer
