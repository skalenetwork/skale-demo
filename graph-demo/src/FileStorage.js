require('dotenv').config()
const Filestorage = require('@skalenetwork/filestorage.js');
const Web3 = require('web3');
const fs = require("fs");
const path = require("path");
const endpoint = process.env.REACT_APP_ENDPOINT;
const addressMinter = process.env.REACT_APP_ACCOUNT;
const pk = process.env.REACT_APP_PRIVATE_KEY;
const web3Provider = new Web3.providers.HttpProvider(endpoint);

let fileStorageMinter = new Filestorage(web3Provider, true);
let web3Minter = new Web3(web3Provider);


const uploadJson = async (directory, fileName, jsonMetadata) => {
    await createDirectory(directory);
    let myBuffer = [];
    let buffer = new Buffer(jsonMetadata, 'utf-8');
    for (let i = 0; i < buffer.length; i++) {
        myBuffer.push(buffer[i]);
    }
    return await createFile(directory, fileName, myBuffer);
};

const uploadFile = async (directory, localDirectory ,fileName) => {
    await createDirectory(directory);
    let bytes = fs.readFileSync(path.resolve(__dirname, `${localDirectory}${fileName}`));
    return await createFile(directory, fileName, new Uint8Array(bytes.buffer, bytes.byteOffset, bytes.length));
};


async function createFile(directory, fileName, byteArray) {
    let storagePath = await web3Minter.utils.stripHexPrefix(addressMinter).toLowerCase();
    let nftStoragePath = `${directory}/${fileName}`;

    if (!await fileExist(storagePath, directory, fileName)) {
        console.log("Uploading to : ", nftStoragePath);
        return fileStorageMinter.uploadFile(
            addressMinter,
            nftStoragePath,
            byteArray,
            pk
        );

    } else {
        console.log("file already exists!");
        return `${storagePath}/${directory}/${fileName}`
    }
}

const createDirectory = async (directory) => {
    let storagePath = await web3Minter.utils.stripHexPrefix(addressMinter).toLowerCase();
    if (!await directoryExist(directory, storagePath)) {
        console.log("Directory doesn't exist, Creating");
        await fileStorageMinter.createDirectory(addressMinter, directory, pk);
    }
}

let fileExist = async function (storagePath, directory, fileName) {
    let nftFiles = await fileStorageMinter.listDirectory(`${storagePath}/${directory}`);
    if (nftFiles.find(o => o.name.toString() === fileName.toString())) {
        console.log('NFT already exist in file storage, Change NFT File Name!')
        return true;
    }
    console.log('NFT doesn\'t Exist in File Storage!')
    return false;
}

let directoryExist = async function (directory, storagePath) {
    let files = await fileStorageMinter.listDirectory(storagePath);
    let findDirectory = files.filter(function (file) {
        let reg = new RegExp(`(${directory})`, 'i');
        return !!file.name.match(reg);
    });
    return findDirectory.length > 0;
};

let stripHexPrefix = function (addressMinter) {
    return web3Minter.utils.stripHexPrefix(addressMinter).toLowerCase();
}

const reserveSpace = async (reserveToAddress) => {
    console.log("reserving space");
    const reservedSpace = 3 * 10 ** 8;
    await fileStorageMinter.grantAllocatorRole(addressMinter, reserveToAddress, pk);
    await fileStorageMinter.reserveSpace(addressMinter, reserveToAddress, reservedSpace, pk);
    console.log("reserved space for:" + reserveToAddress);
}

module.exports = {
    uploadFile,
    uploadJson,
    reserveSpace,
    stripHexPrefix
};
