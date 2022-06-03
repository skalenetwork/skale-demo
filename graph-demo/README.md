# CHECK NODE VERSION
1. `node -v` make sure Use node.js ver16

## Update .env-sample

1. change name of `.env-sample` -> `.env`

2. REACT_APP_PRIVATE_KEY= {pK} with your {privateKey}
    ex. `REACT_APP_PRIVATE_KEY= djijd29929jd29deidffjaksed3qskfj33949j9dj`

### Compile & Deploy

1. `yarn && yarn compile && yarn deploy`
2.  COPY outputted contract address
    ex. `0xB2f089d6dC315E40B2bf61ed68D0a6BD5b23c2ef`

#### Update .env & subgraph.yaml with step 2 from Compile & Deploy 

1. UPDATE <addContractAddress> in .env
    ex. `REACT_NFT_CONTRACT_ADDRESS=0xB2f089d6dC315E40B2bf61ed68D0a6BD5b23c2ef`

2. UPDATE subgraph.yaml

    dataSources
        kind:
        name:
        network: skale     
        source:
            address: <addContractAddress>
            ...

##### Create and Deploy to Graph

RUN `yarn && yarn codegen && yarn remove-local && yarn create-local && yarn deploy-local`





###### FINALLY RUN THIS to MINT!

`node src/MooTokenGenerate.js`
