import React, { Component } from 'react'
import { registerPlayer } from '../../components/rps/RockPaperScissorsGameActions'

// Images
import nodes from './../../assets/reversed.gif'
import skale_logo from './../../assets/Skale_Logo_White.png'

class Demo extends Component {

  render() {
    return(
      <main className="container-fluid pt-5">
        <div className="pt-5">
          <div className="nodes" onClick={registerPlayer} style={{backgroundImage: 'url(' + nodes + ')'}}>
            <div className="center-both">
              <img className="medium" alt="SKALE Demo" src={skale_logo}/>
              <p className="text-shadowbox"><strong>Hello User!</strong>
                <br/>Let's play a game.
              </p>
            </div>
          </div>
        </div>
      </main>
    )
  }
}

export default Demo
