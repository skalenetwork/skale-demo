import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import {
    Card,
    CardActionArea,
    CardActions,
    CardContent,
    CardMedia,
    Typography,
    Button
} from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
    root: {
        minWidth: 345,
        minHeight: 345,
        maxWidth: 345,
        marginBottom: 10,
        marginRight: 10
    },
    media: {
        height: 140
    }
}));

export default function NFTCard({ itemID, itemURL, itemName, itemDesc }) {
    const classes = useStyles();

    const truncText = itemDesc;

    return (
        <Card className={classes.root}>
            <CardActionArea>
                <CardMedia
                    className={classes.media}
                    image={itemURL}
                    title="Contemplative Reptile"
                />
                <CardContent>
                    <Typography gutterBottom variant="h5" component="h2">
                        {itemName}
                    </Typography>
                    <Typography variant="body2" color="textSecondary" component="p">
                        {truncText}
                    </Typography>
                </CardContent>
            </CardActionArea>
            <CardActions>
                <Button size="small" color="primary">
                    Share
                </Button>
                <Button size="small" color="primary">
                    Learn More
                </Button>
            </CardActions>
        </Card>
    );
}
