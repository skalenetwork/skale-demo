var express = require('express');
var bodyParser = require('body-parser');
var morgan = require('morgan');
var faucetRouter = require('./routers/faucet-router');

var app = express();

app.use(morgan('dev'));
app.use(express.static('client'));

app.use((req, res, next) => {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept,recording-session");
    next();
});

app.use(bodyParser.urlencoded({extended: true}));
app.use(bodyParser.json());

app.use('/faucet', faucetRouter);

app.listen(6666, () => console.log("Listening on port 6666!"));