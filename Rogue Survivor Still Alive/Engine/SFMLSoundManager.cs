using System;
using System.Collections.Generic;
using System.Linq;

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
        SFMLSound m_CurrentSFX; // alpha10
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
        public SFMLSoundManager()
        {
            m_Sounds = new Dictionary<string, SFMLSound>();
            m_Volume = 100;
            this.Priority = AudioPriority.PRIORITY_NULL; //alpha 10
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
        public void Play(string soundname, int priority)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing sound {0}.", soundname));
                Play(sound);
                this.Track = soundname;
                this.Priority = priority;
            }
        }

        /// <summary>
        /// Start playing a sound from the beginning if not already playing and if sound is enabled.
        /// </summary>
        /// <param name="soundname"></param>
        public void PlayIfNotAlreadyPlaying(string soundname, int priority, bool looping = false) //@@MP - added looping parameter (Release 6-4)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                if (!IsPlaying(sound))
                {
                    if (looping)
                        PlayLooping(soundname, priority);
                    else
                        Play(sound);
                    this.Track = soundname;
                    this.Priority = priority;
                }
            }
        }

        /// <summary>
        /// Restart playing in a loop a sound from the beginning if sound is enabled.
        /// </summary>
        /// <param name="soundname"></param>
        public void PlayLooping(string soundname, int priority)
        {
            if (!m_IsAudioEnabled)
                return;

            SFMLSound sound;
            if (m_Sounds.TryGetValue(soundname, out sound))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing looping sound {0}.", soundname));
                sound.Loop = true;
                Play(sound);
                this.Track = soundname;
                this.Priority = priority;
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
            this.Track = "";
            this.Priority = AudioPriority.PRIORITY_NULL;
        }

        public void StopAll()
        {
            Logger.WriteLine(Logger.Stage.RUN_SOUND, "stopping all sounds.");
            foreach (SFMLSound a in m_Sounds.Values)
            {
                Stop(a);
            }
            this.Track = "";
            this.Priority = AudioPriority.PRIORITY_NULL;
            m_CurrentSFX = null; //alpha 10
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

        void Stop(SFMLSound audio)
        {
            audio.Stop();
        }

        void Play(SFMLSound audio)
        {
            audio.Stop();
            audio.Volume = m_Volume;
            audio.Play();
            //m_CurrentSFX = audio; //alpha 10
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
            return audio.Status == SoundStatus.Stopped;// || audio.PlayingOffset >= audio.Duration; //@@MP - Duration is an SFML:Music property only (Release 5-3)
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
        #endregion

        #region IDisposable
        public void Dispose()
        {
            StopAll(); // alpha10

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
