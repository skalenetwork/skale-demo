

[![Average time to resolve an issue](http://isitmaintained.com/badge/resolution/skalenetwork/skale-demo.svg)](http://isitmaintained.com/project/skalenetwork/skale-demo "Average time to resolve an issue")
[![Percentage of issues still open](http://isitmaintained.com/badge/open/skalenetwork/skale-demo.svg)](http://isitmaintained.com/project/skalenetwork/skale-demo "Percentage of issues still open")

# SKALE: Rock Paper Scissors dApp
### Table of Contents

<!-- MarkdownTOC -->

- [Introduction](#introduction)
- [Installation](#installation)
    - [Requirements](#requirements)
    - [Run App Locally](#run)
- [Troubleshooting](#troubleshooting)

<!-- /MarkdownTOC -->

<a name="introduction"></a>
# Introduction

Demo dApp designed as a proof of concept for deploying simple smart contracts to SKALE chains. This dApp was developed by SKALE Labs, with the intent of being a reference for other dApp developers looking to integrate with the SKALE network.

<a name="installation"></a>
# Installation

<a name="requirements"></a>
### Requirements

+ [nodeJS](https://nodejs.org/en/download/)
+ [Truffle](https://truffleframework.com/)
+ [SKALE Chain](https://discord.gg/vvUtWJB)
+ [MetaMask](https://metamask.io/)

<a name="run"></a>
### Run App Locally

Open a terminal, and...

1. Make sure **[nodeJS 8.11.3](https://nodejs.org/en/download/ "nodeJS 8.11.3")** is installed and running
2. Make sure [SKALE Chain](https://discord.gg/vvUtWJB) is running
3. Make sure [Truffle](https://truffleframework.com/) is installed 
4. Make sure [MetaMask](https://metamask.io/) is installed in your browser
5. **Clone this project to your desktop**
 
    ``` 
    git clone --recursive https://github.com/skalenetwork/skale-demo.git
    cd skale-demo
    ```
    
+ Install node packages

    ```
    npm install
    ```  
    
+ Modify the `truffle.js` file to point to add in your MetaMask mnemonic, and the IP address of your SKALE Chain


+ Compile and Migrate Contracts to your SKALE Chain

    ```
    truffle deploy --reset --network skale --compile-all
    ```   
    
+ Run the app locally

    ```
    npm start
    ```
    

<a name="troubleshooting"></a>
# Troubleshooting



