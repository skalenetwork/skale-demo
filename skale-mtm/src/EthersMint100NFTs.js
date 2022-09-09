require('dotenv').config()
const StrangeToken = require('./EthersMTMToken');

(async () => {
        await execute_txs();

        function sleep(ms) {
                return new Promise((resolve) => {
                        setTimeout(resolve, ms);
                });
        }

        async function execute_txs() {
                let nonce = await StrangeToken.getTransactionCount();
                let current_tokenId = await StrangeToken.getCurrentTokenId() + 1;

                for (let tokenId = 0; tokenId <= 100; tokenId++) {
                        console.log("nonce: ", nonce, " tokenId: ", current_tokenId);
                        if (tokenId % 20 === 0) {
                                await sleep(6000);
                        }
                        StrangeToken.mint(current_tokenId, nonce).then((quote) => {
                                console.log(quote["hash"]);
                        }).catch((error) => {
                                console.error("Connection lost ", error);
                        });
                        current_tokenId++
                        nonce++;
                }
        }
})();

