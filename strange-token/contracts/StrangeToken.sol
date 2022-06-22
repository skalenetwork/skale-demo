/**
 * SPDX-License-Identifier: AGPL-3.0-only
 *   StrangeToken.sol - SKALE Demo token
 *   Copyright (C) 2021-Present SKALE Labs
 *   @author Ebru Engwall
 **/
pragma solidity ^0.8.7;

import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/utils/Counters.sol";
import 'base64-sol/base64.sol';
import "@openzeppelin/contracts/utils/Strings.sol";

contract StrangeToken is ERC721URIStorage{

    // using Counters for Counters.Counter;
    // Counters.Counter private _tokenIds;
    string[] palette;
    string svg;
    uint last_tokenid;
    event TokenMinted(address from, uint tokenId, string tokenURI);


    constructor() ERC721("StrangeToken", "Strange") {
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

    function mint(uint id)
    external
    {
        // _tokenIds.increment();
        // uint id = _tokenIds.current();
        _mint(msg.sender, id);
        last_tokenid = id;
        string memory tokenURI =  constructTokenURI();
        _setTokenURI(id,tokenURI);
        emit TokenMinted(msg.sender, id, tokenURI );
    }

    function getSVG() public view returns (string memory) {
        return svg;
    }
    function getCurrentTokenId() public view returns (uint ) {
        return last_tokenid;
    }

    /********************
    Utility Stuff Starts
    *********************/
    function concat(string memory first, string memory second) private pure returns (string memory){
        return string(abi.encodePacked(first,'/',second));
    }

    function getRandomNumber(uint mode) public view returns (uint )
    {
        bytes32 res = getRandom();
        uint256 num = uint256(res) % mode;
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


    function constructTokenURI() public returns (string memory) {
        svg = generateSVG();
        string memory image = Base64.encode(bytes(svg));

        return
        string(
            abi.encodePacked(
                "data:image/svg+xml;base64",
                image
            )
        );
    }



    function generateSVG() internal view returns (string memory) {
        uint palette_no = getRandomNumber(10);
        string memory x_axis = Strings.toString(getRandomNumber(900));
        string memory y_axis = Strings.toString(getRandomNumber(1000));
        string memory radius = Strings.toString(getRandomNumber(100));

        string memory yo_svg = string(abi.encodePacked(
                "<svg height='1100' width='1100' xmlns='http://www.w3.org/2000/svg' version='1.1'> ",
                "<circle cx='",x_axis,"' cy='",y_axis,"' r='",radius,"' stroke='black' stroke-width='3' fill='",
                palette[palette_no],"'/>",
                "</svg>"
            ));

        return yo_svg;
    }


    /********************
    Utility Stuff Ends
    *********************/
}
