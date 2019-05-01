import { connect } from 'react-redux'
import MakeCake from './MakeCake'
import { addIngredient, makeCake, getFiles, preLoad, upload, deleteFile, download } from './MakeCakeActions'
 
const mapStateToProps = (state, ownProps) => {
  return {
    files: state.web3.files,
    ingredients: state.web3.ingredients,
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
            await upload(
              file.name, 
              file.size, 
              reader.result
            );
            getFiles();
        };
        reader.readAsArrayBuffer(file);
    },
    onDownload: (event, link, index) => {
      event.preventDefault();
      download(link, index);
    },
    onPreDownload: (event, link) => {
      event.preventDefault();
      preLoad(link);
    },
    onAdd: (event, link) => {
      event.preventDefault();
      addIngredient(link);
    },
    onDelete: (event, address, fileName) => {
      event.preventDefault();
      deleteFile(address, fileName);
    },
    onMakeCake: (event, value) => {
      event.preventDefault();
      makeCake(value);
    },
  }
}

const MakeCakeContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(MakeCake)

export default MakeCakeContainer
