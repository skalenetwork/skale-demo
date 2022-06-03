const contract = require("../abi/"+process.env.REACT_APP_ABI_NAME);
const ethers = require("ethers");
const endpoint = process.env.REACT_APP_ENDPOINT;
const pk = process.env.REACT_APP_PRIVATE_KEY;

const web3Provider = new ethers.providers.JsonRpcProvider(endpoint)
const signer = new ethers.Wallet(pk, web3Provider);

const APIURL = "http://127.0.0.1:8000/subgraphs/name/MooToken";

async function mint(owner, stroagePath) {
    let mooToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi);
    let balance = 5;
    if (!await addressHasSFUEL) await transfer_sFUEL(owner)
    console.log("request to mint to:", owner)
    const res = await mooToken.connect(signer).mint(owner, balance, stroagePath);
    console.log("token is minted");
    return res;
}

async function stake(owner, tokenId) {
    let mooToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi);
    if (!await addressHasSFUEL) await transfer_sFUEL(owner)
    const res = await mooToken.connect(signer).stake(tokenId);
    console.log("token is staked", await mooToken.connect(signer).getStake(owner, tokenId));
    console.log(res);
    return res;
}

async function unStake(owner, tokenId) {
    let mooToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi);
    if (!await addressHasSFUEL) await transfer_sFUEL(owner)
    const res = await mooToken.connect(signer).unStake(tokenId);
    console.log("token is unStaked", await mooToken.connect(signer).getStake(owner, tokenId));
    console.log(res);
    return res;
}

async function use(owner, tokenId) {
    let mooToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi);
    if (!await addressHasSFUEL) await transfer_sFUEL(owner)
    const res = await mooToken.connect(signer).use(tokenId);
    console.log("token is used");
    return res;
}

async function getBalance(owner, tokenId) {
    let mooToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi);
    return parseInt(await mooToken.connect(signer).getBalance(owner,tokenId));
}
async function getUsed(owner, tokenId) {
    let mooToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi);
    return parseInt(await mooToken.connect(signer).getUsed(owner,tokenId));
}


async function getCurrentTokenId() {
    let mooToken = new ethers.Contract(contract.erc721_address, contract.erc721_abi);
    return await mooToken.connect(signer).getCurrentTokenId();
}

async function addressHasSFUEL(address) {
    let addressBalance = await web3.eth.getBalance(address);
    if (addressBalance < 0.1) {
        console.log("Address Balance is low")
        return false;
    }
    return true;
}

async function getGraphQueryTokens(address)
{
    const tokensQuery = `
     query {
            myMooTokens (where: { from: "` + address + `", type:1})
              {
                  id,
                  from,
                  balance,
                  tokenURI,
                  used
                }
           }
`
    const client = createClient({
        url: APIURL
    });

    return await client.query(tokensQuery).toPromise();
}

 async function getGraphQueryTokensUsed(address)
{
    const tokensQuery = `
     query {
            myMooTokens (where: { from: "` + address + `", type:2})
              {
                  id,
                  from,
                  amount,
                  type,
                  tokenURI,
                  used
                }
           }
`
    const client = createClient({
        url: APIURL
    });

    return await client.query(tokensQuery).toPromise();
}

async function transfer_sFUEL(receiverAddress) {
    // Create a wallet instance
    console.log("amountInETH");
    let wallet = new ethers.Wallet(process.env.REACT_APP_PRIVATE_KEY, customHttpProvider)
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
    stake,
    unStake,
    getBalance,
    getUsed,
    use,
    getGraphQueryTokens,
    getGraphQueryTokensUsed,
    getCurrentTokenId
};
