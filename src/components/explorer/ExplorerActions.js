import store from '../../store'

export const DISPLAY_TRANSACTIONS = 'DISPLAY_TRANSACTIONS'
export function displayTransactions(transaction) {
  return {
    type: DISPLAY_TRANSACTIONS,
    payload: transaction
  }
}

export async function getBlockData(dispatch) {
  let lastBlock = store.getState().transactions.lastBlock;
  let web3 = store.getState().web3.web3Instance;
  if (typeof web3 !== 'undefined') {
    const nLatestBlockNumber = await web3.eth.getBlockNumber();
    let arrBlocks = [];
    for( var i = lastBlock; i <= nLatestBlockNumber; ++ i ) {
        const sklBlock = await web3.eth.getBlock( i );
        sklBlock.cntTransactions = 0;
        try { 
          sklBlock.cntTransactions = sklBlock.transactions.length;
        } catch( e ) { 
          console.log(e);
        }
        arrBlocks.push( sklBlock );
    }
    return dispatch(
      displayTransactions({
        blocks: arrBlocks, 
        lastBlock: nLatestBlockNumber+1
      }));
  } else {
    console.error('Web3 is not with us.');
  }
}

export function getBlocks() { 
  return function(dispatch) {
    return getBlockData(dispatch);
  }
}


