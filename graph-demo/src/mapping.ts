import {TokenMinted, Transfer, TokenUsed, TokenUnStaked, TokenStaked} from '../generated/MooToken/MooToken'
import {MyMooToken, Owner} from '../generated/schema'
import {  BigInt } from '@graphprotocol/graph-ts'

let ONE = BigInt.fromI32(1)
let ZERO = BigInt.fromI32(0)

export function handleNewMooToken(event: TokenMinted): void {
  let id = event.params.tokenId.toString();
  let tokenMinted = new MyMooToken(id)
  let owner = Owner.load(event.params.from.toString())
  if(owner!=null)
  {
    owner.totalCountTokens = owner.totalCountTokens.plus(ONE)
  }
  else
  {
    owner = new Owner(event.params.from.toString())
    owner.totalCountTokens = ONE;
    owner.from = event.params.from;
  }
  owner.save();
  tokenMinted.tokOwner = owner.id;
  tokenMinted.owner = event.params.from
  tokenMinted.tokenURI = event.params.tokenURI
  tokenMinted.used = 0
  tokenMinted.startingBalance = event.params.userBalance;
  tokenMinted.balanceNow = event.params.userBalance;
  tokenMinted.isStaked = false
  tokenMinted.stakeCount = 0
  tokenMinted.stakeTimeStamp = ZERO;
  tokenMinted.save()
}

export function handleTransferMooToken(event: Transfer): void {

  let tokenId = event.params.tokenId.toString();
  let tokenTransfered = MyMooToken.load(tokenId)
  if (tokenTransfered) {
    tokenTransfered.owner = event.params.to;
    tokenTransfered.save()
    let owner = Owner.load(event.params.to.toString())
    if (owner != null) {
      owner.totalCountTokens = owner.totalCountTokens.minus(ONE)
    } else {
      owner = new Owner(event.params.to.toString())
      owner.totalCountTokens = ONE;
    }
    owner.save();

    tokenTransfered.tokOwner = owner.id;
  }
}

export function handleUseMooToken(event: TokenUsed): void {
  let id = event.params.tokenId.toString();
  let tokenMinted = MyMooToken.load(id)
  if (tokenMinted) {
    tokenMinted.used = tokenMinted.used + 1
    tokenMinted.balanceNow = tokenMinted.balanceNow.minus(ONE)
    tokenMinted.save()
  }
}

export function handleStakeMooToken(event: TokenStaked): void {
  let id = event.params.tokenId.toString();
  let tokenMinted = MyMooToken.load(id)
  if (tokenMinted) {
    tokenMinted.isStaked = true;
    tokenMinted.stakeTimeStamp = event.block.timestamp;
    tokenMinted.stakeCount = tokenMinted.stakeCount +1
    tokenMinted.save();
  }
}

export function handleUnStakeMooToken(event: TokenUnStaked): void {
  let id = event.params.tokenId.toString();
  let tokenMinted = MyMooToken.load(id)
  if (tokenMinted) {
    tokenMinted.isStaked = false;
    tokenMinted.save()
  }
}

