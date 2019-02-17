using System;
using System.Collections.Generic;
using Microsoft.DirectX.AudioVideoPlayback;
using Microsoft.DirectX;
using System.Linq;

namespace djack.RogueSurvivor.Engine
{
    class MDXSoundManager : ISoundManager
    {
        #region Fields
        bool m_IsAudioEnabled; //@@MP renamed (Release 2)
        int m_Volume;
        int m_Attenuation;
        Dictionary<string, Audio> m_Musics;
        Audio m_CurrentAudio; // alpha10
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
        public MDXSoundManager()
        {
            m_Musics = new Dictionary<string, Audio>();
            this.Volume = 100;
            this.Priority = AudioPriority.PRIORITY_NULL; //alpha 10
        }

        static string FullName(string fileName) //@@MP - made static (Release 5-7)
        {
            return fileName + ".ogg";//".mp3"; //@@MP (Release 5-3)
        }
        #endregion

        #region Loading music
        public bool Load(string musicname, string filename)
        {
            filename = FullName(filename);
            Logger.WriteLine(Logger.Stage.INIT_SOUND, String.Format("loading music {0} file {1}", musicname, filename));
            try
            {
                Audio music = new Audio(filename);
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
            m_Attenuation = ComputeDXAttenuationFromVolume();
            foreach (Audio a in m_Musics.Values)
                try
                {
                    a.Volume = -m_Attenuation; // yep mdx volume is negative and means attenuation instead of volume.
                }
                catch (DirectXException)
                {
                }
        }

        /**
         * MDX is retarded, "volume" audio property means attenuation instead and 0 is max volume and -10000 is zero db.
         * Go figure.
         */
        private int ComputeDXAttenuationFromVolume()
        {
            const int MIN_ATT = 10000;
            const int ATT_FACTOR = 2500; // should be min_att but it doesn't work. mdx is weird like that.
            if (m_Volume <= 0)
                return MIN_ATT;
            int att = ((100 - m_Volume) * ATT_FACTOR) / 100;
            return att;
        }

        /// <summary>
        /// Restart playing a music from the beginning if music is enabled.
        /// </summary>
        /// <param name="musicname"></param>
        public void Play(string musicname, int priority)
        {
            if (!m_IsAudioEnabled)
                return;

            Audio music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing music {0}.", musicname));
                Play(music);
                this.Track = musicname;
                this.Priority = priority;
            }
        }

        /// <summary>
        /// Start playing a music from the beginning if not already playing and if music is enabled.
        /// </summary>
        /// <param name="musicname"></param>
        public void PlayIfNotAlreadyPlaying(string musicname, int priority, bool looping = false) //@@MP - added looping parameter (Release 6-4)
        {
            if (!m_IsAudioEnabled)
                return;

            Audio music;
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
        /// <param name="musicname"></param>
        public void PlayLooping(string musicname, int priority)
        {
            if (!m_IsAudioEnabled)
                return;

            Audio music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("playing looping music {0}.", musicname));
                music.Ending += new EventHandler(OnLoopingMusicEnding);
                Play(music);
                this.Track = musicname;
                this.Priority = priority;
            }
        }

        public void ResumeLooping(string musicname)
        {
            if (!m_IsAudioEnabled)
                return;

            Audio music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("resuming looping music {0}.", musicname));
                Resume(music);
            }
        }

        void OnLoopingMusicEnding(object sender, EventArgs e)
        {
            Audio music = (Audio)sender;
            Play(music);
        }

        public void Stop(string musicname)
        {
            Audio music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("stopping music {0}.", musicname));
                Stop(music);
            }
            this.Track = "";
            this.Priority = AudioPriority.PRIORITY_NULL;
        }

        public void StopAll()
        {
            Logger.WriteLine(Logger.Stage.RUN_SOUND, "stopping all musics.");
            foreach (Audio a in m_Musics.Values)
            {
                Stop(a);
            }
            this.Track = "";
            this.Priority = AudioPriority.PRIORITY_NULL;
        }

        public bool IsPlaying(string musicname)
        {
            Audio music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                return IsPlaying(music);
            }
            else
                return false;
        }

        public bool IsPaused(string musicname)
        {
            Audio music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                return IsPaused(music);
            }
            else
                return false;
        }

        public bool HasEnded(string musicname)
        {
            Audio music;
            if (m_Musics.TryGetValue(musicname, out music))
            {
                return HasEnded(music);
            }
            else
                return false;
        }

        void Stop(Audio audio)
        {
            audio.Ending -= OnLoopingMusicEnding;
            //audio.Pause(); //@@MP (Release 2)
            m_CurrentAudio = null; //alpha 10
            if (audio.Playing)
                audio.Stop();
        }

        void Play(Audio audio)
        {
            audio.Stop();
            audio.SeekCurrentPosition(0, SeekPositionFlags.AbsolutePositioning);
            audio.Volume = -m_Attenuation;
            audio.Play();
            m_CurrentAudio = audio; //alpha 10
        }

        static void Resume(Audio audio) //@@MP - made static (Release 5-7)
        {
            audio.Play();
        }

        static bool IsPlaying(Audio audio) //@@MP - made static (Release 5-7)
        {
            return audio.CurrentPosition > 0 && audio.CurrentPosition < audio.Duration && audio.State == StateFlags.Running;
        }

        static bool IsPaused(Audio audio) //@@MP - made static (Release 5-7)
        {
            return (audio.State & StateFlags.Paused) != 0;
        }

        static bool HasEnded(Audio audio)
        {
            return audio.CurrentPosition >= audio.Duration;
        }

        public void PlayRandom(IEnumerable<string> playlist, int priority) //@@MP (Release 6-1)
        {
            if (!m_IsAudioEnabled)
                return;

            if (!playlist.Any()) //the list is empty
            {
                Logger.WriteLine(Logger.Stage.RUN_SOUND, String.Format("empty playlist provided to Audio.PlayRandom"));
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

            Logger.WriteLine(Logger.Stage.CLEAN_SOUND, "disposing MDXMusicManager...");
            foreach (string musicname in m_Musics.Keys)
            {
                Audio music = m_Musics[musicname];
                if(music==null)
                {
                    Logger.WriteLine(Logger.Stage.CLEAN_SOUND, String.Format("WARNING: null music for key {0}", musicname));
                    continue;
                }
                Logger.WriteLine(Logger.Stage.CLEAN_SOUND, String.Format("disposing music {0}.", musicname));
                music.Dispose();
            }

            m_Musics.Clear();
            Logger.WriteLine(Logger.Stage.CLEAN_SOUND, "disposing MDXMusicManager done.");
        }
        #endregion
    }
}
