using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        #endregion

        #region Init
        public SFMLMusicManager()
        {
            m_Musics = new Dictionary<string, SFMLMusic>();
            m_Volume = 100;
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
        /// <param name="musicname"></param>
        public void Play(string musicname)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing music {0}.", musicname));
                Play(music);
            }
        }

        /// <summary>
        /// Start playing a music from the beginning if not already playing and if music is enabled.
        /// </summary>
        /// <param name="musicname"></param>
        public void PlayIfNotAlreadyPlaying(string musicname)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                if (!IsPlaying(music))
                    Play(music);
            }
        }

        /// <summary>
        /// Restart playing in a loop a music from the beginning if music is enabled.
        /// </summary>
        /// <param name="musicname"></param>
        public void PlayLooping(string musicname)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing looping music {0}.", musicname));
                music.Loop = true;
                Play(music);
            }
        }

        public void ResumeLooping(string musicname)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("resuming looping music {0}.", musicname));
                Resume(music);
            }
        }

        public void Stop(string musicname)
        {
            SFMLMusic music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("stopping music {0}.", musicname));
                Stop(music);
            }
        }

        public void StopAll()
        {
            Logger.WriteLine(Logger.Stage.RUN_SOUND, "stopping all musics.");
            foreach (SFMLMusic a in m_Musics.Values)
            {
                Stop(a);
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

        static void Stop(SFMLMusic audio) //@@MP - made static (Release 5-7)
        {
            audio.Stop();
        }

        void Play(SFMLMusic audio)
        {
            audio.Stop();
            audio.Volume = m_Volume;
            audio.Play();
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
        #endregion

        #region IDisposable
        public void Dispose()
        {
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
