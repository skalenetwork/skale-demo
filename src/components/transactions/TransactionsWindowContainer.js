import { connect } from 'react-redux'
import TransactionsWindow from './TransactionsWindow'
import { getBlocks } from './TransactionsWindowActions'

const mapStateToProps = (state, ownProps) => {
  return {
    transactions: state.transactions.data,
    skaleTransactions: state.transactions.dataSkale,
    isSkale: state.web3.skale,
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    onGetBlocks: () => {

      dispatch(getBlocks())
    }
  }
}

const TransactionsWindowContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(TransactionsWindow)

export default TransactionsWindowContainer
