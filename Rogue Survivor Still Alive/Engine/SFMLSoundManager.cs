using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Audio;

using SFMLSound = SFML.Audio.Sound;

namespace djack.RogueSurvivor.Engine
{
    class SFMLSoundManager : ISoundManager
    {
        #region Fields
        bool m_IsAudioEnabled; //@@MP renamed (Release 2)
        int m_Volume;
        Dictionary<string, SFMLSound> m_Sounds;
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
        public SFMLSoundManager()
        {
            m_Sounds = new Dictionary<string, SFMLSound>();
            m_Volume = 100;
        }

        static string FullName(string fileName) //@@MP - made static (Release 5-7)
        {
            return fileName + ".ogg";
        }
        #endregion

        #region Loading sound
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public bool Load(string soundname, string filename)
        {
            filename = FullName(filename);
            Logger.WriteLine(Logger.Stage.INIT_SOUND, String.Format("loading sound {0} file {1}", soundname, filename));
            SoundBuffer buffer = null; //@@MP - try/finally ensures that the buffer is always closed (Release 5-7)
            try
            {
                //SFMLSound sound = new SFMLSound(filename);
                buffer = new SoundBuffer(filename); //@@MP - SFML:Sound buffers rather than streams from the drive like Music (Release 5-3)
                SFMLSound sound = new SFMLSound(buffer);
                m_Sounds.Add(soundname, sound);
            }
            catch (Exception e)
            {
                Logger.WriteLine(Logger.Stage.INIT_SOUND, String.Format("failed to load sound file {0} exception {1}.", filename, e.ToString()));
                throw;
            }
            finally
            {
                /*if (buffer != null)
                    buffer.Dispose);*/
            }

            return true;
        }

        public void Unload(string soundname)
        {
            m_Sounds.Remove(soundname);
        }
        #endregion

        #region Playing sound

        private void OnVolumeChange()
        {
            foreach (SFMLSound a in m_Sounds.Values)
                a.Volume = m_Volume;
        }

        /// <summary>
        /// Restart playing a sound from the beginning if sound is enabled.
        /// </summary>
        /// <param name="soundname"></param>
        public void Play(string soundname)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing sound {0}.", soundname));
                Play(sound);
            }
        }

        /// <summary>
        /// Start playing a sound from the beginning if not already playing and if sound is enabled.
        /// </summary>
        /// <param name="soundname"></param>
        public void PlayIfNotAlreadyPlaying(string soundname)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                if (!IsPlaying(sound))
                    Play(sound);
            }
        }

        /// <summary>
        /// Restart playing in a loop a sound from the beginning if sound is enabled.
        /// </summary>
        /// <param name="soundname"></param>
        public void PlayLooping(string soundname)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing looping sound {0}.", soundname));
                sound.Loop = true;
                Play(sound);
            }
        }

        public void ResumeLooping(string soundname)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("resuming looping sound {0}.", soundname));
                Resume(sound);
            }
        }

        public void Stop(string soundname)
        {
            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("stopping sound {0}.", soundname));
                Stop(sound);
            }
        }

        public void StopAll()
        {
            Logger.WriteLine(Logger.Stage.RUN_SOUND, "stopping all sounds.");
            foreach (SFMLSound a in m_Sounds.Values)
            {
                Stop(a);
            }
        }

        public bool IsPlaying(string soundname)
        {
            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                return IsPlaying(sound);
            }
            else
                return false;
        }

        public bool IsPaused(string soundname)
        {
            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                return IsPaused(sound);
            }
            else
                return false;
        }

        public bool HasEnded(string soundname)
        {
            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                return HasEnded(sound);
            }
            else
                return false;
        }

        static void Stop(SFMLSound audio) //@@MP - made static (Release 5-7)
        {
            audio.Stop();
        }

        void Play(SFMLSound audio)
        {
            audio.Stop();
            audio.Volume = m_Volume;
            audio.Play();
        }

        static void Resume(SFMLSound audio) //@@MP - made static (Release 5-7)
        {
            audio.Play();
        }

        static bool IsPlaying(SFMLSound audio) //@@MP - made static (Release 5-7)
        {
            return audio.Status == SoundStatus.Playing;
        }

        static bool IsPaused(SFMLSound audio) //@@MP - made static (Release 5-7)
        {
            return audio.Status == SoundStatus.Paused;
        }

        static bool HasEnded(SFMLSound audio) //@@MP - made static (Release 5-7)
        {
            return audio.Status == SoundStatus.Stopped;// || audio.PlayingOffset >= audio.Duration; //@@MP - Duration is a SFML:Music property only (Release 5-3)
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Logger.WriteLine(Logger.Stage.CLEAN_SOUND, "disposing SFMLSoundManager...");
            foreach (string soundname in m_Sounds.Keys)
            {
                SFMLSound sound = m_Sounds[soundname];
                if(sound==null)
                {
                    Logger.WriteLine(Logger.Stage.CLEAN_SOUND, String.Format("WARNING: null sound for key {0}", soundname));
                    continue;
                }
                Logger.WriteLine(Logger.Stage.CLEAN_SOUND, String.Format("disposing sound {0}.", soundname));
                sound.Dispose();
            }

            m_Sounds.Clear();
            Logger.WriteLine(Logger.Stage.CLEAN_SOUND, "disposing SFMLSoundManager done.");
        }
        #endregion
    }
}
