import React, { Component } from 'react'

class SignUpForm extends Component {
  constructor(props) {
    super(props)

    this.state = {
      name: '',
      error: ''
    }
  }

  onInputChange(event) {
    this.setState({ name: event.target.value });
  }

  handleSubmit(event) {
    event.preventDefault()

    if (this.state.name.length < 2)
    {
      return this.setState({ error: "Name is required!" });
    }

    this.props.onSignUpFormSubmit(this.state.name)
  }

  render() {
    return(
      <form onSubmit={this.handleSubmit.bind(this)}>
        <div className="form-group">
          <div className="row justify-content-center">
            <div className="col-3 pb-3">
              <input id="name" className="form-control" type="text" value={this.state.name} 
              onChange={this.onInputChange.bind(this)} placeholder="Name" />
              <p className="text-danger">{this.state.error}</p>
            </div>
          </div>
          <button type="submit" className="btn btn-outline-primary yellow-button">Sign Up</button>
        </div>
      </form>
    )
  }
}

export default SignUpForm
