npm install

.env variables
```
REACT_APP_ENDPOINT=https://testnet-proxy.skalenodes.com/v1/whispering-turais
REACT_APP_PRIVATE_KEY=
REACT_APP_ACCOUNT=
REACT_NFT_CONTRACT_ADDRESS=
REACT_APP_ABI_NAME=Shack15NFTToken-WithAddress.json
REACT_APP_THE_GRAPH_API=http://127.0.0.1:8000/subgraphs/name/Shack15NFTToken
REACT_APP_FS_ENDPOINT=https://testnet-proxy.skalenodes.com/fs/whispering-turais

```

node version change, first make sure to install the correct version 16 in your machine:
```
export NVM_DIR=~/.nvm                      
source $(brew --prefix nvm)/nvm.sh
nvm use 16```

```npm run compile```

```npx hardhat erc1155 --uri ERC1155TokenURI --network custom```

```node src/MintNFT.js ```
