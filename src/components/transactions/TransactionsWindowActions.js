import store from '../../store'

export const DISPLAY_TRANSACTIONS = 'DISPLAY_TRANSACTIONS'
export function displayTransactions(transaction) {
  return {
    type: DISPLAY_TRANSACTIONS,
    payload: transaction
  }
}
export const DISPLAY_SKALE_TRANSACTIONS = 'DISPLAY_SKALE_TRANSACTIONS'
export function displaySkaleTransactions(transaction) {
  return {
    type: DISPLAY_SKALE_TRANSACTIONS,
    payload: transaction
  }
}

export async function getBlockData(dispatch) {
  let web3 = store.getState().web3.web3Instance;
  const isSkale = store.getState().web3.skale;
  let lastBlock = isSkale ? 
    store.getState().transactions.lastBlockSkale :
    store.getState().transactions.lastBlock;

  if (web3 !== null && typeof web3 !== 'undefined') {
    const nLatestBlockNumber = await web3.eth.getBlockNumber();
    if(lastBlock === 0 && nLatestBlockNumber > 100){
      lastBlock = nLatestBlockNumber - 50;
    }
    for( var i = lastBlock; i < nLatestBlockNumber; ++ i ) {
      const sklBlock = await web3.eth.getBlock( i );
      sklBlock.cntTransactions = 0;
      try { 
        sklBlock.cntTransactions = sklBlock.transactions.length;
      } catch( e ) { 
        console.log(e);
      }
      if(isSkale) {
        dispatch(
          displaySkaleTransactions({
            blocks: sklBlock, 
            lastBlock: nLatestBlockNumber
        }));
      } else {
        dispatch(
          displayTransactions({
            blocks: sklBlock, 
            lastBlock: nLatestBlockNumber
        }));
      }
    }
    if(isSkale) {
      dispatch(
        displaySkaleTransactions({
          blocks: null, 
          lastBlock: nLatestBlockNumber
      }));
    } else {
      dispatch(
        displayTransactions({
          blocks: null, 
          lastBlock: nLatestBlockNumber
      }));
    }
    
    return setTimeout(function() {
      getBlockData(dispatch);
    }, 1000);
  } else {
    console.log('Web3 is not with us yet.');
    return setTimeout(function() {
      getBlockData(dispatch);
    }, 5000);
  }
}

export function getBlocks() { 
  return function(dispatch) {
    return getBlockData(dispatch);
  }
}


