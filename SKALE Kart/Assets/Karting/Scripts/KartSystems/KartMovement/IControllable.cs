using UnityEngine;

namespace KartGame.KartSystems
{
    /// <summary>
    /// Should be implemented by things that are controlled by user input when that control can be turned on or off.
    /// </summary>
    public interface IControllable
    {
        /// <summary>
        /// Should be used to allow user input control.
        /// </summary>
        void EnableControl ();

        /// <summary>
        /// Should be used to stop user input control.
        /// </summary>
        void DisableControl ();

        /// <summary>
        /// Should be used to check whether the user currently has control.
        /// </summary>
        /// <returns>Whether or not the user currently has control.</returns>
        bool IsControlled ();
    }
}