import React, { Component } from 'react'

class ProfileForm extends Component {
  constructor(props) {
    super(props)

    this.state = {
      name: this.props.name,
      address: this.props.address
    }
  }

  onInputChange(event) {
    this.setState({ name: event.target.value })
  }

  handleSubmit(event) {
    event.preventDefault()

    if (this.state.name.length < 2)
    {
      return alert('Please fill in your name.')
    }

    this.props.onProfileFormSubmit(this.state.name)
  }

  render() {
    return(
      <form onSubmit={this.handleSubmit.bind(this)}>
        <div className="form-group">
          <div className="row justify-content-center">
            <div className="col-6 pb-3">
              <p>Account in use: {this.state.address}</p>
            </div>
          </div>

          <div className="row justify-content-center">
            <div className="col-3 pb-3">
              <input id="name" className="form-control" type="text" value={this.state.name} 
              onChange={this.onInputChange.bind(this)} placeholder="Name" />
            </div>
          </div>
          <button type="submit" className="btn btn-outline-primary yellow-button">Update</button>
        </div>
      </form>
    )
  }
}

export default ProfileForm
