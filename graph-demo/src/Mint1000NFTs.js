require('dotenv').config()
const mooToken = require('./MooToken');
const fs = require("fs");
const path = require("path");

(async () => {
        // for(let tokenId=1; tokenId<1000; tokenId++ ) {
        //         // walletIds += await mooToken.generateNew();
        // }
        // nftModification.createNewWallets(walletIds);

        let file = fs.readFileSync(
            path.resolve(__dirname,  "./assets/wallets.csv"),
            "utf8"
        );

        let res = {};
        res.wallets = file
            .split(',').map(e => ({
                    "wallet": e.split('\n')[0],
                    "pk": e.split('\n')[1]
            }));
        try {
        let counter = 1;
        for(let wlt of res.wallets) {
                if (wlt.wallet !== undefined && wlt.pk !== undefined && counter<1000) {
                        console.log(wlt.wallet)
                        console.log(wlt.pk)
                        let jsonNo = counter % 100
                        if (jsonNo === 0) jsonNo = 1;
                        try {
                                mooToken.mintWithGeneratedWallets(wlt.wallet, wlt.pk,  "905173b6c0a51925d3c9b619466c623c754fb7bb/0xE1E4905E8Faa4B2Cdd0A241B952C4615A42a176a/" + counter % 100 + ".json").then((quote) => {
                                        // console.log(quote);
                                }).catch((error) => {
                                        console.error("Connection lost trying again");
                                });

                                if (counter % 100 === 0) {
                                        await sleep(6000)
                                }
                        }
                        catch (err)
                        {
                           console.log("connection lost!")
                        }
                }
                //
                counter++;
        }
        }
        catch (err)
        {
                console.log("connection lost!")
                // sleep(6000)
        }

        function sleep(ms) {
                return new Promise((resolve) => {
                        setTimeout(resolve, ms);
                });
        }


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

