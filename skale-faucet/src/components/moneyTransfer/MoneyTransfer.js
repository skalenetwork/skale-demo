import React, { Component } from 'react'

import './styles.scss';

class MoneyTransfer extends Component {
  constructor(props) {
    super(props)
    this.handleChangeAccount = this.handleChangeAccount.bind(this);
    this.handleChangeEndpoint = this.handleChangeEndpoint.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChangeAccount(event) {
    this.props.onUpdateAccount(event.target.value);
  }

  handleChangeEndpoint(event) {
    this.props.onUpdateEndpoint(event.target.value);
  }

  handleSubmit(event) {
    this.props.onSendETH(event);
  }

  render() {
    const {account, endpoint} = this.props;
    return(
      <div className="money-transfer h-100 pb-5">
        <div className="center margin50">
        <h1>SKALE Innovator Program Faucet</h1>
        <br/>
        <p>To receive tokens for testing out your SKALE Chain</p>
        <p> please enter your SKALE Chain Endpoint, and wallet to receive ETH!</p>
        <br/>
      </div>
        <div className="transfer">
          <div className="d-flex justify-content-center">
            <div className="mb-5">
              <input
                id="greeting"
                type="text"
                autoComplete="off"
                className="text-center"
                placeholder="Enter your SKALE Endpoint..."
                value={endpoint}
                onChange={(event) => this.handleChangeEndpoint(event)}
              />
            </div>
          </div> 
          <div className="d-flex justify-content-center">
            <div className="mb-5">
              <input
                id="greeting"
                type="text"
                autoComplete="off"
                className="text-center"
                placeholder="Enter your Account..."
                value={account}
                onChange={(event) => this.handleChangeAccount(event)}
              />
            </div>
          </div> 
        </div> 
        <div className="d-flex justify-content-center">
          <button onClick={(event) => this.handleSubmit(event)} 
            className="btn btn-outline-primary yellow-button">Get ETH</button>
        </div>
      </div>
    )
  }
}

export default MoneyTransfer
