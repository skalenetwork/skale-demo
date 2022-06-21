require('dotenv').config()
const mooToken = require('./MooToken');
const fs = require("fs");
const path = require("path");

const skaleFileDirectory= process.env.REACT_NFT_CONTRACT_ADDRESS;
const nftFileName = "Joey.svg";
const fsEndpoint = process.env.REACT_APP_FS_ENDPOINT;
const addressMinter = process.env.REACT_APP_ACCOUNT;
const nftMetadataFileName = "JoeyMetadata.json";
const localFileDirectory = "./assets/";
const uploadFile = require('./FileStorage');
let generatedImageDirectory = fsEndpoint + "/" + uploadFile.stripHexPrefix(addressMinter) + "/" + skaleFileDirectory + "/" ;
const nftModification = require('./NFTModification');

(async () => {
        // for(let tokenId=1; tokenId<1000; tokenId++ ) {
        //         // walletIds += await mooToken.generateNew();
        // }
        // nftModification.createNewWallets(walletIds);


        for(let tokenId=33; tokenId<100; tokenId++ ) {
                console.log("--------Changing SVG colors--------")
                let svgNewFileName = await nftModification.changeSvgColor(nftFileName);

                console.log("--------Uploading new nft to FileStorage--------");
                 uploadFile.uploadFile(skaleFileDirectory, localFileDirectory, svgNewFileName);
                console.log(generatedImageDirectory + svgNewFileName);

                console.log("--------Uploading new nft metadata to FileStorage--------", await mooToken.getCurrentTokenId());
                const uploadedMDFileName = tokenId + ".json";

                let jsonWithImageURL = await nftModification.changeImageURL(nftMetadataFileName, generatedImageDirectory + svgNewFileName);
                 uploadFile.uploadJson(skaleFileDirectory, uploadedMDFileName, jsonWithImageURL);
                console.log(generatedImageDirectory + uploadedMDFileName);

        }


        // let file = fs.readFileSync(
        //     path.resolve(__dirname,  "./assets/wallets.csv"),
        //     "utf8"
        // );
        //
        // let res = {};
        // res.wallets = file
        //     .split(',').map(e => ({
        //             "wallet": e.split('\n')[0],
        //             "pk": e.split('\n')[1]
        //     }));
        // try {
        // let counter = 1;
        // for(let wlt of res.wallets) {
        //         if (wlt.wallet !== undefined && wlt.pk !== undefined && counter<1000) {
        //                 console.log(wlt.wallet)
        //                 console.log(wlt.pk)
        //                 let jsonNo = counter % 100
        //                 if (jsonNo === 0) jsonNo = 1;
        //                 try {
        //                         mooToken.mintWithGeneratedWallets(wlt.wallet, wlt.pk,  "905173B6C0A51925d3C9B619466c623c754Fb7BB/0x780fEc8d3C552653b8B0c0f875aBC99A6BA8FC25/" + counter % 100 + ".json").then((quote) => {
        //                                 // console.log(quote);
        //                         }).catch((error) => {
        //                                 console.error("Connection lost trying again");
        //                         });
        //
        //                         if (counter % 100 === 0) {
        //                                 await sleep(6000)
        //                         }
        //                 }
        //                 catch (err)
        //                 {
        //                    console.log("connection lost!")
        //                 }
        //         }
        //         //
        //         counter++;
        // }
        // }
        // catch (err)
        // {
        //         console.log("connection lost!")
        //         // sleep(6000)
        // }
        //
        // function sleep(ms) {
        //         return new Promise((resolve) => {
        //                 setTimeout(resolve, ms);
        //         });
        // }


        // let counter=1;
        // fs.readFileSync(path.resolve(__dirname, "./assets/wallets.csv"))
        //     .on("data", function (row) {
        //             console.log(row[0], row[1]);
        //             // mooToken.mintWithGeneratedWallets(row[0], row[1],skaleFileDirectory + "/" + counter+ ".json")
        //             counter++
        //     }).on("end", function () {
        //         console.log("finished");
        // })
        //     .on("error", function (error) {
        //             console.log(error.message);
        //     });

        // await mooToken.mint(addressMinter, skaleFileDirectory + "/"+ "1.json")
        // console.log("--------Changing SVG colors--------")
        // let svgNewFileName = await nftModification.changeSvgColor(nftFileName);

        // console.log("--------Uploading new nft to FileStorage--------");
        // await uploadFile.uploadFile(skaleFileDirectory, localFileDirectory, svgNewFileName);
        // console.log(generatedImageDirectory + svgNewFileName);
        //
        // console.log("--------Uploading new nft metadata to FileStorage--------", await tokenId);
        // // let tokenId = await mooToken.getCurrentTokenId() + 1;
        // const uploadedMDFileName = tokenId + ".json";
        //
        // let jsonWithImageURL = await nftModification.changeImageURL(nftMetadataFileName, generatedImageDirectory + svgNewFileName);
        // await uploadFile.uploadJson(skaleFileDirectory, uploadedMDFileName, jsonWithImageURL);
        // console.log(generatedImageDirectory + uploadedMDFileName);

        // console.log("--------Mint an NFT with tokenId-------- ");
        //
        // // generate 100 wallets and execute all at once.


        // console.log("--------Stake an NFT with tokenId-------- ", tokenId);
        // await mooToken.stake(addressMinter, tokenId);
        // console.log("--------UnStake an NFT with tokenId-------- ", tokenId);
        // await mooToken.unStake(addressMinter, tokenId);
        // console.log("--------Use an NFT with tokenId-------- ");
        // await mooToken.use(addressMinter, tokenId);
})();

