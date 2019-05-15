import store from '../../store'
import Web3 from 'web3'

export const ADD_TRANSACTIONS = 'ADD_TRANSACTIONS'
export function addTransactions(transaction) {
  return {
    type: ADD_TRANSACTIONS,
    payload: transaction
  }
}

export const ADD_BLOCKS = 'ADD_BLOCKS'
export function addBlocks(blocks) {
  return {
    type: ADD_BLOCKS,
    payload: blocks
  }
}

async function getBlockData(dispatch, web3) {
  const nLatestBlockNumber = await web3.eth.getBlockNumber();
  const indexEnd = store.getState().transactions.lastBlock >= 0 ? 
    store.getState().transactions.lastBlock : (nLatestBlockNumber - 10);
  for( var i = nLatestBlockNumber; i > indexEnd; -- i ) {
    const block = await web3.eth.getBlock( i );
    block.cntTransactions = 0;
    block.transactionData = [];
    try { 
      block.cntTransactions = block.transactions.length;
      if(block.cntTransactions > 0) {
        block.transactions.forEach(async function(hash) {
          const transactionData = await web3.eth.getTransaction(hash);
          transactionData.receipt = await web3.eth.getTransactionReceipt(hash);
          console.log(transactionData)
          block.transactionData.push(transactionData);

          const transactions = [];
          transactions.push(transactionData);
          dispatch(addTransactions({transactionData: transactions, reverse: true}));
        });            
      }
    } catch(e) { 
      console.log(e);
    }
    const blocks = [];
    blocks.push(block);
    dispatch(addBlocks({blocks: blocks, lastBlock: nLatestBlockNumber, reverse: true}));
  }
  setTimeout(function() {
    getBlockData(dispatch, web3);
  }, 3000);
}

async function getBlockDataPast(dispatch, web3) {
  let {lastBlock} = store.getState().transactions
  const nLatestBlockNumber = await web3.eth.getBlockNumber();
  const indexEnd = lastBlock >= 0 ? lastBlock : (nLatestBlockNumber - 300);
  //get recent block data
  dispatch(addBlocks({blocks: [], lastBlock: nLatestBlockNumber, reverse: true}));
  setTimeout(function() {
    getBlockData(dispatch, web3);
  }, 2000);getBlockData(dispatch, web3);
  //get past block data
  for( var i = nLatestBlockNumber; i > indexEnd; -- i ) {
    const block = await web3.eth.getBlock( i );
    block.cntTransactions = 0;
    block.transactionData = [];
    try { 
      block.cntTransactions = block.transactions.length;
      if(block.cntTransactions > 0) {
        block.transactions.forEach(async function(hash) {
          const transactionData = await web3.eth.getTransaction(hash);
          transactionData.receipt = await web3.eth.getTransactionReceipt(hash);
          block.transactionData.push(transactionData);

          const transactions = [];
          transactions.push(transactionData);
          dispatch(addTransactions({transactionData: transactions, reverse: false}))
        });            
      }
    } catch(e) { 
      console.log(e);
    }
    const blocks = [];
    blocks.push(block);
    dispatch(addBlocks({blocks: blocks, lastBlock: "", reverse: false}));
  }
  
}

export function getBlocks(endpoint) {
  let web3 = new Web3(endpoint);

  return function(dispatch) {
    getBlockDataPast(dispatch, web3);
  }
  
}

export function getBlock(endpoint, block, callback) {
  let web3 = new Web3(endpoint);
  web3.eth.getBlock(block).then(function(blockData) {
    blockData.cntTransactions = 0;
    blockData.transactionData = [];
    try { 
    blockData.cntTransactions = blockData.transactions.length;
    if(blockData.cntTransactions > 0) {
      blockData.transactions.forEach(async function(hash) {
        let transactionData = await web3.eth.getTransaction(hash);
        transactionData.receipt = await web3.eth.getTransactionReceipt(hash);
        blockData.transactionData.push(transactionData);
      });            
    }
    callback(blockData);
  } catch(e) { 
    console.log(e);
  }
  });
}

export function getTransaction(endpoint, hash, callback) {
  let web3 = new Web3(endpoint);
  web3.eth.getTransaction(hash).then(function(transaction) {
    web3.eth.getTransactionReceipt(hash).then(function(receipt) {
      transaction.receipt = receipt;
      callback(transaction);
    })
  })
}


