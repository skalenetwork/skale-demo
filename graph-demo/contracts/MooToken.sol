/**
 * SPDX-License-Identifier: AGPL-3.0-only
 *   MooToken.sol - SKALE Demo token
 *   Copyright (C) 2021-Present SKALE Labs
 *   @author Ebru Engwall
 **/
pragma solidity ^0.8.7;

import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/access/AccessControl.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/utils/Counters.sol";

contract MooToken is ERC721URIStorage, AccessControl, Ownable{

    string private _baseTokenURI;

    using Counters for Counters.Counter;
    Counters.Counter private _tokenIds;

    bytes32 public constant MINTER_ROLE = keccak256("MINTER_ROLE");

    event TokenMinted(address from, uint tokenId, uint userBalance, string tokenURI);
    event TokenUsed(address from, uint tokenId,uint usedAmount);
    event TokenStaked(address from, uint tokenId);
    event TokenUnStaked(address from, uint tokenId);


    mapping(uint => mapping(address => uint)) private _userBalance;
    mapping(uint => mapping(address => uint)) private _used;
    mapping(uint => mapping(address => bool)) private _staked;


    constructor(address minter, string memory baseURI) ERC721("MooToken", "Moo") {
        _setupRole(MINTER_ROLE, minter);
        _baseTokenURI = baseURI;
    }

    function supportsInterface(bytes4 interfaceId) public view virtual override(ERC721, AccessControl) returns (bool) {
        return super.supportsInterface(interfaceId);
    }

    function mint(
        address owner,
        uint userBalance,
        string memory metadataURI
    )
    external
    {
        string memory fullPath;
        if(hasRole(MINTER_ROLE, msg.sender))
        {
            fullPath = concat(concat(_baseTokenURI,toAsciiString(msg.sender)),metadataURI);
        }
        else
        {
            fullPath = concat(_baseTokenURI,metadataURI);
        }

        _tokenIds.increment();
        uint id = _tokenIds.current();
        _mint(owner, id);
        _setTokenURI(id,fullPath);
        _userBalance[id][msg.sender] = userBalance;
        _staked[id][msg.sender]=false;
        emit TokenMinted(owner, id, userBalance, fullPath);
    }

    function use(uint tokenId)  public{
        _used[tokenId][msg.sender]++;
        emit TokenUsed(msg.sender, tokenId, 1);
    }

    function stake(uint tokenId)  public{
        _staked[tokenId][msg.sender]=true;
        emit TokenStaked(msg.sender, tokenId);
    }

    function unStake(uint tokenId) public{
        emit TokenUnStaked(msg.sender, tokenId);
    }

    function getCurrentTokenId() public view returns (uint) {
        return _tokenIds.current();
    }

    function getStake(address member, uint tokenId) public view returns (bool) {
        return _staked[tokenId][member];
    }

    function getBalance(address member, uint tokenId) public view returns (uint) {
        return _userBalance[tokenId][member] - _used[tokenId][member];
    }
    function getUsed(address member, uint tokenId) public view returns (uint) {
        return _used[tokenId][member];
    }


    /********************
    Utility Stuff Starts
    *********************/
    function concat(string memory first, string memory second) private pure returns (string memory){
        return string(abi.encodePacked(first,'/',second));
    }

    function toAsciiString(address x) internal pure returns (string memory) {
        bytes memory s = new bytes(40);
        for (uint i = 0; i < 20; i++) {
            bytes1 b = bytes1(uint8(uint(uint160(x)) / (2**(8*(19 - i)))));
            bytes1 hi = bytes1(uint8(b) / 16);
            bytes1 lo = bytes1(uint8(b) - 16 * uint8(hi));
            s[2*i] = char(hi);
            s[2*i+1] = char(lo);
        }
        return string(s);
    }

    function char(bytes1 b) internal pure returns (bytes1 c) {
        if (uint8(b) < 10) return bytes1(uint8(b) + 0x30);
        else return bytes1(uint8(b) + 0x57);
    }
    /********************
    Utility Stuff Ends
    *********************/

}
