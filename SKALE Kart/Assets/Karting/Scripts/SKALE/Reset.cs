using System;
using System.Collections;
using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.Blocks;
using System.Text;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Nethereum.Util;


namespace KartGame.Track.SKALE
{
    public class Reset : MonoBehaviour
    {

        [Tooltip("SKALE Chain Endpoint.")]
        public string endpoint;
        [Tooltip("Ethereum wallet private key.")]
        public string privateKey;
        [Tooltip("Ethereun wallet account address.")]
        public string account;
        [Tooltip("Rinkeby ERC20 smart contract address.")]
        public string Rinkeby_ERC20;
        [Tooltip("SKALE ERC20 smart contract address.")]
        public string SKALE_ERC20;
        [Tooltip("SKALE IMA Token Manager smart contract address.")]
        public string SKALE_TokenManager;
        [Tooltip("SKALE Player Wallet.")]
        public string playerWallet;
        [Tooltip("SKALE Player Private Key.")]
        public string playerPrivateKey;

        // Start is called before the first frame update
        void Start()
        {

            endpoint = "http://sip2211-1.skalenodes.com:10035";
            privateKey = "A100C2FB3087F4E3945EAA312C65450ABED6ADF182399B3AE5CFBC99751030F3";
            account = "0x600F622CBd06cEd4D3ebC24fB97A69c62ae00Bb3";
            Rinkeby_ERC20 = "0x3a3b88c310E051cb61B15C4F430cD4ee5D7dCD2B";
            SKALE_ERC20 = "0x3081C6c5960f6a968Fdf13204CE6F6384cEa9000";
            SKALE_TokenManager = "0x831f4Bb2dcc565d93E19d7ec3Fc676eFD671550b";
            playerWallet = "0xB03076f5e382FC136E468bf36c945c7159Df0b24";
            playerPrivateKey = "759D2AD4D10FF2F082A6B244CAA9EBA771D70312205091C309AEECAFE1C3C25E";


        }

        public void ResetGame()
        {
            StartCoroutine(TransferTokensBack());
        }

        public IEnumerator TransferTokensBack()
        {

            //Query request using our acccount and the contracts address (no parameters needed and default values)
            var queryRequest = new QueryUnityRequest<ERC20.BalanceOfFunction, ERC20.BalanceOfOutputDTO>(endpoint, playerWallet);
            yield return queryRequest.Query(new ERC20.BalanceOfFunction() { Account = playerWallet }, SKALE_ERC20);

            //Getting the dto response already decoded
            var dtoResult = queryRequest.Result.ReturnValue1;
            Debug.Log(dtoResult);

            var transactionApproveRequest = new TransactionSignedUnityRequest(endpoint, playerPrivateKey);

            var transactionApproveMessage = new ERC20.ApproveFunction
            {
                Spender = account,
                Amount = UnitConversion.Convert.ToWei(1),
            };

            yield return transactionApproveRequest.SignAndSendTransaction(transactionApproveMessage, SKALE_ERC20);
            var transactionApproveHash = transactionApproveRequest.Result;

            Debug.Log(playerWallet);
            Debug.Log("Approve txn hash:" + transactionApproveHash);

            var transactionTransferRequest = new TransactionSignedUnityRequest(endpoint, playerPrivateKey);

            var transactionMessage = new ERC20.TransferFunction
            {
                Recipient = account,
                Amount = UnitConversion.Convert.ToWei(1),
            };

            yield return transactionTransferRequest.SignAndSendTransaction(transactionMessage, SKALE_ERC20);
            var transactionTransferHash = transactionTransferRequest.Result;

            Debug.Log(playerWallet);
            Debug.Log("Transfer txn hash:" + transactionTransferHash);

        }
    }
}

