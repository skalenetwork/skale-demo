require('dotenv').config()
const Shack15Token = require('./Shack15Token');

(async () => {
       await Shack15Token.mint("0xb713eAfca2f6984b3e7AdBc0531b1d37403812a8",5,1
,"https://testnet-proxy.skalenodes.com/fs/whispering-turais/905173b6c0a51925d3c9b619466c623c754fb7bb/EbruFolder/norway10.jpeg"
            ).then((quote) => {
                console.log("tx response", quote);
        }).catch((error) => {
                console.error("Connection lost ", error);
        });

})();

