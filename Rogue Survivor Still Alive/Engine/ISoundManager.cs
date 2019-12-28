using System;
using System.Collections.Generic;

namespace djack.RogueSurvivor.Engine
{
    // alpha10 Added concept of music priority, can play only one music at a time, renamed to MusicManager and
    // some cleanup. Concrete classes updated.

    static class AudioPriority
    {
        /// <summary>
        /// Lowest priority when not playing any music.
        /// </summary>
        public const int PRIORITY_NULL = 0;  // must be 0!

        /// <summary>
        /// Medium priority for background musics.
        /// </summary>
        public const int PRIORITY_BGM = 1;

        /// <summary>
        /// High priority for events musics.
        /// </summary>
        public const int PRIORITY_EVENT = 2;
    }

    interface ISoundManager : IDisposable
    {
        #region Properties
        bool IsAudioEnabled { get; set; } //@@MP renamed (Release 2)
        int Volume { get; set; }
        // alpha10
        int Priority { get; }
        string Track { get; }
        #endregion

        #region Loading music
        bool Load(string musicname, string filename);

        void Unload(string musicname);
        #endregion

        #region Playing music
        /// <summary>
        /// Restart playing a music from the beginning if music is enabled.
        /// </summary>
        void Play(string musicname, int priority);

        /// <summary>
        /// Start playing a music from the beginning if not already playing and if music is enabled.
        /// </summary>
        void PlayIfNotAlreadyPlaying(string musicname, int priority, bool looping = false); //@@MP - added looping parameter (Release 6-4)

        /// <summary>
        /// Restart playing in a loop a music from the beginning if music is enabled.
        /// </summary>
        void PlayLooping(string musicname, int priority);

        void ResumeLooping(string musicname);

        void Stop(string musicname);

        void StopAll();

        bool IsPlaying(string musicname);

        bool IsPaused(string musicname);

        void Pause(string musicname); //@@MP (Release 7-3)

        void PauseAll(); //@@MP (Release 7-3)

        void ResumeAll(); //@@MP (Release 7-3)

        bool HasEnded(string musicname);

        /// <summary>
        /// Give me a list of track names and I'll pick one at random to play
        /// </summary>
        void PlayRandom(IEnumerable<string> playlist, int priority); //@@MP (Release 6-1)
        #endregion
    }
}
