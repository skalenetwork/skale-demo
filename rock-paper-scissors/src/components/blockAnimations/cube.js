import React, { Component } from 'react'
import './styles.scss';

// @author: Christine Perry
class BlockAnimation extends Component {
	constructor (props) {
    super(props)
    this.state = {
      show: false
    }
  }

  handleClick (event) {
    event.preventDefault();
    this.setState({
      show: !this.state.show
    });
    
  }

	render() {
    const { blockNumber, date, count, gasUsed, hash, parentHash } = this.props;
    const { show } = this.state;
    return (
      <div>
			<div onClick={(event) => this.handleClick(event)} 
        className={"smallBlock " + (show ? "disable" : "")}>
					<div className="cube">
	          <div className="top"></div>
	          <div className="right cubeText">{blockNumber}</div>
	          <div className="bottom"></div>
	          <div className="left"></div>
	          <div className="back"></div>
	          <div className="front cubeText">Block</div>
	        </div>
			</div> 
      <div onClick={(event) => this.handleClick(event)} 
        className={"showBlock " + (show ? "" : "disable")}>
          <div className="cube">
            <div className="top"></div>
            <div className="right cubeTextData data">
              <span className="yellow">Block Number:</span> {blockNumber}<br/>
              <span className="yellow">Date:</span> {date}<br/>
              <span className="yellow">Transaction Count:</span> {count}<br/>
              <span className="yellow">Gas Used:</span> {gasUsed}<br/>
              <span className="yellow">Hash:</span><br/> {hash}<br/>
              <span className="yellow">Parent Hash:</span><br/> {parentHash}

            </div>
            <div className="bottom"></div>
            <div className="left"></div>
            <div className="back"></div>
            <div className="front cubeText">Block <br/>{blockNumber}</div>
          </div>
      </div> 
      </div>  
		)
  }
}

export default BlockAnimation
