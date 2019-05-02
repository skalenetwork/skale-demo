import { connect } from 'react-redux'
import MoneyTransfer from './MoneyTransfer'
import {updateAccount, updateEndpoint, updateSkaleId, sendETH} from './MoneyTransferActions'

const mapStateToProps = (state, ownProps) => {
  return {
    account: state.web3.account,
    endpoint: state.web3.endpoint,
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
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
