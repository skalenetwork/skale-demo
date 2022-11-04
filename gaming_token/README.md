# Intented Usage 
This project is intended for the Hackathon ETHSF. You can go here for the details https://hackathon.skale.network/
If you want to have access to this project outside of the hackathon you can reach out to the Skale team via Discord.

# Project setup

## Requirements
Make sure that you are using the node version `v14.17.0`.

## Clone the skale-demo repository
To run this example project, execute these commands 
```
git clone https://github.com/skalenetwork/skale-demo.git
```
must checkout the branch name `gaming_token`
```
git checkout gaming_token
```
and the example is located under `gaming_token` directory
```
cd gaming_token
```

## Install the NPM packages
Run the `npm install`.

## Setting of the env variables
Please contact Skale for the values of the env variables (except for contract address)
create a new `.env` file 
which contains 
```
ENDPOINT=http://exampleendpoint
PRIVATE_KEY=exampleprivatekey
CONTRACT_ADDRESS=examplecontractaddress
ABI_NAME=GamingToken-WithAddress.json
```

## Creating the private key
To create a new private key for the project, run the command
```
npm run createWallet
```
copy the private key from the output to the `.env` file as `PRIVATE_KEY`.

### Get sFUEL in a faucet.

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
Replace the `CONTRACT_ADDRESS` in the .env with the address from the output of the command

## Minting NFTs
To mint the NFTs run
```
npm run mintTokens
```
Expect this script to take ~30 seconds 
Once done you can check the block explorer to see the NFTs minted .

The NFT gaming token attributes are being generated randomly via the `scripts/MintTokens.js` script

