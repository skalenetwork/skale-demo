

# SKALE Rock Paper Scissors Demo

# Introduction

Demo dApp to use during the ETH Denver hackathon.


### Run App Locally

> Compatible with [MetaMask 6.1.0](https://github.com/MetaMask/metamask-extension/releases/tag/v6.1.0) or lower

+ Update `.env` file with your credentials.

```
ACCOUNT=[YOUR_ACCOUNT]
PRIVATE_KEY=[YOUR_PRIVATE_KEY]
SKALE_CHAIN=[YOUR_SKALE_CHAIN_ENDPOINT]
```

+ Install node packages

```
$ npm install
```

+ Deploy smart contracts

```
$ truffle migrate --network skale
```

+ Run the app locally

```
$ npm start
```

Navigate to http://localhost:3000/ to check out your dApp!



