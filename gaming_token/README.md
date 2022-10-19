# Project setup

## Clone the skale-demo repository
To run this example project, execute these commands 
```
git clone https://github.com/skalenetwork/skale-demo.git
```
must checkout the branch name gaming_token
```
git checkout gaming_token
```
and the example is located under gaming_token directory
```
cd gaming_token
```
## Setting of the env variables
Please contact Skale for the values of the env variables (except for contract address)
create a new .env file 
which contains 
```
ENDPOINT=http://exampleendpoint
PRIVATE_KEY=exampleprivatekey
CONTRACT_ADDRESS=examplecontractaddress
ABI_NAME=GamingToken-WithAddress.json
```
## Compile and deploy contract
To compile the contract run 
```
npm run compile 
```
To deploy this contract run
```
npm run deploy
```
After running this command you will see an output like this example
```
ERC721 Token GamingToken was deployed
Address: 0x5bBf089e42af7711cbF299BcE945B6A0801c864f
```
Replace the CONTRACT_ADDRESS in the .env with the address from the output of the command

## Minting NFTs
To mint the NFTs run
```
node src/MintTokens.js
```
Expect this script to take ~30 seconds 
Once done you can check the block explorer to see the NFTs minted 

The NFT gaming token attributes are being generated randomly via the src/MintTokens.js script

