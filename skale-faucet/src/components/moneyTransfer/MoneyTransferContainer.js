import { connect } from 'react-redux'
import MoneyTransfer from './MoneyTransfer'
import {refreshBalance, updateAccount, updateEndpoint, updateSkaleId, sendETH} from './MoneyTransferActions'

const mapStateToProps = (state, ownProps) => {
  return {
    account: state.web3.account,
    endpoint: state.web3.endpoint,
    balance: state.web3.balance,
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    onRefreshBalance: () => {
      refreshBalance();
    },
    onUpdateAccount: (accountData) => {
      event.preventDefault();
      dispatch(updateAccount(accountData));
    },
    onUpdateEndpoint: (accountData) => {
      event.preventDefault();
      dispatch(updateEndpoint(accountData));
    },
    onUpdateSkaleId: (accountData) => {
      event.preventDefault();
      dispatch(updateSkaleId(accountData));
    },
    onSendETH: (event) => {
      event.preventDefault();
      sendETH();
    }
  }
}

const MoneyTransferContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(MoneyTransfer)

export default MoneyTransferContainer
