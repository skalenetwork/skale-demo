const { getAllJSDocTagsOfKind } = require("typescript");
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


      for (let idx = 0; idx < 100; idx++) {
        if (idx % 20 === 0 && idx > 0) {
          console.log("Sent", idx, "txs");
          await sleep(2000);
        }
        const uri = generateRandomTraits();

        const nonce = initialNonce + idx;
        GamingToken.mint(uri, nonce);
      }
      console.log("100 txs sent to the SKALE chain!");
      console.log("Let's go to block explorer to see them");
    } catch (err) {
      console.log("Looks like something went wrong!", err);
    }
  }
})();
