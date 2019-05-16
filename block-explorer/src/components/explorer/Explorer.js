import React, { Component } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

import './styles.scss';

class Explorer extends Component {
  constructor(props) {
    super(props);
    this.handleShowBlock = this.handleShowBlock.bind(this); 
    this.handleShowTransaction = this.handleShowTransaction.bind(this); 
    this.state = {
      endpoint: "",
      showBlock: false,
      showTransaction: false,
      block: {},
      transaction: {receipt: ""},
    }

  }

  handleChange(event) {
    event.preventDefault();
    let endpoint = event.target.value;
    this.setState({ endpoint: endpoint });
    let endpointVerify = /((https?)|(wss?):\/\/.*):(\d*)\/?/;
    if(endpointVerify.test(endpoint)){
      this.props.onGetBlocks(endpoint);
    }
  }

  handleShowBlock(block) {
    this.setState({ showBlock: true, block: block });
  }

  handleShowTransaction(transaction) {
    this.setState({ showTransaction: true, transaction: transaction });
  }

  handleCloseBlock(event) {
    event.preventDefault();
    this.setState({ showBlock: false });
  }

  handleCloseTransaction(event) {
    event.preventDefault();
    this.setState({ showTransaction: false });
  }


  render() {
    const {endpoint, block, transaction, showBlock, showTransaction} = this.state;
    const {blockData, transactionData, filter} = this.props; 
    return(
      <div className="explorer h-100 pb-5">
        <div className="d-flex justify-content-center">
          <div className="mb-5">
            <input
              id="greeting"
              type="text"
              autoComplete="off"
              className="text-center"
              placeholder="Enter your SKALE Endpoint..."
              aria-describedby="endpointHelp"
              value={endpoint}
              onChange={(event) => this.handleChange(event)}
            />
          </div>
        </div> 
        <div className="row pb-5">
          <div className="col-md-6">
            <div className="accordion" id="dataBlock">
              <div className="list-group border-grey">
              <button type="button" className="list-group-item list-group-item-action background-grey border-between text-white">
                <div className="row">
                  <div className="col-sm-3">
                    <h3 className="mb-3">Blocks</h3>
                  </div>
                  <div className="col-sm-9">
                    <div className="search-container ml-auto">
                      <div className="search-icon-btn">
                        <FontAwesomeIcon icon="search" />
                      </div>
                      <div className="search-input">
                        <input 
                          onChange={(event) => this.props.onGetBlock(event, endpoint, event.target.value, this.handleShowBlock)}
                          type="search" className="search-bar" 
                          placeholder="Search block number..."/>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="row">
                  <div className="col-3 ">
                    Number
                  </div>
                  <div className="col-4">
                    Date
                  </div>
                  <div className="col-5 text-right">
                    Transactions
                  </div>
                </div>
                </button>
              <div className={"multi-collapse searchBlock " + (showBlock ? "" : "disable")} id="multiCollapseBlock">
              <button type="button" onClick={(event) => this.handleCloseBlock(event)} className="close text-white" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
              <div><strong><span className="border-bottom border-dark">
                Hash:</span></strong> {block.hash}
              </div>
              <div><strong><span className="border-bottom border-dark">
                Date:</span></strong> {new Date(block.timestamp * 1000).toLocaleString()}
              </div>
              <div><strong><span className="border-bottom border-dark">
                Transaction Count:</span></strong> {block.cntTransactions}
              </div>
              <div><strong><span className="border-bottom border-dark">
                Size:</span></strong> {block.size}
              </div>
              <div><strong><span className="border-bottom border-dark">
                Gas Limit:</span></strong> {block.gasLimit}
              </div>
              <div><strong><span className="border-bottom border-dark">
                Gas Used:</span></strong> {block.gasUsed}
              </div>
              <div><strong><span className="border-bottom border-dark">
                Trsndsctions:</span></strong>
              </div>
              <div className="accordion" id="dataTxBlock">
                {block.transactionData ? block.transactionData.map((dataTx, index) => {
                  return (
                    <div key={index} data-id={index}>
                      <button className={"list-group-item list-group-item-action blue-button border-between text-truncate " + ((index % 2) ? "background-grey" : "")} type="button" data-toggle="collapse" data-target={"#collapseDataTx_" + index} aria-expanded="true" aria-controls={"collapseDataTx_" + index}>
                          {dataTx.hash}
                      </button>

                      <div id={"collapseDataTx_" + index} className={"collapse transactionData pl-4 pt-1 pb-4 border-blue-top " + ((index % 2) ? "background-grey" : "")} aria-labelledby={"headingTx_" + index} data-parent="#dataTxBlock">
                          <div><strong><span className="border-bottom border-dark">
                            Hash:</span></strong> {dataTx.hash}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Status:</span></strong> {dataTx.status}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Block Number:</span></strong> {dataTx.blockNumber}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Contract Address:</span></strong> {dataTx.receipt.contractAddress}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            From:</span></strong> {dataTx.from}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            To:</span></strong> {dataTx.to}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Logs:</span></strong> { JSON.stringify(dataTx.receipt.logs, null, 4)}
                          </div>
                      </div>
                    </div>
                    )
                  }) : ""}
                </div>
              </div>
              <div id="multiCollapseBlocks" className={"multi-collapse data-list " + (showBlock ? "disable" : "")}>
                {blockData.map((data, index) => {
                  return (
                    <div style={{display: (filter ? (data.cntTransactions === 0 ? 'none': 'block') : 'block')}} key={index} data-id={index}>
                      <button className={"list-group-item list-group-item-action blue-button border-between " + ((index % 2) ? "background-grey" : "")} type="button" data-toggle="collapse" data-target={"#collapseData_" + index} aria-expanded="true" aria-controls={"collapseData_" + index}>
                        <div className="row">
                          <div className="col-3">
                            {data.number}
                          </div>
                          <div className="col-6">
                            {new Date(data.timestamp * 1000).toLocaleString()}
                          </div>
                          <div className="col-3 text-right">
                            {data.cntTransactions}
                          </div>
                        </div>
                      </button>

                      <div id={"collapseData_" + index} className={"collapse blockData px-4 pt-1 pb-4 border-blue-top " + ((index % 2) ? "background-grey" : "")}  aria-labelledby={"headingData_" + index} data-parent="#dataBlock">
                          <div><strong><span className="border-bottom border-dark">
                            Hash:</span></strong> {data.hash}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Date:</span></strong> {new Date(data.timestamp * 1000).toLocaleString()}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Transaction Count:</span></strong> {data.cntTransactions}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Size:</span></strong> {data.size}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Gas Limit:</span></strong> {data.gasLimit}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Gas Used:</span></strong> {data.gasUsed}
                          </div>
                          <div><strong><span className="border-bottom border-dark">
                            Trsndsctions:</span></strong>
                          </div>
                          <div className="accordion" id="dataTxBlock">
                            {data.transactionData.map((dataTx, index) => {
                              return (
                                <div key={index} data-id={index}>
                                  <button className={"list-group-item list-group-item-action blue-button border-between text-truncate " + ((index % 2) ? "background-grey" : "")} type="button" data-toggle="collapse" data-target={"#collapseDataTx_" + index} aria-expanded="true" aria-controls={"collapseDataTx_" + index}>
                                      {dataTx.hash}
                                  </button>

                                  <div id={"collapseDataTx_" + index} className={"collapse transactionData pl-4 pt-1 pb-4 border-blue-top " + ((index % 2) ? "background-grey" : "")} aria-labelledby={"headingTx_" + index} data-parent="#dataTxBlock">
                                      <div><strong><span className="border-bottom border-dark">
                                        Hash:</span></strong> {dataTx.hash}
                                      </div>
                                      <div><strong><span className="border-bottom border-dark">
                                        Status:</span></strong> {dataTx.status}
                                      </div>
                                      <div><strong><span className="border-bottom border-dark">
                                        Block Number:</span></strong> {dataTx.blockNumber}
                                      </div>
                                      <div><strong><span className="border-bottom border-dark">
                                        Contract Address:</span></strong> {dataTx.receipt.contractAddress}
                                      </div>
                                      <div><strong><span className="border-bottom border-dark">
                                        From:</span></strong> {dataTx.from}
                                      </div>
                                      <div><strong><span className="border-bottom border-dark">
                                        To:</span></strong> {dataTx.to}
                                      </div>
                                      <div><strong><span className="border-bottom border-dark">
                                        Logs:</span></strong> { JSON.stringify(dataTx.receipt.logs, null, 4)}
                                      </div>
                                  </div>
                                </div>
                                )
                              })}
                            </div>
                      </div>
                    </div>
                    )
                  })}
                </div>
              </div>
            </div> 
          </div>
          <div className="col-md-6">
            <div className="accordion" id="txBlock">
              <div className="list-group border-grey">
              <button type="button" className="list-group-item list-group-item-action background-grey border-between text-white">
                <div className="row">
                  <div className="col-sm-3">
                    <h3 className="mb-3">Transactions</h3>
                  </div>
                  <div className="col-sm-9">
                    <div className="search-container ml-auto">
                      <div className="search-icon-btn">
                        <FontAwesomeIcon icon="search" />
                      </div>
                      <div className="search-input">
                        <input 
                          onChange={(event) => this.props.onGetTransaction(event, endpoint, event.target.value, this.handleShowTransaction)}
                          type="search" className="search-bar" 
                          placeholder="Search transaction hash..."/>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="row">
                  <div className="col-12">
                    Hash
                  </div>
                </div>
                </button>
                <div className={"multi-collapse searchTransaction " + (showTransaction ? "" : "disable")}>
                  <button type="button" onClick={(event) => this.handleCloseTransaction(event)} className="close text-white" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
                  <div><strong><span className="border-bottom border-dark">
                    Hash:</span></strong> {transaction.hash}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    Status:</span></strong> {transaction.status}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    Block Number:</span></strong> {transaction.blockNumber}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    Contract Address:</span></strong> {transaction.receipt ? transaction.receipt.contractAddress : ""}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    Value:</span></strong> {transaction.value}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    From:</span></strong> {transaction.from}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    To:</span></strong> {transaction.to}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    Gas:</span></strong> {transaction.gas}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    Gas Price:</span></strong> {transaction.gasPrice}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    Input:</span></strong> {transaction.input}
                  </div>
                  <div><strong><span className="border-bottom border-dark">
                    Logs:</span></strong> { JSON.stringify(transaction.receipt.logs, null, 4)}
                  </div>
                </div>
                <div className={"multi-collapse data-list " + (showTransaction ? "disable" : "")}>
                  {transactionData.map((data, index) => {
                    return (
                      <div key={index} data-id={index}>
                        <button className={"list-group-item list-group-item-action blue-button border-between " + ((index % 2) ? "background-grey" : "")} type="button" data-toggle="collapse" data-target={"#collapseTx_" + index} aria-expanded="true" aria-controls={"truncate-text collapseTx_" + index}>
                            {data.hash }
                        </button>

                        <div id={"collapseTx_" + index} className={"collapse transactionData pl-4 pt-1 pb-4 border-blue-top " + ((index % 2) ? "background-grey" : "")} aria-labelledby={"headingTx_" + index} data-parent="#txBlock">
                            <div><strong><span className="border-bottom border-dark">
                              Hash:</span></strong> {data.hash}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              Status:</span></strong> {data.status}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              Block Number:</span></strong> {data.blockNumber}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              Contract Address:</span></strong> {data.receipt.contractAddress}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              Value:</span></strong> {data.value}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              From:</span></strong> {data.from}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              To:</span></strong> {data.to}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              Gas:</span></strong> {data.gas}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              Gas Price:</span></strong> {data.gasPrice}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              Input:</span></strong> {data.input}
                            </div>
                            <div><strong><span className="border-bottom border-dark">
                              Logs:</span></strong> { JSON.stringify(data.receipt.logs, null, 4)}
                            </div>
                        </div>
                      </div>
                      )
                    })}
                  </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default Explorer
