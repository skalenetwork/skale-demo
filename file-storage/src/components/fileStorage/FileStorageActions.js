import store from '../../store'
import { showMessage, hideMessage } from './../status/StatusActions'

export const UPDATE_FILES = 'UPDATE_FILES'

function updateFiles(results) {
  return {
    type: UPDATE_FILES,
    payload: results
  }
}

export async function deleteFile(address, fileName) {
  let {filestorage, web3Instance} = store.getState().web3;
  let privateKey = '0x' + web3Instance.utils.stripHexPrefix(process.env.PRIVATE_KEY);
  showMessage("Deleting your file.");
  await filestorage.deleteFile(address, fileName, privateKey);
  getFiles();
  hideMessage();
}

export async function download(link, index) {
  let filestorage = store.getState().web3.filestorage;
  showMessage("Downloading your file.");
  await filestorage.downloadToFile(link);
  hideMessage();
}

export async function preLoad(link, index) {
  let filestorage = store.getState().web3.filestorage;
  showMessage("Loading your image.");
  let file = await filestorage.downloadToBuffer(link);
      document.getElementById("image_" + index).src = 'data:image/png;base64,' + file.toString('base64');
  hideMessage();
}

export async function upload(fileName, fileData){
  let {account, filestorage, web3Instance} = store.getState().web3;
  let privateKey = '0x' + web3Instance.utils.stripHexPrefix(process.env.PRIVATE_KEY);
  showMessage("Uploading your image.");
  await filestorage.reserveSpace(process.env.ACCOUNT, process.env.ACCOUNT, 10**7, process.env.PRIVATE_KEY)
  let path = await filestorage.uploadFile(account, fileName, fileData, privateKey);
  console.log('STORAGE PATH: ');
  let regExp = /^[^/]+/;
  let urlPath = path.replace(regExp, function(v) { return v.toLowerCase(); })
  console.log('SCHAIN_NAME/' + urlPath);
  hideMessage();
}

export async function getFiles(){
  let {account, filestorage, web3Instance} = store.getState().web3;
  let files = await filestorage.listDirectory(web3Instance.utils.stripHexPrefix(account));
  store.dispatch(updateFiles(files));
}


