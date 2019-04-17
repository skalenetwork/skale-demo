import RockPaperScissorsContract from '../../../build/contracts/RockPaperScissors.json'
import { browserHistory } from 'react-router'
import store from '../../store'
import contract from 'truffle-contract'

let count = 1000;
export const DISPLAY_WINNER = 'DISPLAY_WINNER'
function setWinner(winner) {
  return {
    type: DISPLAY_WINNER,
    payload: winner
  }
}

export const SHOW_WINNER_SCREEN = 'SHOW_WINNER_SCREEN'
function showWinnerScreen(show_winner_screen) {
  return {
    type: SHOW_WINNER_SCREEN,
    payload: show_winner_screen
  }
}



export function registerPlayer() {
  return browserHistory.push('/demo/rps')
}

export function makeChoice(value) {
  let web3 = store.getState().web3.web3Instance;

  // Double-check web3's status.
  if (typeof web3 !== 'undefined') {

    return function(dispatch) {
      // Using truffle-contract we create the RockPaperScissors object.
      const rps = contract(RockPaperScissorsContract);
      rps.setProvider(web3.currentProvider);

      // Declaring this for later so we can chain functions on RockPaperScissors.
      var rpsInstance;

      // Get current ethereum wallet.
      web3.eth.getCoinbase((error, coinbase) => {
        // Log errors, if any.
        if (error) {
          console.error(error);
        }

        rps.deployed().then(function(instance) {
          rpsInstance = instance;
          dispatch(showWinnerScreen("show_winner"));
          // Attempt to make choice.
          rpsInstance.play(value, {from: coinbase})
          .then(function(result) {
            // If no error, update ui.
            return dispatch(getWinner());
          })
          .catch(function(result) {
            // If error...
            dispatch(showWinnerScreen(""));
          })
        })
      })
    }
  } else {
    console.error('Web3 is not initialized.');
  }
}

export function computerPlay(computerChoice, computerChoice2, callback) {
  let web3 = store.getState().web3.web3Instance;
  let account = process.env.ACCOUNT;
  let privateKey = process.env.PRIVATE_KEY;

  // Double-check web3's status.
  if (typeof web3 !== 'undefined') {

    return function(dispatch) {
      // Using truffle-contract we create the RockPaperScissors object.
      const rps = contract(RockPaperScissorsContract);
      rps.setProvider(web3.currentProvider);

      // Declaring this for later so we can chain functions on RockPaperScissors.
      var rpsInstance;

      dispatch(showWinnerScreen("show_winner"));

      // Get current ethereum wallet.
      web3.eth.getCoinbase((error, coinbase) => {
        // Log errors, if any.
        if (error) {
          console.error(error);
        }

        rps.deployed().then( async function(instance) {
          rpsInstance = instance;

          var contract = new web3.eth.Contract(rpsInstance.abi, rpsInstance.address);

          var computerWar = contract.methods.computerWar(computerChoice, computerChoice2);
          var encodedABI = computerWar.encodeABI();

          var nonce = await web3.eth.getTransactionCount(coinbase,'pending')

          console.log(nonce);
          var tx = {
            nonce: nonce,
            from: account,
            to: rpsInstance.address,
            gasPrice: 0,
            gasLimit: 3141562,
            data: encodedABI
          };

          console.log(tx);
          web3.eth.accounts.signTransaction(tx, privateKey).then(signed => {
            var tran = web3.eth.sendSignedTransaction(signed.rawTransaction);
            console.log(tran);
            tran.on('receipt', receipt => {

              dispatch(setWinner({message: "Computer War", opponent: computerChoice2}));
              
              if(count !== 0){
                console.log(count);
                count--;
                return callback();
              } else {
                count = 10;
                console.log(count);
              }

            });



            tran.on('error', console.error);
          });
        })
      })
    }
  } else {
    console.error('Web3 is not initialized.');
  }
}

export function soloPlay(value, computerChoice) {
  let web3 = store.getState().web3.web3Instance;

  // Double-check web3's status.
  if (typeof web3 !== 'undefined') {

    return function(dispatch) {
      // Using truffle-contract we create the RockPaperScissors object.
      const rps = contract(RockPaperScissorsContract);
      rps.setProvider(web3.currentProvider);

      rps.currentProvider.sendAsync = function() {
        return rps.currentProvider.send.apply(
          rps.currentProvider, arguments
        );
      };

      // Declaring this for later so we can chain functions on RockPaperScissors.
      var rpsInstance;
      // Get current ethereum wallet.
      web3.eth.getCoinbase((error, coinbase) => {
        // Log errors, if any.
        if (error) {
          console.error(error);
        }

        rps.deployed().then(function(instance) {
          rpsInstance = instance;
          // Attempt to make choice.
          rpsInstance.playSolo(value, computerChoice, {from: coinbase})
          .then(function(result) {
            // If no error, update ui.
            dispatch(showWinnerScreen("show_winner"));


            if(result.logs[1].event === "Winner"){
              var res = result.logs[1];

              if("Tie" === res.args.winner){
                dispatch(setWinner({message: "It's a Tie", opponent: res.args.player2Choice}));
              } else if("You Win" === res.args.winner){
                dispatch(setWinner({message: "You Win", opponent: res.args.player2Choice}));
              } else if("You Lose" === res.args.winner){
                dispatch(setWinner({message: "You Lose", opponent: res.args.player2Choice}));
              }
            }
          })
          .catch(function(result) {
            // If error...
            dispatch(showWinnerScreen(""));
          })
        })
      })
    }
  } else {
    console.error('Web3 is not initialized.');
  }
}

export function getWinner(value) {
  let web3 = store.getState().web3.web3Instance

  // Double-check web3's status.
  if (typeof web3 !== 'undefined') {

    return function(dispatch) {
      // Using truffle-contract we create the RockPaperScissors object.
      const rps = contract(RockPaperScissorsContract)
      rps.setProvider(web3.currentProvider)

      rps.currentProvider.sendAsync = function() {
        return rps.currentProvider.send.apply(
          rps.currentProvider, arguments
        );
      };

      // Declaring this for later so we can chain functions on RockPaperScissors.
      var rpsInstance

      // Get current ethereum wallet.
      web3.eth.getCoinbase((error, coinbase) => {
        // Log errors, if any.
        if (error) {
          console.error(error);
        }

        rps.deployed().then(function(instance) {
          rpsInstance = instance

          rpsInstance.allEvents(
            {fromBlock: 0, toBlock: 'latest'},
            function(err, res){
              if (err) {
                console.log(err);
              }
              console.log(res);
              if(res.event === "Winner"){
                var iWon = coinbase === res.args.winningPlayer ? true : false;

                if("Tie" === res.args.winner){
                  dispatch(setWinner({message: "Game Over", opponent: res.args.player1Choice}));
                } else if("Player 1 Wins" === res.args.winner){
                  dispatch(setWinner({message: iWon ? "Game Over" : "You Lost",
                    opponent: iWon ? res.args.player2Choice : res.args.player1Choice}));
                } else if("Player 2 Wins" === res.args.winner){
                  dispatch(setWinner({message: iWon ? "Game Over" : "You Lost",
                    opponent: iWon ? res.args.player1Choice : res.args.player2Choice}));
                }
              }
            });
          }).catch(function(err) {
              console.log(err.message);
        });
      })
    }
  } else {
    console.error('Web3 is not initialized.');
  }
}

export function gameReset(value) {
  return function(dispatch) {
    dispatch(showWinnerScreen(""));
    dispatch(setWinner({message: "", opponent: ""}));
  }
}

