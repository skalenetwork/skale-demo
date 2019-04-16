import store from '../../store'

export const SHOW_STATUS = 'SHOW_STATUS'
function showStatus(results) {
  return {
    type: SHOW_STATUS,
    payload: results
  }
}

export const HIDE_STATUS = 'HIDE_STATUS'
function hideStatus() {
  return {
    type: HIDE_STATUS,
    payload: ''
  }
}

export async function showMessage(message) {
    store.dispatch(showStatus(message));
}

export async function hideMessage() {
    store.dispatch(hideStatus());
}




