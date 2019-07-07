import { connect } from 'react-redux'
import ValidatorTransfer from './ValidatorTransfer'
import {refreshBalance, updateAccount, updateSkaleId, sendETH, sendSKL} from './ValidatorTransferActions'

const mapStateToProps = (state, ownProps) => {
  return {
    account: state.web3.account,
    balance: state.web3.balance,
    balanceSKL: state.web3.balanceSKL,
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
    onUpdateSkaleId: (accountData) => {
      event.preventDefault();
      dispatch(updateSkaleId(accountData));
    },
    onSendETH: (event) => {
      event.preventDefault();
      sendETH();
      sendSKL();
    }
  }
}

const ValidatorTransferContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(ValidatorTransfer)

export default ValidatorTransferContainer
