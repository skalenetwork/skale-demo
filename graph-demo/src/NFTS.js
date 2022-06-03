import React, { useState, useEffect } from "react";
import Card from "./Card";
import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1
    }
}));

function NFTS() {
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [items, setItems] = useState([]);

    //styles

    const classes = useStyles();

    // Note: the empty deps array [] means
    // this useEffect will run once
    // similar to componentDidMount()
    useEffect(() => {
        fetch("https://testnets-api.opensea.io/api/v1/assets")
            .then((res) => res.json())
            .then(
                (result) => {
                    console.log(result.assets);
                    setIsLoaded(true);
                    // const fetchedItems = result.assets;
                    setItems(result.assets);
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    setIsLoaded(true);
                    setError(error);
                }
            );
    }, []);

    if (error) {
        return <div>Error: {error.message}</div>;
    } else if (!isLoaded) {
        return <div>Loading...</div>;
    } else {
        return (
            <Grid container className={classes.root} spacing={2}>
                <Grid item xs={12}>
                    <Grid container justify="center">
                        {items.map((item) => (
                            <li key={item.id}>
                                <Card
                                    itemID={item.id}
                                    itemURL={item.image_url}
                                    itemName={item.name}
                                    itemDesc={item.description}
                                />
                            </li>
                        ))}
                    </Grid>
                </Grid>
            </Grid>
        );
    }
}

export { NFTS };

// const fetch = require("node-fetch");

// const url =
//   "https://api.opensea.io/api/v1/assets?order_direction=desc&offset=0&limit=20";
// const options = { method: "GET" };

// fetch(url, options)
//   .then((res) => res.json())
//   .then((json) => console.log(json))
//   .catch((err) => console.error("error:" + err));
