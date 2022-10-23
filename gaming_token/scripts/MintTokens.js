const GamingToken = require("./GamingToken");

ethers.getContractAt(address, abi)
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

(async () => {
  await execute_txs();

  function sleep(ms) {
    return new Promise((resolve) => {
      setTimeout(resolve, ms);
    });
  }

  async function execute_txs() {
    try {
      await GamingToken.getTransactionCount().then(async (initialNonce) => {
        for (let idx = 0; idx < 100; idx++) {
          if (idx % 20 === 0) {
            await sleep(6000);
          }
          const uri = generateRandomTraits();

          const nonce = initialNonce + idx;
          GamingToken.mint(uri, nonce)
            .then((receipt) => {
              receipt.wait().then(async (resp) => {
                const { events } = resp;
                const transferEvent = events.filter(
                  (evt) => evt.event === "Transfer"
                )[0];
                const tokenId = transferEvent.args.tokenId;

                const tokenURI = await GamingToken.tokenURI(tokenId);
                console.log(
                  `Minted token: TokenId: #${tokenId}\nIdx: ${idx}\nData:${tokenURI}`
                );
              });
            })
            .catch((error) => {
              console.error("Connection lost ", error);
            });
        }
      });
    } catch (err) {
      console.log("connection lost!", err);
    }
  }
})();
