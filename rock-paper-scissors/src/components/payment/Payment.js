import React, { Component } from 'react'
import './styles.scss';

class Payment extends Component {
  constructor(props) {
    super(props)
    this.onChange = this.onChange;
    this.onMakeDeposit = this.props.onMakeDeposit;
    this.onMakeWithdrawal = this.props.onMakeWithdrawal;
    this.state = {
      transactions: [],
      address: null,
      balance: 0,
      addressKTM: null,
      balanceKTM: 0,
      depositValue: 0,
    }
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevState.transactions !== prevProps.transactions) {
      this.setState({ transactions: this.props.transactions });
    }    
    if (prevState.address !== prevProps.address) {
      this.setState({ address: prevProps.address });
    }
    if (prevState.balance !== prevProps.balance) {
      this.setState({ balance: prevProps.balance });
    }
    if (prevState.addressKTM !== prevProps.addressKTM) {
      this.setState({ addressKTM: prevProps.addressKTM });
    }
    if (prevState.balanceKTM !== prevProps.balanceKTM) {
      this.setState({ balanceKTM: prevProps.balanceKTM });
    }
  } 

  onChange(event) {
    event.preventDefault();
    this.setState({ depositValue: event.target.value });
  }

  render() {
    const {address, balance, addressKTM, balanceKTM, depositValue} = this.state;
    return(
      <div className="row justify-content-around">
        <div className="col-sm-6">
          <h4 className="text-center pt-2">Deposit Connector</h4>
          <form id="transferFunds" className="payment-form px-4 py-2 rounded">
            <fieldset disabled>
              <div className="form-group pt-4 text-truncate">
                {address}
                <small id="addressHelp" className="form-text text-muted yellow">Account to transfer ETH from.</small>
              </div>
            </fieldset>
            <div className="form-group pt-4">
              <input type="number" onChange={(event) => this.onChange(event)}
              className="form-control" id="amount" aria-describedby="amountHelp"  placeholder="Deposit Amount?"/>
              <small id="amountHelp" className="form-text text-muted yellow text-truncate">Your ETH Balance is: {balance}</small>
            </div>
            <div className="text-center">
              <button onClick={(event) => this.onMakeDeposit(event, depositValue)} className="btn btn-primary my-3 mx-auto">Deposit</button>
            </div>
          </form> 
        </div>
        <div className="col-sm-6">
          <h4 className="text-center pt-2">Exit Connector</h4>
          <form id="transferFunds" className="payment-form px-4 py-2 rounded">
            <fieldset disabled>
              <div className="form-group pt-4 text-truncate">
                {addressKTM}
                <small id="addressHelp" className="form-text text-muted yellow text-truncate">Account to transfer ETH to.</small>
              </div>
            </fieldset>
            <div className="form-group pt-4">
              <input type="number" className="form-control" id="amount" aria-describedby="amountHelp"  placeholder="Exit Amount?"/>
              <small id="amountHelp" className="form-text text-muted yellow">Your ETH Balance is: {balanceKTM}</small>
            </div>
            <div className="text-center">
              <button onClick={(event) => this.onMakeWithdrawal(event)} className="btn btn-primary my-3 mx-auto">Exit</button>
            </div>
          </form>
        </div>  
      </div>
    )
  }
}

export default Payment
