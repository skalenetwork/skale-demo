import { connect } from 'react-redux'
import Status from './Status'
import { showMessage, hideMessage } from './StatusActions'
 
const mapStateToProps = (state, ownProps) => {
  return {
    message: state.status.message,
    show: state.status.show
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    onShowMessage: (event, message) => {
      event.preventDefault();
      showMessage(message);
    },
    onHideMessage: (event) => {
      event.preventDefault();
      hideMessage();
    },
  }
}

const StatusContainer = connect(
  mapStateToProps,
  mapDispatchToProps
)(Status)

export default StatusContainer
