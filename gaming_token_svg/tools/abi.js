
module.exports = {
    getAbi: (contractInterface) => {
        if (!contractInterface) {
            return undefined;
        }
        const abi = JSON.parse(contractInterface.format("json"));

        abi.forEach((obj) => {
            if (obj.type === "function") {
                const func = obj 
                func.inputs.concat(func.outputs).forEach((output) => {
                    Object.assign(output, Object.assign({name: ""}, output));
                })
            }
        });

        return abi;
    }
}