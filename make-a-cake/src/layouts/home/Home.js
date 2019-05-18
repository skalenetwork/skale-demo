import React, { Component } from 'react'
import { browserHistory } from 'react-router'
import MakeCakeContainer from '../../components/makeCake/MakeCakeContainer'
import StatusContainer from '../../components/status/StatusContainer'

class Home extends Component {

  handleSubmit(event, choice) {
    event.preventDefault()

    return browserHistory.push('/signup')
  }

  render() {
    return(
      <main className="pt-5">
        <div className="">
          <MakeCakeContainer/>
          <StatusContainer/>
        </div>
      </main>
    )
  }
}

export default Home
