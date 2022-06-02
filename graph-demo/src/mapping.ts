import {TokenMinted,Transfer,TokenUsed} from '../generated/MooToken/MooToken'
import { MyMooToken } from '../generated/schema'



export function handleNewMooToken(event: TokenMinted): void {
  let id = (event.params.from.toHex()+"-"+ event.params.tokenId.toString());

  let tokenMinted = new MyMooToken(id)
  tokenMinted.from = event.params.from
  tokenMinted.tokenURI = event.params.tokenURI

  tokenMinted.save()

}

export function handleTransferMooToken(event: Transfer): void {

  let old_id = (event.params.from.toHex()+"-"+ event.params.tokenId.toString());
  let new_id = event.params.to.toHex() + "-" + event.params.tokenId.toString();

  let tokenMinted = MyMooToken.load(old_id)
  let tokenTransfered = MyMooToken.load(new_id)
  if(tokenTransfered==null) {
    let tokenTransfered = new MyMooToken(new_id)
    tokenTransfered.from = event.params.to
    // tokenTransfered.used = ZERO;
    tokenTransfered.tokenURI = tokenMinted.tokenURI
    tokenTransfered.save()
  }
  else
  {
    if (tokenMinted) {
      tokenTransfered.tokenURI = tokenMinted.tokenURI
    }
    tokenTransfered.save()
  }
}

  export function handleUseMooToken(event: TokenUsed): void {
      let id = event.params.from.toHex() + "-" + event.params.tokenId.toString();

      let tokenMinted = MyMooToken.load(id)
      if (tokenMinted != null) {
          // tokenMinted.used = tokenMinted.used.plus(ONE)
          tokenMinted.save()
      }
  }

