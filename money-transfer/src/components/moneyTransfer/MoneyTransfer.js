import React, { Component } from 'react'
import MainnetTransactionsContainer from './transactions/MainnetTransactions'
import SKALEChainTransactionsContainer from './transactions/SKALEChainTransactions'

import './styles.scss';

class MoneyTransfer extends Component {
  constructor(props) {
    super(props)
    this.onMakeDeposit = this.props.onMakeDeposit;
    this.onMakeWithdrawal = this.props.onMakeWithdrawal;
    this.handleChangeExit = this.handleChangeExit.bind(this);
    this.handleChangeDeposit = this.handleChangeDeposit.bind(this);
    this.state = {
      exitValue: '',
      depositValue: ''
    };
  }

  componentDidMount(){
    this.props.onRefreshBalances();
  }

  handleChangeExit(event) {
    this.setState({exitValue: event.target.value});
  }

  handleChangeDeposit(event) {
    this.setState({depositValue: event.target.value});
  }

  render() {
    const {depositBoxBalance, mainnetBalance, schainBalance, transactionDataSchain, transactionDataMainnet} = this.props;
    const {exitValue, depositValue} = this.state;
    return(
      <div className="money-transfer h-100 pb-5">
        <div className="transfer">
          <div className="row justify-content-around">
            <div className="col-sm-5">
              <h4 className="text-center pt-2">Mainnet</h4>
              <form id="transferFunds" className="payment-form px-4 py-2 rounded">
                <small className="form-text text-truncate"><span className="yellow">ETH:</span> {mainnetBalance}</small>
                <small className="form-text text-truncate"><span className="yellow">Endpoint:</span> {process.env.PRIVATE_MAINNET}</small>
                <small id="addressHelp" className="form-text"><span className="yellow">Account:</span> {process.env.ACCOUNT}</small>

                <div className="col-12 pt-4">
                  <div className="form-group pt-4">
                    <input type="number" className="form-control center-both" id="deposit-amount" placeholder="Deposit Amount?" value={this.state.depositValue} onChange={this.handleChangeDeposit}/>
                  </div>
                </div>
                <div className="text-center">
                  <button onClick={(event) => this.props.onDeposit(event, depositValue)} className="btn btn-primary my-3 mx-auto">Deposit</button>
                </div>
              </form> 
              <div className="pt-5 pb-5">
                <MainnetTransactionsContainer transactionData={transactionDataMainnet}/>
              </div>
            </div>
            <div className="col-md-2">
                <h4 className="text-center pt-2">Deposit Box</h4>
                <div className="payment-form px-4 py-2 rounded">
                  <small id="addressHelp" className="form-text text-white center">
                    <span className="yellow">Total:</span> {depositBoxBalance}
                  </small>
                </div>
            </div>
            <div className="col-sm-5">
              <h4 className="text-center pt-2">SKALE Chain</h4>
              <form id="transferFunds" className="payment-form px-4 py-2 rounded">
                <small className="form-text"><span className="yellow">ETH:</span> {schainBalance}</small>
                <small className="form-text text-truncate"><span className="yellow">Endpoint:</span> {process.env.SKALE_CHAIN}</small>
                <small id="addressHelp" className="form-text text-truncate"><span className="yellow">Account:</span> {process.env.ACCOUNT}</small>

                <div className="col-12 pt-4">
                  <div className="form-group pt-4">
                    <input type="number" className="form-control center-both" id="amount"placeholder="Exit Amount?"  value={this.state.exitValue} onChange={this.handleChangeExit}/>
                  </div>
                </div>

                <div className="text-center">
                  <button onClick={(event) => this.props.onExit(event, exitValue)} className="btn btn-primary my-3 mx-auto">Exit</button>
                </div>
              </form>
              <div className="pt-5 pb-5">
                <SKALEChainTransactionsContainer transactionData={transactionDataSchain}/>
              </div>
            </div>  
          </div>
        </div>
      </div>
    )
  }
}

export default MoneyTransfer
