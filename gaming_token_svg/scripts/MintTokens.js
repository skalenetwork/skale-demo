const GamingToken = require("./GamingToken");
const fs = require('fs');

const sleep = (ms) => {
  return new Promise((resolve) => {
    setTimeout(resolve, ms);
  });
}

(async () => {
  await mintTokens();

  async function mintTokens() {
    try {
      //Initialize the HTML boilerplate
      let fileContent = '<html><head></head><body>';
      
      const initialNonce = await GamingToken.getTransactionCount();


      for (let idx = 0; idx < 2; idx++) {
        if (idx % 20 === 0 && idx > 0) {
          console.log("Sent", idx, "txs");
          await sleep(2000);
        }
      
        const nonce = initialNonce + idx;
        const receipt = GamingToken.mint(nonce);
        const response = await receipt;
        const details = await response.wait();
        const evt = details.logs[0];
        const tokenId = evt.topics[3];
        const tokenURI = await GamingToken.tokenURI(tokenId);

        //Create an image tag
        const image = `<h2>Token Id: ${tokenId}</h2><img src="${tokenURI}" />\n`;
        //Append image to file content
        fileContent += image;

      }
      console.log("100 txs sent to the SKALE chain!");
      console.log("Let's go to block explorer to see them");
      //Append the </body> and </html> tags
      fileContent += '</body></html>';
      
      //Write file contents to the file
      fs.writeFile('result.html', fileContent, (err) => { if (err) throw err; })
      
      console.log('The resulting SVGs are to be viewed in `result.html`');
    } catch (err) {
      console.log("Looks like something went wrong!", err);
    }
  }
})();
