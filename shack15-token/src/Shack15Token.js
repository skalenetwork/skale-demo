const contract = require("../abi/"+process.env.REACT_APP_ABI_NAME);
const ethers = require("ethers");
const endpoint = process.env.REACT_APP_ENDPOINT;
const pk = process.env.REACT_APP_PRIVATE_KEY;
const account = process.env.REACT_APP_ACCOUNT;

const web3Provider = new ethers.providers.JsonRpcProvider(endpoint)
const signer = new ethers.Wallet(pk, web3Provider);


async function mint(address, amount, type, tokenUri) {
    let data = "0x";
    let shack15nfttoken = new ethers.Contract(contract.erc1155_address, contract.erc1155_abi);
    console.log("type of the token: " + type)
    let typeOfToken = "";
    if (type === 1) {
        typeOfToken = "PAYITFORWARD";
    } else if (type === 2) {
        typeOfToken = "USABLE";
    } else {
        throw new Error("Invalid type of token");
    }

    data = ethers.utils.keccak256(ethers.utils.toUtf8Bytes(typeOfToken));

    if(await addressHasSFUEL(address)) {
        await transfer_sFUEL(address)
    };

    const res = await (await shack15nfttoken.connect(signer).mint(address, amount, data, tokenUri)).wait();
    console.log("token is minted");
    return res;
}



async function getCurrentTokenId() {
    let StrangeToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi);
    return parseInt(await StrangeToken.connect(signer).getCurrentTokenId());
}

async function getTransactionCount() {
    let tx = await web3Provider.getTransactionCount(account);
    let pending = await web3Provider.getTransactionCount(account, "pending");
    if (tx > pending) {
        return tx;
    } else {
        return pending
    }
    ;
}

async function addressHasSFUEL(address) {
    web3Provider.getBalance(address).then((balance) => {
        // convert a currency unit from wei to ether
        const balanceInEth = ethers.utils.formatEther(balance)
        console.log(`balance: ${balanceInEth} ETH`)

        if (balanceInEth < 0.1) {
            console.log("Address Balance is low")
            return false;
        }
    })
    return true;
}

async function transfer_sFUEL(receiverAddress) {
    // Create a wallet instance
    console.log("amountInETH");
    let wallet = new ethers.Wallet(process.env.REACT_APP_PRIVATE_KEY, web3Provider)
    // Ether amount to send
    let amountInEther = '0.01'
    // Create a transaction object
    let tx = {
        to: receiverAddress,
        // Convert currency unit from ether to wei
        value: ethers.utils.parseEther(amountInEther)
    }
    // Send a transaction
    await wallet.sendTransaction(tx)
        .then((txObj) => {
            console.log('txHash', txObj.hash)
        })
}


module.exports = {
    mint,
    getTransactionCount,
    getCurrentTokenId
};
