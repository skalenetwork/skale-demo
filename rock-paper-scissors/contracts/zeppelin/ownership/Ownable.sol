pragma solidity >=0.4.4;

/*
 * Ownable
 *
 * Base contract with an owner.
 * Provides onlyOwner modifier, which prevents function from running if it is called by anyone other than the owner.
 */
contract Ownable {
  address payable public owner = address(0);

 constructor() public { 
    owner = msg.sender;
  }

  modifier onlyOwner() {
    if (msg.sender == owner)
      _;
  }

  function transferOwnership(address payable newOwner) public payable onlyOwner {
    if (newOwner != address(0)) owner = newOwner;
  }

}
