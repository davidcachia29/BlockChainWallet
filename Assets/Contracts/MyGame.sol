pragma solidity ^0.5.11;

contract MyGame{

    //THE CONTRACT DOES THE FOLLOWING:
    //EVERY USER CAN HAVE HIS/HER OWN INVENTORY. (IN THIS SCENARIO THE INVENTORY CONTAINS :MANA and GOLD)
    //EVERY INVENTORY IS BINDED TO AN ACCOUNT ADDRESS
    //USER CAN BUY MANA FOR 1000 WEI
    //Upon registration the player receives 200 gold
    //Player can transfer gold to another player
    

    struct Inventory{
        uint mana;
        uint gold;
    }

    mapping (address => Inventory) inventories; //allow us to look up a specific store with user's Ethereum address, and retrieve their mana
    address[] private playersAccts; //array that stores all addresses
    mapping (address => bool) private accounts;
    
    
    function createInventoryForPlayer() public{ //gives 200 gold only for one time for player
        address _address = msg.sender; //msg.sender is the address that is interacting with the contract
        MyGame.Inventory storage objInventory = inventories[_address]; 
        
        if(containsAddress(_address)== false){ //if address does not exist then add to array storesAccts
            accounts[_address] = true;
            playersAccts.push(_address);
            objInventory.gold +=200; //give 200 gold
        }
    }
    
    function transferGold(address _addressTo, uint goldAmmount) public{
        address _addressFrom = msg.sender;
        MyGame.Inventory storage objInventoryAddressFrom = inventories[_addressFrom]; //load the inventory of who is going to transfer gold
        MyGame.Inventory storage objInventoryAddressTo = inventories[_addressTo]; //load the inventory of who is going to receive gold
        
        require (objInventoryAddressFrom.gold >= goldAmmount); //make sure that the from account has enough gold to transfer
        objInventoryAddressFrom.gold -= goldAmmount;
        objInventoryAddressTo.gold += goldAmmount;
    }

    function buyManaAndReturnChange() payable public{
          uint minAmount = 1000;
          require (msg.value >= minAmount);
          uint moneyToReturn = msg.value - minAmount;

          addManaToAddress(msg.sender); //msg.sender is the address that is interacting with the contract

          if(moneyToReturn > 0) //send change if user payed more than 1000
            msg.sender.transfer(moneyToReturn);
    }

    function addManaToAddress(address _address) private{ //add mana to account
        MyGame.Inventory storage objInventory = inventories[_address]; //add binding with address and return object. If object not found it is auto created
        objInventory.mana +=100; //modify mana's returned object

        if(containsAddress(_address)== false){ //if address does not exist then add to array storesAccts
            accounts[_address] = true;
            playersAccts.push(_address);
        }
    }

    function getPlayersAccts() view public returns (address[] memory) { //returns a list of addresses from storesAccts
        return playersAccts;
    }

    function getManaForAddress(address addrs) view public returns (uint) { //returns the mana for particular address
        return (inventories[addrs].mana);
    }
    
    function getGoldForAddress(address addrs) view public returns (uint) { //returns the gold for particular address
        return (inventories[addrs].gold);
    }

    function countInventories() view public returns (uint) { //give you the number of stores that exist in this contract
        return playersAccts.length;
    }

    function containsAddress(address _address) private returns (bool){ //check if accounts already contains address
        return accounts[_address];
    }

}
