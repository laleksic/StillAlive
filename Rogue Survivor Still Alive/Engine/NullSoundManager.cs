using System.Collections.Generic;

namespace djack.RogueSurvivor.Engine
{
    class NullSoundManager : ISoundManager
    {
        #region Properties
        public bool IsAudioEnabled { get; set; } //@@MP renamed (Release 2)
        public int Volume { get; set; }
        // alpha10
        public string Track { get; private set; }
        public int Priority { get; private set; }
        #endregion

        #region Init
        public NullSoundManager() { }
        #endregion

        #region Loading music
        public bool Load(string musicname, string filename) { return true; }

        public void Unload(string musicname) { }
        #endregion

        #region Playing music

        public void Play(string musicname, int priority) { }

        public void PlayIfNotAlreadyPlaying(string musicname, int priority, bool looping = false) { } //@@MP - added looping parameter (Release 6-4)

        public void PlayLooping(string musicname, int priority) { }

        public void ResumeLooping(string musicname) { }

        public void Stop(string musicname) { }

        public void StopAll() { }

        public bool IsPlaying(string musicname) { return false; }

        public bool IsPaused(string musicname) { return false; }

        public bool HasEnded(string musicname) { return true; }

        public void PlayRandom(IEnumerable<string> playlist, int priority) { } //@@MP (Release 6-1)
        #endregion

        #region IDisposable
        public void Dispose() { }
        #endregion
    }
}
