import React, { Component } from 'react'
import {formatBytes} from './../../utils/utils'
import './styles.scss';
import image from './../../assets/transparent.png'

import eggs from './../../assets/eggs.png'
import flour from './../../assets/flour.png'
import milk from './../../assets/milk.png'
import fruit from './../../assets/fruit.png'
import cheese from './../../assets/cheese.png'

import cheesecake from './../../assets/cheesecake.png'
import fruitcake from './../../assets/fruitcake.png'
import tiramisu from './../../assets/tiramisu.png'

class MakeCake extends Component {

  componentDidMount() {
    document.getElementById("cakeShow").classList.add("disable"); 
  }

  close (event) {
    event.preventDefault();
    var skaleOn = document.getElementById("cakeShow").classList.add("disable"); 
  }

  render() {
    const {account, files, ingredients} = this.props;
    
    return (
      <div className="filestorage">
      <div className="center cakeShow" id="cakeShow" onClick={(event) => this.close(event)}>
        <img className="cakeMade" id="cakeMade" onClick={(event) => this.close(event)} src={fruitcake} />
      </div>
        <h1 className="underline-yellow mb-5">Have Your Cake</h1>
        <div className="center">
          <div className="dropdown">
            <button className="btn btn-outline-primary yellow-button dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
              Ingredients
            </button>
            <div className="dropdown-menu" aria-labelledby="dropdownMenuButton">
              <a onClick={(event) => this.props.onAdd(event, eggs)} className="dropdown-item center" href="#">eggs</a>
              <a  onClick={(event) => this.props.onAdd(event, flour)} className="dropdown-item center" href="#">flour</a>
              <a  onClick={(event) => this.props.onAdd(event, milk)} className="dropdown-item center" href="#">milk</a>
              <a  onClick={(event) => this.props.onAdd(event, fruit)} className="dropdown-item center" href="#">fruit</a>
              <a  onClick={(event) => this.props.onAdd(event, cheese)} className="dropdown-item center" href="#">cheese</a>

              {files.map((item, index) => (
              <a key={index} onClick={(event) => this.props.onPreDownload(event, account.slice(2) + "/" + item.name)} className="dropdown-item center" href="#">{item.name.split('.')[0]}</a>
              ))}
              <div className="dropdown-divider"></div>
              <a className="dropdown-item center" href="#">
                <input onChange={(event) => this.props.onUpload(event)}
                  type="file" id="files" 
                  className="fileUpload " 
                  data-multiple-caption="{count} files selected" />
                <label htmlFor="files">
                  <svg xmlns="http://www.w3.org/2000/svg" width="20" height="17" viewBox="0 0 20 17"><path d="M10 0l-5.2 4.9h3.3v5.1h3.8v-5.1h3.3l-5.2-4.9zm9.3 11.5l-3.2-2.1h-2l3.4 2.6h-3.5c-.1 0-.2.1-.2.1l-.8 2.3h-6l-.8-2.2c-.1-.1-.1-.2-.2-.2h-3.6l3.4-2.6h-2l-3.2 2.1c-.4.3-.7 1-.6 1.5l.6 3.1c.1.5.7.9 1.2.9h16.3c.6 0 1.1-.4 1.3-.9l.6-3.1c.1-.5-.2-1.2-.7-1.5z"></path>
                  </svg> Add Ingredient
                </label>
              </a>
            </div>
          </div>
        </div>
        <div className="row justify-content-center">
          <div className="col-12 py-5 center">
            <a onClick={(event) => this.props.onDelete(event, 0)}>
              <img className="rps" id="ingredient_0"/>
            </a>
            <a onClick={(event) => this.props.onDelete(event, 1)}>
              <img className="rps" id="ingredient_1"/>
            </a>
            <a onClick={(event) => this.props.onDelete(event, account, 2)}>
              <img className="rps" id="ingredient_2"/>
            </a>
            <a onClick={(event) => this.props.onDelete(event, account, 2)}>
              <img className="rps" id="ingredient_3"/>
            </a>
          </div>
        </div>
        <div className="bottom">
          <button className="btn btn-outline-primary yellow-button center"
          onClick={(event) => this.props.onMakeCake(event, "Cake Recipe")}>Make Cake
          </button>
        </div>
      </div>
    );
  }
}
export default MakeCake
