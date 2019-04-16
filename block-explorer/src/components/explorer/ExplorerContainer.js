import { connect } from 'react-redux'
import Explorer from './Explorer'
import { getBlocks, getBlock, getTransaction } from './ExplorerActions'

const mapStateToProps = (state, ownProps) => {
  return {
    blockData: state.transactions.blockData,
    transactionData: state.transactions.transactionData
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    onGetBlocks: (endpoint) => {
      dispatch(getBlocks(endpoint));
    },
    onGetBlock: (event, endpoint, block, callback) => {
      event.preventDefault();
      getBlock(endpoint, block, callback);
    },
    onGetTransaction: (event, endpoint, hash, callback) => {
      event.preventDefault();
      getTransaction(endpoint, hash, callback);
    }
  }
}

const ExplorerContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(Explorer)

export default ExplorerContainer
