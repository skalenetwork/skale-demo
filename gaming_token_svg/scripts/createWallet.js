const ethers = require("ethers");

const wallet = ethers.Wallet.createRandom();
console.log(`Created wallet with:\nAddress: ${wallet.address}\nPrivate key: ${wallet.privateKey}`);
console.log('Please, copy the private key above to the .env file under PRIVATE_KEY!');