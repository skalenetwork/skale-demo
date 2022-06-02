const fs = require("fs");
const path = require("path");
const uuid = require("uuid");

const changeSvgColor = async (fileName) => {
    try {
        let svgCode = fs.readFileSync(
            path.resolve(__dirname, `./${fileName}`),
            "utf8"
        );
        let mySet = new Set()
        let counter = 0;
        let reg = /(fill:#)\w+/g
        let result;
        while ((result = reg.exec(svgCode)) !== null) {
            let newFill = "fill:" + setBg();
            if (!mySet.has(result[0])) {
                svgCode = svgCode.replace(result[0], newFill);
                mySet.add(result[0])
            }
            counter++
        }

        let newFileName = `${uuid.v4()}.svg`
        let pathFile = `./assets/${newFileName}`
        fs.writeFile(
            path.resolve(__dirname, pathFile),
            svgCode,
            function (err) {
                console.log(err);
            }
        );
        return newFileName;
    } catch (error) {
        console.log(error);
    }
};

const changeImageURL = (fileName, tokenImageURL) => {
    let check = false;
    try {
        let readJson = fs.readFileSync(
            path.resolve(__dirname, `./${fileName}`),
            "utf8"
        );
        let reg = /(<ImageURL_Here>)+/g
        let result;
        while ((result = reg.exec(readJson)) !== null) {
            check = true;
            readJson = readJson.replaceAll(result[0], tokenImageURL);
        }
        if (check) {
            return readJson;
        }
    } catch (error) {
        console.log(error);
    }

    throw "<ImageURL_Here> is not found in the json to Replace";

};


const setBg = () => {
    const randomColor = Math.floor(Math.random() * 16777215).toString(16);
    if (randomColor.length === 5) {
        return setBg();
    }
    return "#" + randomColor.toUpperCase();
}


module.exports = {
    changeSvgColor,
    changeImageURL
};
