require('dotenv').config()
const StrangeToken = require('./StrangeToken');

(async () => {
        let nonce = await StrangeToken.getTransactionCount();
        await execute_txs(nonce + 1);

        function sleep(ms) {
                return new Promise((resolve) => {
                        setTimeout(resolve, ms);
                });
        }

        async function execute_txs(startingTxCount) {
                try {
                        let nonce = await StrangeToken.getTransactionCount();
                        let tokenId = await startingTxCount - nonce;
                        for (tokenId <= 100; tokenId++;) {
                                console.log("nonce: ", nonce);
                                console.log("tokenId: ", tokenId);
                                console.log("startingTxCount: ",startingTxCount);
                                if (tokenId % 10 === 0) {
                                        await sleep(4000);
                                }
                                StrangeToken.mint(tokenId,nonce).then(r => {
                                        console.log(r)
                                });
                                nonce++;
                        }
                } catch (err) {
                        console.log("connection lost!")
                        await sleep(4000);
                        await execute_txs(startingTxCount);
                }
        }


        // let counter=1;
        // fs.readFileSync(path.resolve(__dirname, "./assets/wallets.csv"))
        //     .on("data", function (row) {
        //             console.log(row[0], row[1]);
        //             // StrangeToken.mintWithGeneratedWallets(row[0], row[1],skaleFileDirectory + "/" + counter+ ".json")
        //             counter++
        //     }).on("end", function () {
        //         console.log("finished");
        // })
        //     .on("error", function (error) {
        //             console.log(error.message);
        //     });

        // await StrangeToken.mint(addressMinter, skaleFileDirectory + "/"+ "1.json")
        // console.log("--------Changing SVG colors--------")
        // let svgNewFileName = await nftModification.changeSvgColor(nftFileName);

        // console.log("--------Uploading new nft to FileStorage--------");
        // await uploadFile.uploadFile(skaleFileDirectory, localFileDirectory, svgNewFileName);
        // console.log(generatedImageDirectory + svgNewFileName);
        //
        // console.log("--------Uploading new nft metadata to FileStorage--------", await tokenId);
        // // let tokenId = await StrangeToken.getCurrentTokenId() + 1;
        // const uploadedMDFileName = tokenId + ".json";
        //
        // let jsonWithImageURL = await nftModification.changeImageURL(nftMetadataFileName, generatedImageDirectory + svgNewFileName);
        // await uploadFile.uploadJson(skaleFileDirectory, uploadedMDFileName, jsonWithImageURL);
        // console.log(generatedImageDirectory + uploadedMDFileName);

        // console.log("--------Mint an NFT with tokenId-------- ");
        //
        // // generate 100 wallets and execute all at once.


        // console.log("--------Stake an NFT with tokenId-------- ", tokenId);
        // await StrangeToken.stake(addressMinter, tokenId);
        // console.log("--------UnStake an NFT with tokenId-------- ", tokenId);
        // await StrangeToken.unStake(addressMinter, tokenId);
        // console.log("--------Use an NFT with tokenId-------- ");
        // await StrangeToken.use(addressMinter, tokenId);
})();

