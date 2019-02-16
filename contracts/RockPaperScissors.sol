pragma solidity >=0.4.20;

//SKALE - Rock Paper Scissors
contract RockPaperScissors {
    mapping (string => mapping(string => int)) payoffMatrix;
    mapping (int => string) computerMatrix;
    address payable player1 = address(0);
    address payable player2 = address(0);
    string public player1Choice;
    string public player2Choice;

    /// @dev Event to log player1 and player 2
    event LogPlay(address player1, address player2, string player1Choice, string player2Choice);
    event LogSoloPlay(string playerChoice, string computerChoice);
    event Winner(address winningPlayer, string winner, string player1Choice, string player2Choice);
    
    modifier notRegisteredYet() {
        require(
            msg.sender != player1 && msg.sender != player2,
            "Not Registered."
        );
        _;
    }
    
    modifier sentEnoughCash(uint amount) {
        require(
            msg.value >= amount,
            "More cash needed!"
        );
        _;
    }
    
    
    constructor() public { 
        payoffMatrix["rock"]["rock"] = 0;
        payoffMatrix["rock"]["paper"] = 2;
        payoffMatrix["rock"]["scissors"] = 1;
        payoffMatrix["paper"]["rock"] = 1;
        payoffMatrix["paper"]["paper"] = 0;
        payoffMatrix["paper"]["scissors"] = 2;
        payoffMatrix["scissors"]["rock"] = 2;
        payoffMatrix["scissors"]["paper"] = 1;
        payoffMatrix["scissors"]["scissors"] = 0;

        computerMatrix[0] = "rock";
        computerMatrix[1] = "paper";
        computerMatrix[2] = "scissors";
    }   

    function getWinner() public view returns (int x) {
        return payoffMatrix[player1Choice][player2Choice];
    }
    
    function play(string memory choice) payable public returns (int w) {
        if (msg.sender == player1)
            player1Choice = choice;
        else if (msg.sender == player2)
            player2Choice = choice;
        if (bytes(player1Choice).length != 0 && bytes(player2Choice).length != 0)
        {
            emit LogPlay(player1, player2, player1Choice, player2Choice);
            
            int winner = payoffMatrix[player1Choice][player2Choice];
            if (winner == 1) {
                emit Winner(player1, "Player 1 Wins", player1Choice, player2Choice); 
                player1.transfer(address(this).balance);
            } else if (winner == 2) {
                emit Winner (player2, "Player 2 Wins", player1Choice, player2Choice);
                player2.transfer(address(this).balance);
            }else
            {
                emit Winner(player1, "Tie", player1Choice, player2Choice); 
                player1.transfer(address(this).balance/2);
                player2.transfer(address(this).balance);
            }
             
            // unregister players and choices
            player1Choice = "";
            player2Choice = "";
            player1 = address(0);
            player2 = address(0);
            return winner;
        }
        else 
            return -1;
    }

    function playSolo(string memory choice, int computerChoice) payable public returns (int w) {
        if (bytes(choice).length != 0 && !(computerChoice < 0))
        {
            string memory computerPlay = computerMatrix[computerChoice % 3];
            emit LogSoloPlay(choice, computerPlay);
            
            int winner = payoffMatrix[choice][computerPlay];
            if (winner == 1) {
                emit Winner(msg.sender, "You Win", choice, computerPlay); 
            } else if (winner == 2) {
                emit Winner (address(this), "You Lose", choice, computerPlay);
            }else
            {
                emit Winner (address(this), "Tie", choice, computerPlay);
            }
             
            return winner;
        }
        else 
            return -1;
    }

    function computerWar(int computerChoice, int computerChoice2) payable public returns (int w) {
        if (!(computerChoice < 0) && !(computerChoice < 0))
        {
            string memory computerPlay = computerMatrix[computerChoice % 3];
            string memory computerPlay2 = computerMatrix[computerChoice2 % 3];
            emit LogSoloPlay(computerPlay, computerPlay2);
            
            int winner = payoffMatrix[computerPlay][computerPlay2];
            if (winner == 1) {
                emit Winner(msg.sender, "You Win", computerPlay, computerPlay2); 
            } else if (winner == 2) {
                emit Winner (address(this), "You Lose", computerPlay, computerPlay2);
            }else
            {
                emit Winner (address(this), "Tie", computerPlay, computerPlay2);
            }
             
            return winner;
        }
        else 
            return -1;
    }
       
    
// HELPER FUNCTIONS (not required for game)

    function getMyBalance () public view returns (uint amount) {
        return msg.sender.balance;
    }
    
    function getContractBalance () public view returns (uint amount) {
        return address(this).balance;
    }
    
    function register()
        payable
        public 
        notRegisteredYet()
        returns (address player)
    {
        if (player1 == address(0))
            player1 = msg.sender;
        else if (player2 == address(0))
            player2 = msg.sender;
            
        emit LogPlay(player1, player2, player1Choice, player2Choice);
        return player1;
    }
    
    function AmIPlayer1() public view returns (bool x) {
        return msg.sender == player1;
    }
    
    function AmIPlayer2() public view returns (bool x) {
        return msg.sender == player2;
    }

    
    function checkBothNotNull() public view returns (bool x) {
        return (bytes(player1Choice).length == 0 && bytes(player2Choice).length == 0);
    }

}