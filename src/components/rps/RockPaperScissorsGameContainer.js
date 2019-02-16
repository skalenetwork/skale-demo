import { connect } from 'react-redux'
import RockPaperScissorsGame from './RockPaperScissorsGame'
import { registerPlayer, soloPlay, computerPlay, gameReset } from './RockPaperScissorsGameActions'

const mapStateToProps = (state, ownProps) => {
  return {
    player: state.user.data.name,
    winner: state.game.winner,
    showWinner: state.game.displayWinner,
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    onRegisterUser: () => {
      dispatch(registerPlayer())
    },
    onPlay: (value, computerChoice) => {
      dispatch(soloPlay(value, computerChoice))
    },
    onPlayTwo: (value, computerChoice, callback) => {
      dispatch(computerPlay(value, computerChoice, callback))
    },
    resetGame: () => {
      dispatch(gameReset())
    }

  }
}

const RockPaperScissorsGameContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(RockPaperScissorsGame)

export default RockPaperScissorsGameContainer
