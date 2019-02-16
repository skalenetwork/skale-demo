import React, { Component } from 'react'

class TransactionsWindow extends Component {
  constructor(props) {
    super(props)
    this.state = {
      transactions: this.props.transactions,
      skaleTransactions: this.props.skaleTransactions,
      isSkale: this.props.isSkale
    }
  }

  componentDidMount() {
    this.props.onGetBlocks();
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
      <div className="col-12 scroll">
        <h4 className="underline-yellow">Recent Blocks</h4>
            <div className="accordion pt-3" id="userBlock">
            {reverseData.reverse().map((data, index) => {
              return (
                <div key={indexName + index} data-id={indexName + index} className="card rounded bg-cloud">
                  <div className="card-header" id={"headingUser_" + indexName + index}>
                    <h5 className="mb-0">
                      <button className="btn btn-link" type="button" data-toggle="collapse" data-target={"#collapseUser_" + indexName + index} aria-expanded="true" aria-controls={"collapseUser_" + indexName + index}>
                        <h5><strong>Block # </strong>{data.number} </h5>
                      </button>
                    </h5>
                  </div>

                  <div id={"collapseUser_" + indexName + index} className="collapse" aria-labelledby={"headingUser_" + indexName + index} data-parent="#userBlock">
                    <div className="card-body">
                      <p><strong><span className="border-bottom border-dark">
                        Date:</span></strong> {
                          new Date(data.timestamp * 1000).toString()
                          .split(' ').slice(1,5).join(' ')}
                      </p>
                      <p><strong><span className="border-bottom border-dark">
                        Transaction Count:</span></strong> {data.cntTransactions}
                      </p>
                      <p><strong><span className="border-bottom border-dark">
                        Gas Used:</span></strong> {data.gasUsed}
                      </p>
                      <p><strong><span className="border-bottom border-dark">
                        Hash:</span></strong> {data.hash}
                      </p>
                      <p><strong><span className="border-bottom border-dark">
                        Parent Hash:</span></strong> {data.parentHash}
                      </p>
                    </div>
                  </div>
                </div>
                )
              })}
            </div>
      </div>  
      </div>
    )
  }
}

export default TransactionsWindow
