import React, { Component } from 'react'

import status from './../../assets/status.png'

import './styles.scss';

class Status extends Component {

  render() {
    const {message, show} = this.props;
    return (
      <div className={"status rounded px-2 py-4 " + (show ? "" : "disable")}>
        <div className="message row">
          <div className="col-1">
            <img src={status} alt="status"/>
          </div>
          <div className="col-11">
            <h6 className="pl-2 message message">{message}</h6>
          </div>
        </div>
      </div>
    );
  }
}

export default Status
