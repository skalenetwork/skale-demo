// SPDX-License-Identifier: MIT
pragma solidity 0.8.17;

import '@openzeppelin/contracts/token/ERC721/ERC721.sol';
import '@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol';
import '@openzeppelin/contracts/utils/Counters.sol';
import 'base64-sol/base64.sol';
import "@openzeppelin/contracts/utils/Strings.sol";

contract GamingToken is ERC721URIStorage {
  using Counters for Counters.Counter;
  Counters.Counter private _tokenCounter;

  string[] palette;
  string svg;
  event TokenMinted(address from, uint tokenId, string tokenURI);

  constructor() ERC721('Gaming token', 'GMGT') {
    palette.push("blue");
    palette.push("red");
    palette.push("maroon");
    palette.push("black");
    palette.push("yellow");
    palette.push("orange");
    palette.push("purple");
    palette.push("gold");
    palette.push("lawngreen");
    palette.push("lightblue");
    palette.push("olive");
  }

  function mint() external returns (uint256) {
    _tokenCounter.increment();

    uint256 newTokenId = _tokenCounter.current();
    _mint(msg.sender, newTokenId);
    string memory tokenURI =  constructTokenURI(newTokenId);
    _setTokenURI(newTokenId, tokenURI);

    emit TokenMinted(msg.sender, newTokenId, tokenURI );
    return newTokenId;
  }

  function constructTokenURI(uint tokenId) public returns (string memory) {
    svg = generateSVG(tokenId);
    string memory image = Base64.encode(bytes(svg));

    return string(
      abi.encodePacked(
          "data:image/svg+xml;base64,",
          image
      )
    );
  }

  function generateSVG(uint tokenId) internal view returns (string memory) {
    uint no = getRandomNumber();
    uint rnd10 = (tokenId%10);
    string memory yo_svg = string(abi.encodePacked(
            "<svg height='1100' width='1100' xmlns='http://www.w3.org/2000/svg' version='1.1'> ",
            "<circle cx='",Strings.toString(no%(900-rnd10)),
            "' cy='",Strings.toString(no%(1000-rnd10)),
            "' r='",Strings.toString(no%(100-rnd10)),
            "' stroke='black' stroke-width='3' fill='", palette[no%10],"'/>",

            "<circle cx='",Strings.toString(no%(902-rnd10)),
            "' cy='",Strings.toString(no%(1002-rnd10)),
            "' r='",Strings.toString(no%(102-rnd10)),
            "' stroke='black' stroke-width='3' fill='", palette[no%8],"'/>",


            "<circle cx='",Strings.toString(no%(901-rnd10)),
            "' cy='",Strings.toString(no%(1001-rnd10)),
            "' r='",Strings.toString(no%(101-rnd10)),
            "' stroke='black' stroke-width='3' fill='", palette[no%5],"'/>",

            "</svg>"
        ));

    return yo_svg;
  }
  function getRandomNumber() public view returns (uint )
    {
        bytes32 res = getRandom();
        uint256 num = uint256(res);
        return num;
    }

  function getRandom() private view returns (bytes32 addr) {
      assembly {
          let freemem := mload(0x40)
          let start_addr := add(freemem, 0)
          if iszero(staticcall(gas(), 0x18, 0, 0, start_addr, 32)) {
              invalid()
          }
          addr := mload(freemem)
      }
  }
}