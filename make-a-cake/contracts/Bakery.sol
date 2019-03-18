pragma solidity >=0.5.0;

contract Bakery {
  address[] public contracts;
  event LogCakeCreated(address indexed owner, address indexed cakeAddress);

  function getContractCount() 
    public
    view
    returns(uint contractCount)
  {
    return contracts.length;
  }

  // deploy a new contract

  function newCake(string memory cakeType)
    public
    returns(address newContract)
  {
    Cake cake = new Cake(cakeType);
    emit LogCakeCreated(msg.sender,address(cake));
    contracts.push(address(cake));
    return address(cake);
  }
}


contract Cake {
    string public cake;

     constructor(string memory cakeType) public { 
        cake = cakeType;
    }

  function getFlavor()
    public
    pure
    returns (string memory cakeType)
  {
    return cakeType;
  }    
}