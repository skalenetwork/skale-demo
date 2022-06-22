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
    event TokenMinted(address from, uint tokenId, string tokenURI);
    mapping(uint => uint) public last_tokenid;


    constructor() ERC721("StrangeToken", "Strange") {
        last_tokenid[0] = 0;
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
        last_tokenid[0] = max(last_tokenid[0], id);
        string memory tokenURI =  constructTokenURI();
        _setTokenURI(id,tokenURI);
        emit TokenMinted(msg.sender, id, tokenURI );
    }

    function max(uint highest_val,uint new_value) private pure returns (uint) {
        if(new_value <highest_val)
        {
            return highest_val;
        }
        else
        {
            return new_value;
        }
    }

    function getSVG() public view returns (string memory) {
        return svg;
    }
    function getCurrentTokenId() public view returns (uint) {
        return last_tokenid[0];
    }

    /********************
    Utility Stuff Starts
    *********************/

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


    function constructTokenURI() public returns (string memory) {
        svg = generateSVG();
        string memory image = Base64.encode(bytes(svg));

        return
        string(
            abi.encodePacked(
                "data:image/svg+xml;base64,",
                image
            )
        );
    }



    function generateSVG() internal view returns (string memory) {

        uint no = getRandomNumber();

        string memory yo_svg = string(abi.encodePacked(
                "<svg height='1100' width='1100' xmlns='http://www.w3.org/2000/svg' version='1.1'> ",
                "<circle cx='",Strings.toString(no%900),
                    "' cy='",Strings.toString(no%1000),
                    "' r='",Strings.toString(no%100),
                    "' stroke='black' stroke-width='3' fill='", palette[no%10],"'/>",

                    "<circle cx='",Strings.toString(no%902),
                    "' cy='",Strings.toString(no%1002),
                    "' r='",Strings.toString(no%102),
                    "' stroke='black' stroke-width='3' fill='", palette[no%8],"'/>",


                    "<circle cx='",Strings.toString(no%904),
                    "' cy='",Strings.toString(no%1001),
                    "' r='",Strings.toString(no%101),
                    "' stroke='black' stroke-width='3' fill='", palette[no%9],"'/>",

                "</svg>"
            ));

        return yo_svg;
    }


    /********************
    Utility Stuff Ends
    *********************/
}
