import {TokenMinted, Transfer, TokenUsed, TokenUnStaked, TokenStaked} from '../generated/Strangeen/Strangeen'
import {MyStrangeToken, Owner} from '../generated/schema'
import {  BigInt } from '@graphprotocol/graph-ts'

let ONE = BigInt.fromI32(1)
let ZERO = BigInt.fromI32(0)

export function handleNewStrangeToken(event: TokenMinted): void {
  let id = event.params.tokenId.toString();
  let tokenMinted = new MyStrangeToken(id)
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

export function handleTransferStrangeToken(event: Transfer): void {

  let tokenId = event.params.tokenId.toString();
  let tokenTransfered = MyStrangeToken.load(tokenId)
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

export function handleUseStrangeToken(event: TokenUsed): void {
  let id = event.params.tokenId.toString();
  let tokenMinted = MyStrangeToken.load(id)
  if (tokenMinted) {
    tokenMinted.used = tokenMinted.used + 1
    tokenMinted.balanceNow = tokenMinted.balanceNow.minus(ONE)
    tokenMinted.save()
  }
}

export function handleStakeStrangeToken(event: TokenStaked): void {
  let id = event.params.tokenId.toString();
  let tokenMinted = MyStrangeToken.load(id)
  if (tokenMinted) {
    tokenMinted.isStaked = true;
    tokenMinted.stakeTimeStamp = event.block.timestamp;
    tokenMinted.stakeCount = tokenMinted.stakeCount +1
    tokenMinted.save();
  }
}

export function handleUnStakeStrangeToken(event: TokenUnStaked): void {
  let id = event.params.tokenId.toString();
  let tokenMinted = MyStrangeToken.load(id)
  if (tokenMinted) {
    tokenMinted.isStaked = false;
    tokenMinted.save()
  }
}

