require('dotenv').config()
const SimpleMTM = require('./SimpleMTMToken');

(async () => {
        await execute_txs();

        function sleep(ms) {
                return new Promise((resolve) => {
                        setTimeout(resolve, ms);
                });
        }

        async function execute_txs() {
            SimpleMTM.mtm_query(0, 0).then((quote) => {
            }).catch((error) => {
                console.error("Connection lost ", error);
            });
        }
})();

