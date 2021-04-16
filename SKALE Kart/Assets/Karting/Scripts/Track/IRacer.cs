using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;

namespace KartGame.Track
{
    /// <summary>
    /// Should be implemented by things that take part in a race.  Allows for data to be collected about the timings of the race.  Default implementation is the Racer MonoBehaviour.
    /// </summary>
    public interface IRacer : IControllable
    {
        /// <summary>
        /// Stop the race timer from counting up.
        /// </summary>
        void PauseTimer ();

        /// <summary>
        /// Resume the race timer counting up.
        /// </summary>
        void UnpauseTimer ();

        /// <summary>
        /// Called by the TrackManager when the racer has hit the start/finish line.
        /// </summary>
        void HitStartFinishLine ();

        /// <summary>
        /// Gets the current lap the racer is on, starting at 1 once the racer crosses the start line.
        /// </summary>
        int GetCurrentLap ();

        /// <summary>
        /// Gets all the lap times for this racer for their current race.
        /// </summary>
        List<float> GetLapTimes ();

        /// <summary>
        /// Gets the current lap time.
        /// </summary>
        float GetLapTime ();

        /// <summary>
        /// Gets the total time spent in the race so far.
        /// </summary>
        float GetRaceTime ();

        /// <summary>
        /// Gets the name of the racer for scoreboards and records.
        /// </summary>
        string GetName ();
    }
}