# mtm-skale
StrangeToken generates an NFT with an on-chain dynamically created SVG. 
It uses SKALE solidity random number generator to generate different size circles and different color circles in different positions. 
Every NFT that is minted are unique. No content file storage needed. 

Multi transaction Mode on SKALE

`npm install`

change .env variables with private key and chain endpoint 


execute this code to min 100NFTs in couple seconds with MTM. 
To be able to run this code on SKALE chain SKALE chain MTM mode should be enabled 
`node src/EthersMint100NFTs.js`

Here is an example verified contract on block explorer: 
https://roasted-thankful-unukalhai.explorer.staging-v2.skalenodes.com/address/0x4bbEeaB462f342365d7CeB78e1A9003A9Ef5a4f3/read-contract

If you would like to see the example nfs execute the index.html with different tokenIds to see the minted NFTs on chain 
