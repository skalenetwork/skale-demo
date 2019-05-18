import React, { Component } from 'react'
import ProfileFormContainer from '../../ui/profileform/ProfileFormContainer'

class Profile extends Component {
  render() {
    return(
      <main className="container-fluid pt-5">
        <div className="center pt-5">
          <h1 className="underline-yellow mb-4">Profile</h1>
          <ProfileFormContainer />
        </div>
      </main>
    )
  }
}

export default Profile
