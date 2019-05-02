import React, { Component } from 'react'

import './styles.scss';

class SKALEChainTransactions extends Component {

  render() {
    const {transactionData} = this.props;
    return(
      <div className="accordion" id="txBlock">
        <div className="list-group border-grey">
        <button type="button" className="list-group-item list-group-item-action background-grey border-between text-white">
          <div className="row">
            <div className="col-sm-12">
              <h4 className="mb-3">Transactions</h4>
            </div>
          </div>
          <div className="row">
            <div className="col-10">
              Account
            </div>
            <div className="col-2 text-right">
              Amount
            </div>
          </div>
          </button>
          <div className="multi-collapse data-list">
            {transactionData.map((data, index) => {
              return (
                <div key={index} data-id={index}>
                  <button className={"list-group-item list-group-item-action blue-button border-between " + ((index % 2) ? "background-grey" : "")} type="button" data-toggle="collapse" data-target={"#collapseTx_" + index} aria-expanded="true" aria-controls={"truncate-text collapseTx_" + index}>
                      <div className="row">
                        <div className="col-10">
                          {data.from}
                        </div>
                        <div className="col-2 text-right">
                          {data.amount}
                        </div>
                      </div>
                  </button>
                  <div id={"collapseTx_" + index} className={"collapse transactionData pl-4 pt-1 pb-4 border-blue-top " + ((index % 2) ? "background-grey" : "")} aria-labelledby={"headingTx_" + index} data-parent="#txBlock">
                      <div><strong><span className="border-bottom border-dark">
                        Hash:</span></strong> {data.transactionHash}
                      </div>
                      <div><strong><span className="border-bottom border-dark">
                        Status:</span></strong> {data.status}
                      </div>
                      <div><strong><span className="border-bottom border-dark">
                        Block Number:</span></strong> {data.blockNumber}
                      </div>
                      <div><strong><span className="border-bottom border-dark">
                        Contract Address:</span></strong> {data.contractAddress}
                      </div>
                      <div><strong><span className="border-bottom border-dark">
                        From:</span></strong> {data.from}
                      </div>
                      <div><strong><span className="border-bottom border-dark">
                        Logs:</span></strong> { JSON.stringify(data.logs, null, 4)}
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

export default SKALEChainTransactions
