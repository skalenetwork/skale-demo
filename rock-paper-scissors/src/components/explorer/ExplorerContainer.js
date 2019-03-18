import { connect } from 'react-redux'
import Explorer from './Explorer'
import { getBlocks } from './ExplorerActions'

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

const ExplorerContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(Explorer)

export default ExplorerContainer
