import React, { Component } from 'react'

import './styles.scss';

class ValidatorTransfer extends Component {
  constructor(props) {
    super(props)
    this.handleChangeAccount = this.handleChangeAccount.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  componentDidMount(){
    setTimeout(function() {
      this.props.onRefreshBalance();
    }.bind(this), 2000);
  }

  handleChangeAccount(event) {
    this.props.onUpdateAccount(event.target.value);
  }

  handleSubmit(event) {
    if(this.props.account !== "" && this.props.account.length > 39){
      this.props.onSendETH(event);
    }
  }

  render() {
    const {account, balance, balanceSKL} = this.props;
    return(
      <div className="money-transfer h-100 pb-5">
        <div className="center margin50">
        <h1>SKALE Alpine Validator Team Faucet</h1>
        <br/>
        <p>To receive tokens for testing out your SKALE Validator Node</p>
        <p>please enter your SKALE Node Wallet to receive test ETH and test SKALE tokens!</p>
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
                placeholder="Enter your Account..."
                value={account}
                onChange={(event) => this.handleChangeAccount(event)}
              />
              <small className="form-text text-center text-truncate"><span className="yellow">ETH Balance:</span> {balance}</small>
              <small className="form-text text-center text-truncate"><span className="yellow">SKALE Balance:</span> {balanceSKL}</small>
            </div>
          </div> 
        </div> 
        <div className="d-flex justify-content-center">
          <button onClick={(event) => this.handleSubmit(event)} 
            className="btn btn-outline-primary yellow-button">Get Tokens</button>
        </div>
      </div>
    )
  }
}

export default ValidatorTransfer
