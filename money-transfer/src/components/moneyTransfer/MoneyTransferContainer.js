import { connect } from 'react-redux'
import MoneyTransfer from './MoneyTransfer'
import { refreshBalances, deposit, exit } from './MoneyTransferActions'

const mapStateToProps = (state, ownProps) => {
  return {
    transactionDataMainnet: state.transactions.transactionDataMainnet,
    transactionDataSchain: state.transactions.transactionDataSchain,
    tokenManagerBalance: state.web3.tokenManagerBalance,
    depositBoxBalance: state.web3.depositBoxBalance,
    mainnetBalance: state.web3.mainnetBalance,
    schainBalance: state.web3.schainBalance,
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    onRefreshBalances: () => {
      refreshBalances();
    },
    onDeposit: (event, amount) => {
      event.preventDefault();
      deposit(amount);
    },
    onExit: (event, amount) => {
      event.preventDefault();
      exit(amount);
    }
  }
}

const MoneyTransferContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(MoneyTransfer)

export default MoneyTransferContainer
