import { connect } from 'react-redux'
import FileStorage from './FileStorage'
import { getFiles, preLoad, upload, deleteFile, download } from './FileStorageActions'
 
const mapStateToProps = (state, ownProps) => {
  return {
    files: state.web3.files,
    account: state.web3.account
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    onUpload: (event) => {
      event.preventDefault();
      let file = document.getElementById('files').files[0];
        let reader = new FileReader();

        reader.onload = async function(e) {
          var arrayBuffer = reader.result
          var bytes = new Uint8Array(arrayBuffer);
          await upload(
            file.name, 
            bytes
          );
          getFiles();
        };
        reader.readAsArrayBuffer(file);
    },
    onDownload: (event, link, index) => {
      event.preventDefault();
      download(link, index);
    },
    onPreDownload: (event, link, index) => {
      event.preventDefault();
      preLoad(link, index);
    },
    onDelete: (event, address, fileName) => {
      event.preventDefault();
      deleteFile(address, fileName);
    },
  }
}

const FileStorageContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(FileStorage)

export default FileStorageContainer
