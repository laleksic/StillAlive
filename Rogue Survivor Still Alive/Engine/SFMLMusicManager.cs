using System;
using System.Collections.Generic;
using System.Linq;

using SFML.Audio;

using SFMLMusic = SFML.Audio.Music;

namespace djack.RogueSurvivor.Engine
{
    class SFMLMusicManager : ISoundManager
    {
        #region Fields
        bool m_IsAudioEnabled; //@@MP renamed (Release 2)
        int m_Volume;
        Dictionary<string, SFMLMusic> m_Musics;
        SFMLMusic m_CurrentMusic; // alpha10
        #endregion

        #region Properties
        public bool IsAudioEnabled
        {
            get { return m_IsAudioEnabled; }
            set { m_IsAudioEnabled = value; }
        }

        public int Volume
        {
            get { return m_Volume; }
            set
            {
                m_Volume = value;
                OnVolumeChange();
            }
        }

        // alpha10
        public string Track { get; private set; }
        public int Priority { get; private set; }
        #endregion

        #region Init
        public SFMLMusicManager()
        {
            m_Musics = new Dictionary<string, SFMLMusic>();
            m_Volume = 100;
            this.Priority = AudioPriority.PRIORITY_NULL; //alpha 10
        }

        static string FullName(string fileName) //@@MP - made static (Release 5-7)
        {
            return fileName + ".ogg";
        }
        #endregion

        #region Loading music
        public bool Load(string musicname, string filename)
        {
            filename = FullName(filename);
            Logger.WriteLine(Logger.Stage.INIT_SOUND, String.Format("loading music {0} file {1}", musicname, filename));
            try
            {
                SFMLMusic music = new SFMLMusic(filename);
                m_Musics.Add(musicname, music);
            }
            catch (Exception e)
            {
                Logger.WriteLine(Logger.Stage.INIT_SOUND, String.Format("failed to load music file {0} exception {1}.", filename, e.ToString()));
                throw;
            }
            return true;
        }

        public void Unload(string musicname)
        {
            m_Musics.Remove(musicname);
        }
        #endregion

        #region Playing music

        private void OnVolumeChange()
        {
            foreach (SFMLMusic a in m_Musics.Values)
                a.Volume = m_Volume;
        }

        /// <summary>
        /// Restart playing a music from the beginning if music is enabled.
        /// </summary>
        public void Play(string musicname, int priority)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                //Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing music {0}.", musicname));
                Play(music);
                this.Track = musicname;
                this.Priority = priority;
            }
        }

        /// <summary>
        /// Start playing a music from the beginning if not already playing and if music is enabled.
        /// </summary>
        public void PlayIfNotAlreadyPlaying(string musicname, int priority, bool looping = false) //@@MP - added looping parameter (Release 6-4)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                if (!IsPlaying(music))
                {
                    if (looping)
                        PlayLooping(musicname, priority);
                    else
                        Play(music);
                    this.Track = musicname;
                    this.Priority = priority;
                }
            }
        }

        /// <summary>
        /// Restart playing in a loop a music from the beginning if music is enabled.
        /// </summary>
        public void PlayLooping(string musicname, int priority)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                //Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing looping music {0}.", musicname));
                music.Loop = true;
                Play(music);
                this.Track = musicname;
                this.Priority = priority;
            }
        }

        public void Pause(string musicname) //@@MP (Release 7-3)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                //Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("pausing music {0}.", musicname));
                Pause(music);
            }
        }

        public void PauseAll() //@@MP (Release 7-3)
        {
            if (!m_IsAudioEnabled)
                return;

            foreach (SFMLMusic music in m_Musics.Values)
            {
                //Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("pausing all musics"));
                Pause(music);
            }
        }

        public void ResumeLooping(string musicname)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                //Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("resuming looping music {0}.", musicname));
                Resume(music);
            }
        }

        public void Stop(string musicname)
        {
            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                //Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("stopping music {0}.", musicname));
                Stop(music);
            }
            this.Track = "";
            this.Priority = AudioPriority.PRIORITY_NULL;
        }

        public void StopAll()
        {
            //Logger.WriteLine(Logger.Stage.RUN_SOUND, "stopping all musics.");
            foreach (SFMLMusic a in m_Musics.Values)
            {
                Stop(a);
            }
            this.Track = "";
            this.Priority = AudioPriority.PRIORITY_NULL;
        }

        public void ResumeAll() //@@MP (Release 7-3)
        {
            if (!m_IsAudioEnabled)
                return;

            foreach (SFMLMusic music in m_Musics.Values)
            {
                //Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("resuming all paused musics"));
                if (IsPaused(music))
                    Resume(music);
            }
        }

        public bool IsPlaying(string musicname)
        {
            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                return IsPlaying(music);
            }
            else
                return false;
        }

        public bool IsPaused(string musicname)
        {
            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                return IsPaused(music);
            }
            else
                return false;
        }

        public bool HasEnded(string musicname)
        {
            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                return HasEnded(music);
            }
            else
                return false;
        }

        /// <summary>
        /// Give me a list of track names and I'll pick one at random to play
        /// </summary>
        public void PlayRandom(IEnumerable<string> playlist, int priority) //@@MP (Release 6-1)
        {
            if (!m_IsAudioEnabled)
                return;

            if (!playlist.Any()) //the list is empty
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("empty playlist provided to Music.PlayRandom"));
                return;
            }

            string trackName = playlist.ElementAt(new Random(DateTime.Now.Millisecond).Next(playlist.Count())); // https://stackoverflow.com/questions/2019417/access-random-item-in-list
            PlayIfNotAlreadyPlaying(trackName, priority);
        }

        void Stop(SFMLMusic audio)
        {
            audio.Stop();
            m_CurrentMusic = null; //alpha 10
        }

        void Play(SFMLMusic audio)
        {
            audio.Stop();
            audio.Volume = m_Volume;
            audio.Play();
            m_CurrentMusic = audio; //alpha 10
        }

        static void Resume(SFMLMusic audio) //@@MP - made static (Release 5-7)
        {
            audio.Play();
        }

        static bool IsPlaying(SFMLMusic audio) //@@MP - made static (Release 5-7)
        {
            return audio.Status == SoundStatus.Playing;
        }

        static bool IsPaused(SFMLMusic audio) //@@MP - made static (Release 5-7)
        {
            return audio.Status == SoundStatus.Paused;
        }

        static bool HasEnded(SFMLMusic audio) //@@MP - made static (Release 5-7)
        {
            return audio.Status == SoundStatus.Stopped || audio.PlayingOffset >= audio.Duration;
        }

        static void Pause(SFMLMusic audio) //@@MP (Release 7-3)
        {
            audio.Pause();
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            StopAll(); // alpha10

            Logger.WriteLine(Logger.Stage.CLEAN_SOUND, "disposing SFMLMusicManager...");
            foreach (string musicname in m_Musics.Keys)
            {
                SFMLMusic music = m_Musics[musicname];
                if(music==null)
                {
                    Logger.WriteLine(Logger.Stage.CLEAN_SOUND, String.Format("WARNING: null music for key {0}", musicname));
                    continue;
                }
                Logger.WriteLine(Logger.Stage.CLEAN_SOUND, String.Format("disposing music {0}.", musicname));
                music.Dispose();
            }

            m_Musics.Clear();
            Logger.WriteLine(Logger.Stage.CLEAN_SOUND, "disposing SFMLMusicManager done.");
        }
        #endregion
    }
}
