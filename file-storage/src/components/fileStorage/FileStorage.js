import React, { Component } from 'react'
import {formatBytes} from './../../utils/utils'
import './styles.scss';
import image from './../../assets/placeholder-image.png'
import file from './../../assets/placeholder-file.png'

class FileStorage extends Component {

  render() {
    const {account, files} = this.props;
    return (
      <div className="filestorage">
        <div className="center">
          <input onChange={(event) => this.props.onUpload(event)}
            type="file" id="files" 
            className="fileUpload " 
            data-multiple-caption="{count} files selected" />
          <label htmlFor="files">
            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="17" viewBox="0 0 20 17"><path d="M10 0l-5.2 4.9h3.3v5.1h3.8v-5.1h3.3l-5.2-4.9zm9.3 11.5l-3.2-2.1h-2l3.4 2.6h-3.5c-.1 0-.2.1-.2.1l-.8 2.3h-6l-.8-2.2c-.1-.1-.1-.2-.2-.2h-3.6l3.4-2.6h-2l-3.2 2.1c-.4.3-.7 1-.6 1.5l.6 3.1c.1.5.7.9 1.2.9h16.3c.6 0 1.1-.4 1.3-.9l.6-3.1c.1-.5-.2-1.2-.7-1.5z"></path>
            </svg>Choose a file
          </label>
        </div>
        <div className="files row p-5">
          <h4 className="py-2 px-1">My Files</h4>
          {files.map((item, index) => (
            <div key={index} className="card col-12 border-top">
              <div className="card-body d-flex">
              <div className="image-container border-right center">
                <button className="ml-3 btn btn-outline-primary clear-button"
                onClick={(event) => this.props.onPreDownload(event, account.slice(2) + "/" + item.name, index)}>
                  <img className="" id={"image_" + index} src={item.name.match(/\.(gif|jpe?g|png)$/i) ? image : file} alt={item.name}/>
                </button>
              </div>
              <button className="ml-3 btn btn-outline-primary clear-button"
              onClick={(event) => this.props.onDownload(event, account.slice(2) + "/" + item.name, index)}>
                <h5 className="justify-content-end">{item.name} | {formatBytes(item.size)}</h5>
              </button>
              <div className="ml-auto justify-content-end"><button className="btn btn-outline-primary yellow-button"
              onClick={(event) => this.props.onDelete(event, account, item.name)}>Delete</button></div>
              </div>
            </div>
            ))}
        </div>
      </div>
    );
  }
}

export default FileStorage
