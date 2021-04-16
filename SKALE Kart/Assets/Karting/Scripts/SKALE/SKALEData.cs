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
    /// <summary>
    /// A class to display information about a particular racer's timings.  WARNING: This class uses strings and creates a small amount of garbage each frame.
    /// </summary>
    public class SKALEData : MonoBehaviour
    {

        /// <summary>
        /// The different information that can be displayed on screen.
        /// </summary>
        public enum DisplayOptions
        {
            /// <summary>
            /// Block Time.
            /// </summary>
            Block,
            /// <summary>
            /// ETH Balance of SKALE on SKALE Chain.
            /// </summary>
            Balance,
            /// <summary>
            /// ETH Balance of Player on SKALE Chain.
            /// </summary>
            BalancePlayer,
        }


        [Tooltip("The timings to be displayed and the order to display them.")]
        public List<DisplayOptions> initialDisplayOptions = new List<DisplayOptions>();
        [Tooltip("A reference to the track manager.")]
        public TrackManager trackManager;
        [Tooltip("A reference to the TextMeshProUGUI to display the information.")]
        public TextMeshProUGUI textComponent;
        [Tooltip("A reference to the racer to display the information for.")]
        [RequireInterface(typeof(IRacer))]
        public Object initialRacer;

        //Blockchain variables
        public string endpoint;
        public string privateKey;
        public string account;
        public string SKALE_ERC20;
        public string SKALE_LnD;
        public string playerWallet;

        //initial values
        private string blockNumberText;
        private string balanceText;
        private string balancePlayerText;

        List<Action> m_DisplayCalls = new List<Action>();
        IRacer m_Racer;
        StringBuilder m_StringBuilder = new StringBuilder(0, 300);


        void Start()
        {

            endpoint = "http://sip2211-1.skalenodes.com:10035";
            privateKey = "A100C2FB3087F4E3945EAA312C65450ABED6ADF182399B3AE5CFBC99751030F3";
            account = "0x600F622CBd06cEd4D3ebC24fB97A69c62ae00Bb3";
            SKALE_ERC20 = "0x3081C6c5960f6a968Fdf13204CE6F6384cEa9000";
            SKALE_LnD = "0xc4345Ea69018c9E6dc829DF362C8A9aa18b9e39e";
            playerWallet = "0xB03076f5e382FC136E468bf36c945c7159Df0b24";
            

            StartCoroutine(CheckBlockNumber());
            StartCoroutine(CheckBalance());
            StartCoroutine(CheckBalancePlayer());

        }


        void Awake()
        {
            for (int i = 0; i < initialDisplayOptions.Count; i++)
            {
                switch (initialDisplayOptions[i])
                {
                    case DisplayOptions.Block:
                        m_DisplayCalls.Add(DisplayBlock);
                        break;
                    case DisplayOptions.Balance:
                        m_DisplayCalls.Add(DisplayBalance);
                        break;
                    case DisplayOptions.BalancePlayer:
                        m_DisplayCalls.Add(DisplayBalancePlayer);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            m_Racer = (IRacer)initialRacer;
        }

        void Update()
        {
            m_StringBuilder.Clear();

            for (int i = 0; i < m_DisplayCalls.Count; i++)
            {
                m_DisplayCalls[i].Invoke();
            }

            textComponent.text = m_StringBuilder.ToString();

        }

        void DisplayBlock()
        {
            m_StringBuilder.AppendLine(blockNumberText);

        }

        void DisplayBalance()
        {
            m_StringBuilder.AppendLine(balanceText);

        }

        void DisplayBalancePlayer()
        {
            m_StringBuilder.AppendLine(balancePlayerText);

        }

        public IEnumerator CheckBlockNumber()
        {
            var wait = 1;
            while (true)
            {
                yield return new WaitForSeconds(wait);
                wait = 5;
                var blockNumberRequest = new EthBlockNumberUnityRequest(endpoint);
                yield return blockNumberRequest.SendRequest();
                if (blockNumberRequest.Exception == null)
                {
                    var blockNumber = blockNumberRequest.Result.Value;
                    blockNumberText = "Block: " + blockNumber.ToString();
                }
            }
        }

        public IEnumerator CheckBalance()
        {
            var wait = 1;
            while (true)
            {
                yield return new WaitForSeconds(wait);
                wait = 5;

                //Query request using our acccount and the contracts address (no parameters needed and default values)
                var queryRequest = new QueryUnityRequest<ERC20.BalanceOfFunction, ERC20.BalanceOfOutputDTO>(endpoint, account);
                yield return queryRequest.Query(new ERC20.BalanceOfFunction() { Account = account }, SKALE_ERC20);

                //Getting the dto response already decoded
                var dtoResult = queryRequest.Result;

                balanceText = "SKALE Balance: " + UnitConversion.Convert.FromWei(dtoResult.ReturnValue1);
            }
        }

        public IEnumerator CheckBalancePlayer()
        {
            var wait = 1;
            while (true)
            {
                yield return new WaitForSeconds(wait);
                wait = 5;

                //Query request using our acccount and the contracts address (no parameters needed and default values)
                var queryRequest = new QueryUnityRequest<ERC20.BalanceOfFunction, ERC20.BalanceOfOutputDTO>(endpoint, playerWallet);
                yield return queryRequest.Query(new ERC20.BalanceOfFunction() { Account = playerWallet }, SKALE_ERC20);

                //Getting the dto response already decoded
                var dtoResult = queryRequest.Result;

                balancePlayerText = "Player Balance: " + UnitConversion.Convert.FromWei(dtoResult.ReturnValue1);
            }
        }

        /// <summary>
        /// Call this function to change what information is currently being displayed.  This causes a GCAlloc.
        /// </summary>
        /// <param name="newDisplay">A collection of the new options for the display.</param>
        /// <exception cref="ArgumentOutOfRangeException">One or more of the display options is not valid.</exception>
        public void RebindDisplayOptions(List<DisplayOptions> newDisplay)
        {
            m_DisplayCalls.Clear();

            for (int i = 0; i < newDisplay.Count; i++)
            {
                switch (newDisplay[i])
                {
                    case DisplayOptions.Block:
                        m_DisplayCalls.Add(DisplayBlock);
                        break;
                    case DisplayOptions.Balance:
                        m_DisplayCalls.Add(DisplayBalance);
                        break;
                    case DisplayOptions.BalancePlayer:
                        m_DisplayCalls.Add(DisplayBalancePlayer);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        /// <summary>
        /// Call this function to change the racer about which the information is being displayed.
        /// </summary>
        public void RebindRacer(IRacer newRacer)
        {
            m_Racer = newRacer;
        }

    }
}