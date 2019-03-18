import { connect } from 'react-redux'
import Payment from './Payment'
import { deposit, withdrawal } from './PaymentActions'

const mapStateToProps = (state, ownProps) => {
  return {
    transactions: state.transactions.data,
    address: state.web3.account.address,
    addressKTM: state.web3.ktmAccount.address,
    balance: state.web3.account.balance.toFixed(2),
    balanceKTM: state.web3.ktmAccount.balance.toFixed(2),
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    onMakeDeposit: (event, amount) => {
      event.preventDefault();
      dispatch(deposit(amount));
    },
    onMakeWithdrawal: (event, amount) => {
      event.preventDefault();
      dispatch(withdrawal(amount))
    }
  }
}

const PaymentContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(Payment)

export default PaymentContainer
