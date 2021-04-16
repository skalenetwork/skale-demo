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
    public class Coin : MonoBehaviour
    {

        [Tooltip("SKALE Chain Endpoint.")]
        public string endpoint;
        [Tooltip("Ethereum wallet private key.")]
        public string privateKey;
        [Tooltip("Ethereun wallet account address.")]
        public string account;
        [Tooltip("SKALE ERC20 smart contract address.")]
        public string SKALE_ERC20;
        [Tooltip("SKALE IMA LockAndDataSchain smart contract address.")]
        public string SKALE_LnD;
        [Tooltip("SKALE Chain Endpoint.")]
        public string playerWallet;

        // Start is called before the first frame update
        void Start()
        {
            
            endpoint = "http://sip2211-1.skalenodes.com:10035";
            privateKey = "A100C2FB3087F4E3945EAA312C65450ABED6ADF182399B3AE5CFBC99751030F3";
            account = "0x600F622CBd06cEd4D3ebC24fB97A69c62ae00Bb3";
            SKALE_ERC20 = "0x3081C6c5960f6a968Fdf13204CE6F6384cEa9000";
            SKALE_LnD = "0xc4345Ea69018c9E6dc829DF362C8A9aa18b9e39e";
            playerWallet = "0xB03076f5e382FC136E468bf36c945c7159Df0b24";
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.activeSelf && col.gameObject.name.Contains("Coin"))
            {
                Destroy(col.gameObject);
                StartCoroutine(TransferTokens());
            }
        }

        public IEnumerator TransferTokens()
        {

            var transactionTransferRequest = new TransactionSignedUnityRequest(endpoint, privateKey);

            var transactionMessage = new ERC20.TransferFunction
            {
                Recipient = playerWallet,
                Amount = UnitConversion.Convert.ToWei(1),
            };

            yield return transactionTransferRequest.SignAndSendTransaction(transactionMessage, SKALE_ERC20);
            var transactionTransferHash = transactionTransferRequest.Result;

            Debug.Log(playerWallet);
            Debug.Log("Transfer txn hash:" + transactionTransferHash);


        }
    }
}

