using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.IO
{
    enum SongState
    {
        None,
        Playing
    }
    static class SoundPlayer
    {
        private static SongState state;
        private static List<Song> songQueue = new List<Song>();

        public static void PlayEffect(SoundEffect soundEffect)
        {
            soundEffect.Play(_soundEffectVolume, 0, 0);
        }

        private static float _normalSongVolume;
        public static float SongVolume
        {
            get
            {
                return _normalSongVolume;
            }
            set
            {
                if (value >= 0 && value <= 1f)
                {
                    _normalSongVolume = value;
                    MediaPlayer.Volume = value;
                }
            }
        }
        private static float _soundEffectVolume = 1f;
        public static float SoundEffectVolume
        {
            get
            {
                return _soundEffectVolume;
            }
            set
            {
                _soundEffectVolume = value;
            }
        }

        public static Song CurrentSong
        {
            get
            {
                if (songQueue.Count > 0)
                    return songQueue.First();
                return null;
            }
        }

        public static void PlaySong(Song song)
        {
            switch (state)
            {
                case SongState.None:
                    songQueue.Add(song);
                    MediaPlayer.Play(songQueue.First());
                    state = SongState.Playing;
                    break;

                case SongState.Playing:
                    if (songQueue.Last().Name != song.Name)
                    {
                        songQueue.Add(song);
                    }
                    break;
            }
        }

        private static bool transition = false;
        private const float SECONDS_FOR_TRANSITION = 10f;
        private static float transitionTimer = 0f;
        private static float volumeChangePerTick;

        public static void Update(GameTime gameTime)
        {
            if (songQueue.Count > 1 || transition == true)
            {
                if (transition == false)
                {
                    transitionTimer = 0;
                    volumeChangePerTick = -(_normalSongVolume / (SECONDS_FOR_TRANSITION / 2) / 60); //Dela med updates per sec, Finns i TargetElapsedTime i klassen Game
                    if (volumeChangePerTick <= 0)
                    {
                        songQueue.RemoveAt(0);
                        MediaPlayer.Play(songQueue.First());
                        return;
                    }
                }

                transition = true;
                transitionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (transitionTimer > SECONDS_FOR_TRANSITION / 2 && volumeChangePerTick <= 0)
                {
                    songQueue.RemoveAt(0);
                    MediaPlayer.Play(songQueue.First());
                    volumeChangePerTick *= -1;
                }
                else if (MediaPlayer.Volume == _normalSongVolume && transitionTimer > SECONDS_FOR_TRANSITION)
                {
                    transition = false;
                }

                MediaPlayer.Volume = Math.Min(_normalSongVolume, Math.Max(MediaPlayer.Volume + volumeChangePerTick, 0));
            }
            else if (songQueue.Count == 0)
            {
                state = SongState.None;
            }
        }
    }
}
