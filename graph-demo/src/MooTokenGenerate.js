require('dotenv').config()
const skaleFileDirectory= process.env.REACT_NFT_CONTRACT_ADDRESS;
const nftFileName = "Joey.svg";
const fsEndpoint = process.env.REACT_APP_FS_ENDPOINT;
const addressMinter = process.env.REACT_APP_ACCOUNT;
const nftMetadataFileName = "JoeyMetadata.json";
const nftModification = require('./NFTModification');
const mooToken = require('./MooToken');
const localFileDirectory = "./assets/";
const uploadFile = require('./FileStorage');
let generatedImageDirectory = fsEndpoint + "/" + uploadFile.stripHexPrefix(addressMinter) + "/" + skaleFileDirectory + "/" ;

(async () => {

    console.log("--------Changing SVG colors--------")
    let svgNewFileName = await nftModification.changeSvgColor(nftFileName);

    console.log("--------Uploading new nft to FileStorage--------");
    await uploadFile.uploadFile(skaleFileDirectory, localFileDirectory, svgNewFileName);
    console.log(generatedImageDirectory + svgNewFileName);

    console.log("--------Uploading new nft metadata to FileStorage--------", await mooToken.getCurrentTokenId());
    let tokenId = await mooToken.getCurrentTokenId() + 1;
    const uploadedMDFileName = tokenId + ".json";

    let jsonWithImageURL = await nftModification.changeImageURL(nftMetadataFileName, generatedImageDirectory + svgNewFileName);
    await uploadFile.uploadJson(skaleFileDirectory, uploadedMDFileName, jsonWithImageURL);
    console.log(generatedImageDirectory + uploadedMDFileName);

    console.log("--------Mint an NFT with tokenId-------- ");

    // generate 100 wallets and execute all at once.
    await mooToken.mint(addressMinter, skaleFileDirectory + "/"+ uploadedMDFileName)

    console.log("--------Stake an NFT with tokenId-------- ",tokenId);
    await mooToken.stake(addressMinter,tokenId);
    console.log("--------UnStake an NFT with tokenId-------- ",tokenId);
    await mooToken.unStake(addressMinter,tokenId);
    console.log("--------Use an NFT with tokenId-------- ");
    await mooToken.use(addressMinter,tokenId);
})();

