// SPDX-License-Identifier: MIT
pragma solidity 0.8.17;

import '@openzeppelin/contracts/token/ERC721/ERC721.sol';
import '@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol';
import '@openzeppelin/contracts/utils/Counters.sol';

contract GamingToken is ERC721URIStorage {
  using Counters for Counters.Counter;
  Counters.Counter private _tokenCounter;

  constructor() ERC721('Gaming token', 'GMGT') {}

  function mint(string memory tokenURI) external returns (uint256) {
    _tokenCounter.increment();

    uint256 newTokenId = _tokenCounter.current();
    
    _mint(msg.sender, newTokenId);
    _setTokenURI(newTokenId, tokenURI);

    return newTokenId;
  }
}