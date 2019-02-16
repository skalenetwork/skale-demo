import React, { Component } from 'react'
import RockPaperScissorsGameContainer from '../../../components/rps/RockPaperScissorsGameContainer'

class RockPaperScissors extends Component {
  render() {
    return(
      <main className="container-fluid">
        <div className="pt-2 pl-2">
          <RockPaperScissorsGameContainer />
        </div>
      </main>
    )
  }
}

export default RockPaperScissors
