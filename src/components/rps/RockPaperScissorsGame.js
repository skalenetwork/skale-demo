import React, { Component } from 'react'

import rock from '../../assets/rock.png'
import paper from '../../assets/paper.png'
import scissors from '../../assets/scissors.png'
import winner from '../../assets/winner.gif'

class RockPaperScissorsGame extends Component {
  constructor(props) {
    super(props)
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleComputerWar = this.handleComputerWar.bind(this);
    
    this.state = {
      twoPlayers: false,
      winner: this.props.winner,
      playerMove: "",
      computerChoice: "",
      showWinner: this.props.showWinner
    }
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevProps.showWinner !== prevState.showWinner) {
      this.setState({ showWinner: prevProps.showWinner });
    }
    else if (JSON.stringify(prevProps.winner) !==JSON.stringify(prevState.winner)) {
      this.setState({ winner: prevProps.winner});
    }
  } 

  handlePlayerUpdate(event) {
    event.preventDefault()
    if(!this.state.twoPlayers){
      this.handleComputerWar();
    }
    this.setState({ twoPlayers: !this.state.twoPlayers });
  }

  handleComputerWar() {
    var computerChoice = Math.floor(Math.random()*(50));
    var computerChoice2 = Math.floor(Math.random()*(50));
    this.setState({ 
      playerMove: computerChoice % 3, 
      computerChoice: computerChoice2 % 3,
      winner: {message: "Computer War", opponent: ""}
    });
    this.props.onPlayTwo(computerChoice % 3, 
      computerChoice2 % 3, 
      this.handleComputerWar);
  }

  handleSubmit(event, choice) {
    event.preventDefault()

    this.setState({ playerMove: choice });

    if(!this.state.twoPlayers) {
      // Computer play
      var computerChoice = Math.floor(Math.random()*(50));
      this.setState({ computerChoice: computerChoice % 3});
      this.props.onPlay(choice, computerChoice % 3);
    }
  }

  handlePlayAgain(event) {
    event.preventDefault()
    this.setState({ playerMove: "", computerChoice: "", showWinner: "" });
    this.props.resetGame();
  }

  render() {
    let myMove;
    if (this.state.playerMove === "rock" || this.state.playerMove === 0) {
      myMove = <img className="rps" alt="Rock" src={rock}/>;
    } else if (this.state.playerMove === "paper"  || this.state.playerMove === 1){
      myMove = <img className="rps" alt="Paper" src={paper}/>;
    } else  if (this.state.playerMove === "scissors"  || this.state.playerMove === 2){
      myMove = <img className="rps" alt="Scissors" src={scissors}/>;
    }

    let computerMove;
    if (this.state.computerChoice === 0 || this.state.winner.opponent === "rock") {
      computerMove = <img className="rps" alt="Rock" src={rock}/>;
    } else if (this.state.computerChoice === 1 || this.state.winner.opponent === "paper"){
      computerMove = <img className="rps" alt="Paper" src={paper}/>;
    } else  if (this.state.computerChoice === 2 || this.state.winner.opponent === "scissors"){
      computerMove = <img className="rps" alt="Scissors" src={scissors}/>;
    }

    return(
      <div className="center pt-5">
        <div className="row pt-5 justify-content-center">
        <div className={"rps-winner " + this.state.showWinner} style={{backgroundImage: 'url(' + winner + ')'}}>
          <div className="row">
            <div className="col-4 pt-5">
              <div className="pt-5">
                {myMove}
                <h3 className="pt-5">Your Move</h3>
              </div>
            </div>
            <div className="col-4">
              <div className="text-shadowbox center-both winner-box pt-5">
                <h1 className="gold">{this.state.winner.message} </h1>
              </div>
              <button type="submit" onClick={(event) => this.handlePlayAgain(event)} className="btn btn-outline-primary yellow-button bottom">Play Again</button>
            </div>
            <div className="col-4 pt-5">
              <div className="pt-5">
              {(this.state.twoPlayers || this.state.winner.opponent !== "") &&
                computerMove
              } 
                <div className="pt-5">
                {(this.state.twoPlayers || this.state.winner.opponent !== "") &&
                <h3>Opponent</h3>
                } 
                </div>
              </div>
            </div>
          </div>
        </div>
          <div className="col-6 pb-3">
            <button type="button" onClick={(event) => this.handlePlayerUpdate(event)} className="btn btn-lg btn-toggle" data-toggle="button" aria-pressed="false" autoComplete="off">
              <div className="handle"></div>
            </button>
          </div>
        </div>
        <div className="row justify-content-center">
          <div className="col-12 pb-3">
            <a onClick={(event) => this.handleSubmit(event, "rock")} className="rps-choice center">
              <img className="rps" alt="Rock" src={rock}/>
              <h2 className="mt-4">Rock</h2>
            </a>
            <a onClick={(event) => this.handleSubmit(event, "paper")} className="rps-choice center">
              <img className="rps" alt="Paper" src={paper}/>
              <h2 className="mt-4">Paper</h2>
            </a>
            <a onClick={(event) => this.handleSubmit(event, "scissors")} className="rps-choice center">
              <img className="rps" alt="Scissors" src={scissors}/>
              <h2 className="mt-4">Scissors</h2>
            </a>
          </div>
        </div>
      </div>

    )
  }
}

export default RockPaperScissorsGame
