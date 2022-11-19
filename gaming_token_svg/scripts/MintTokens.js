const GamingToken = require("./GamingToken");

const sleep = (ms) => {
  return new Promise((resolve) => {
    setTimeout(resolve, ms);
  });
}

(async () => {
  await mintTokens();

  async function mintTokens() {
    try {
      const initialNonce = await GamingToken.getTransactionCount();


      for (let idx = 0; idx < 2; idx++) {
        if (idx % 20 === 0 && idx > 0) {
          console.log("Sent", idx, "txs");
          await sleep(2000);
        }
      
        const nonce = initialNonce + idx;
        GamingToken.mint(nonce);
      }
      console.log("100 txs sent to the SKALE chain!");
      console.log("Let's go to block explorer to see them");
    } catch (err) {
      console.log("Looks like something went wrong!", err);
    }
  }
})();
