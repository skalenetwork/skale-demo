// SPDX-License-Identifier: AGPL-3.0-only

/**
 *   Shack15NFTToken.sol - SKALE Interchain Messaging Agent Test tokens
 *   Copyright (C) 2021-Present SKALE Labs
 *   @author Artem Payvin
 *
 *   SKALE IMA is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU Affero General Public License as published
 *   by the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   SKALE IMA is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU Affero General Public License for more details.
 *
 *   You should have received a copy of the GNU Affero General Public License
 *   along with SKALE IMA.  If not, see <https://www.gnu.org/licenses/>.
 */

pragma solidity ^0.8.7;

import "@openzeppelin/contracts/token/ERC1155/extensions/ERC1155Burnable.sol";
import "@openzeppelin/contracts/token/ERC1155/extensions/IERC1155MetadataURI.sol";
import "@openzeppelin/contracts/access/AccessControlEnumerable.sol";

contract Shack15NFTToken is AccessControlEnumerable, ERC1155 {

    enum TokenType {NONE, PAYITFORWARD, USABLE}

    event TokenMinted(address from, uint256 tokenId, uint256 amount,  TokenType nfttype, string tokenURI);

    event TokenUsed(address from, uint256 tokenId, uint256 amount);

    bytes32 public constant MINTER_ROLE = keccak256("MINTER_ROLE");

    bytes32 public constant MARKET_PLACE_ROLE = keccak256("MARKET_PLACE_ROLE");

    bytes32 public constant PAYITFORWARD_HASH = keccak256("PAYITFORWARD");

    bytes32 public constant USABLE_HASH = keccak256("USABLE");

    //     tokenId  =>         member  => TokenType
    mapping(uint256 => mapping(address => TokenType)) private _types;

    //      tokenId =>        member   => amount
    mapping(uint256 => mapping(address => uint256)) private _locked;

    mapping(address => mapping(uint256 => bool)) private _marketPlaceAcceptance;

    mapping(uint256 => string) private _imagePath;

    uint256 private _currentTokenID = 0;

    constructor(
        string memory uri_
    )
        ERC1155(uri_)
    {
        _setRoleAdmin(MINTER_ROLE, MINTER_ROLE);
        _setRoleAdmin(MARKET_PLACE_ROLE, MINTER_ROLE);
        _setupRole(MINTER_ROLE, _msgSender());
    }

    function mint(
        address account,
        uint256 amount,
        bytes memory data,
        string memory tokenURI
    )
        external
    {
        uint256 id = _getNextTokenID();
        require(hasRole(MINTER_ROLE, _msgSender()), "Sender is not a Minter");
        _mint(account, id, amount, data);
        _imagePath[id] = tokenURI;
        _incrementTokenTypeId();
        emit TokenMinted(account, id, amount, _types[id][account], tokenURI);
    }

    function mintBatch(
        address account,
        uint256[] memory ids,
        uint256[] memory amounts,
        bytes memory data
    )
        external
    {
        require(hasRole(MINTER_ROLE, _msgSender()), "Sender is not a Minter");
        _mintBatch(account, ids, amounts, data);
    }

    function supportsInterface(bytes4 interfaceId)
        public
        view
        override(AccessControlEnumerable, ERC1155)
        returns (bool)
    {
        return interfaceId == bytes4(keccak256(abi.encodePacked("mint(address,uint256,uint256,bytes)")))
            || interfaceId == bytes4(keccak256(abi.encodePacked("mintBatch(address,uint256[],uint256[],bytes)")))
            || interfaceId == bytes4(keccak256(abi.encodePacked("lock(address,uint256)")))
            || interfaceId == bytes4(keccak256(abi.encodePacked("addMarketPlaceAcceptance(uint256)")))
            || super.supportsInterface(interfaceId);
    }

    // by member
    function lock(address marketPlace, uint256 tokenId) external {
        require(
            balanceOf(_msgSender(), tokenId) >= 1 + _locked[tokenId][_msgSender()],
            "Shack15NFTToken: Not enough tokens to lock"
        );
        require(hasRole(MARKET_PLACE_ROLE, marketPlace), "Shack15NFTToken: Market place not authorized");
        require(
        _types[tokenId][_msgSender()] == TokenType.USABLE,
            "Shack15NFTToken: You can only use the USABLE Token"
        );
        _locked[tokenId][_msgSender()]++;
        emit TokenUsed(_msgSender(), tokenId, 1);
    }

//    function setTokenURI(uint256 id, string memory tokenURI) external {
//        require(hasRole(MINTER_ROLE, _msgSender()), "Sender is not a Minter");
//        _imagePath[id] = tokenURI;
//        emit TokenURISet(id, tokenURI);
//    }

    function getTokenURI(uint256 tokenId)  external view returns (string memory) {
        return _imagePath[tokenId];
    }

    function getType(address member, uint256 tokenId) external view returns (TokenType) {
        return _types[tokenId][member];
    }

    function getLocked(address member, uint256 tokenId) external view returns (uint256) {
        return _locked[tokenId][member];
    }

    function _getNextTokenID() private view returns (uint256) {
        return _currentTokenID+1;
    }

    function _incrementTokenTypeId() private  {
        _currentTokenID++;
    }

    function uri(uint256 id) public view override(ERC1155) returns (string memory) {
        return string(abi.encodePacked(ERC1155.uri(id), _imagePath[id]));
    }

    /**
     * @dev Hook that is called before any token transfer. This includes minting
     * and burning, as well as batched variants.
     *
     * The same hook is called on both single and batched variants. For single
     * transfers, the length of the `id` and `amount` arrays will be 1.
     *
     * Calling conditions (for each `id` and `amount` pair):
     *
     * - When `from` and `to` are both non-zero, `amount` of ``from``'s tokens
     * of token type `id` will be  transferred to `to`.
     * - When `from` is zero, `amount` tokens of token type `id` will be minted
     * for `to`.
     * - when `to` is zero, `amount` of ``from``'s tokens of token type `id`
     * will be burned.
     * - `from` and `to` are never both zero.
     * - `ids` and `amounts` have the same, non-zero length.
     *
     * To learn more about hooks, head to xref:ROOT:extending-contracts.adoc#using-hooks[Using Hooks].
     */
    function _beforeTokenTransfer(
        address ,
        address from,
        address to,
        uint256[] memory ids,
        uint256[] memory amounts,
        bytes memory data
    )
        internal
        override
    {
        if (from != address(0)) {
            //transfer
            for (uint i = 0; i < ids.length; i++) {
                require(balanceOf(from, ids[i]) >= amounts[i], "Shack15NFTToken: Insufficient balance");
                require(
                    _types[ids[i]][from] == TokenType.PAYITFORWARD,
                    "Shack15NFTToken: This token is not transferable"
                );
                if (balanceOf(from, ids[i]) == amounts[i])
                    _types[ids[i]][from] = TokenType.NONE;
                _types[ids[i]][to] = TokenType.USABLE;
            }
        } else {
            // mint
            require(
                bytes32(abi.encodePacked(data)) == PAYITFORWARD_HASH ||
                bytes32(abi.encodePacked(data)) == USABLE_HASH,
                "Shack15NFTToken: Input should be 1 for PAYITFORWARD or 2 for USABLE"
            );
            TokenType newTokenType = (
                bytes32(abi.encodePacked(data)) == PAYITFORWARD_HASH ?
                TokenType.PAYITFORWARD : TokenType.USABLE
            );
            for (uint i = 0; i < ids.length; i++) {
                require(
                    balanceOf(to, ids[i]) == 0 || _types[ids[i]][to] == newTokenType,
                    "Shack15NFTToken: Incorrect minting type of token"
                );
                _types[ids[i]][to] = newTokenType;
            }
        }
    }

}
