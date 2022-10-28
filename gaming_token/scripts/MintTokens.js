const GamingToken = require("./GamingToken");

const traits = {
  hairColor: ["red", "blue", "green"],
  stamina: ["123", "234", "345"],
  runningSpeed: ["35", "55", "67"],
  strength: ["5", "6", "8"],
  power: ["10", "9", "8"],
};

const generateRandomTraits = () => {
  const newTraits = {};

  Object.entries(traits).forEach((trait) => {
    const traitName = trait[0];
    const traitValues = trait[1];

    const num = Math.random();
    const valueIdx = parseInt(traitValues.length * num);

    newTraits[traitName] = traitValues[valueIdx];
  });

  return JSON.stringify(newTraits);
};
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

      console.log({initialNonce});

      for (let idx = 0; idx < 100; idx++) {
        if (idx % 20 === 0) {
          await sleep(6000);
        }
        const uri = generateRandomTraits();

        const nonce = initialNonce + idx;
        const mintReceipt = await GamingToken.mint(uri, nonce);
        const mintResponse = await mintReceipt.wait();
        const { events } = mintResponse;
        const transferEvent = events.filter(
          (evt) => evt.event === "Transfer"
        )[0];

        const tokenId = transferEvent.args.tokenId;

        const tokenURI = await GamingToken.tokenURI(tokenId);
        console.log(
          `Minted token: TokenId: #${tokenId}\nIdx: ${idx}\nData:${tokenURI}`
        );
      }
    } catch (err) {
      console.log("Looks like something went wrong!", err);
    }
  }
})();
