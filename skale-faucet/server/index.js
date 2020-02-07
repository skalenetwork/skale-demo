const express = require("express");
const os = require("os");
const app = express();
app.use(express.static("build_webpack"));
app.get("/api/getUsername", (req, res) =>
  res.send({ username: os.userInfo().username })
);

app.use('/static', express.static('build_webpack/static'));
app.use('/', express.static('build_webpack'));

app.get('/*', function(req, res) {
  res.sendFile('/var/www/faucet/skale-demo/skale-faucet/build_webpack/index.html', function(err) {
    if (err) {
      res.status(500).send(err)
    }
  })
})
app.listen(5555, () => console.log("Listening on port 5555!"));