import React, { Component } from 'react'
import SignUpFormContainer from '../../ui/signupform/SignUpFormContainer'

class SignUp extends Component {
  render() {
    return(
      <main className="container-fluid pt-5">
        <div className="center pt-5">
          <h1 className="underline-yellow mb-4">Sign Up</h1>
            <p>We've got your wallet information, simply input your name and your account is made!</p>
          <SignUpFormContainer />
        </div>
      </main>
    )
  }
}

export default SignUp
