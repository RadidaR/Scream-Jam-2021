using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;


namespace ScreamJam
{
    public class GameplayAudioScript : MonoBehaviour
    {
        [SerializeField] GameData data;
        [SoundGroupAttribute][SerializeField] string footstepsGroup;


        [SoundGroupAttribute] [SerializeField] string gameplayMusicGroup;
        float _gameplayMusicDefaultVolume;

        [SoundGroupAttribute] [SerializeField] string chaseMusicGroup;
        float _chaseMusicDefaultVolume;

        [SoundGroupAttribute] [SerializeField] string ghostsPresentGroup;
        [SoundGroupAttribute] [SerializeField] string caughtGroup;
        //[SoundGroupAttribute] [SerializeField] string levelLostGroup;

        [SerializeField] GameManager _manager;
        [SerializeField] GameEvent _event;

        bool _isMoving;
        bool _wasMoving;

        bool _wasUsingStairs;

        bool _wasSeen;

        bool _wasDead;

        bool _wasCompleted;

        private void Start()
        {
            PlaySound(gameplayMusicGroup);
            _gameplayMusicDefaultVolume = MasterAudio.GetGroupVolume(gameplayMusicGroup);
            _chaseMusicDefaultVolume = MasterAudio.GetGroupVolume(chaseMusicGroup);

            if (_manager.ghostsLeft.Count != 0)
            {
                PlaySound(ghostsPresentGroup);
            }
        }

        private void Update()
        {
            _isMoving = Mathf.Abs(data.velocity) > 0.025f;

            if (((!_wasMoving && _isMoving) || data.usingStair /*&& !_wasUsingStairs)*/) && !data.hiding)
            {
                PlaySound(footstepsGroup);
            }
            else if (((_wasMoving && !_isMoving) || (!data.usingStair && _wasUsingStairs)) || data.hiding)
            {
                StopSound(footstepsGroup);
            }

            if (!data.dead)
            {
                if (data.inSight && !_wasSeen)
                {
                    FadeSound(gameplayMusicGroup, 0.05f, 0.35f);
                    PlaySound(chaseMusicGroup);
                    FadeSound(chaseMusicGroup, _chaseMusicDefaultVolume, 0.35f);
                }
                else if (!data.inSight && _wasSeen)
                {
                    FadeSound(chaseMusicGroup, 0f, 0.35f);
                    FadeSound(gameplayMusicGroup, _gameplayMusicDefaultVolume, 0.35f);
                }

                if (MasterAudio.GetGroupVolume(chaseMusicGroup) == 0)
                {
                    StopSound(chaseMusicGroup);
                    MasterAudio.SetGroupVolume(chaseMusicGroup, _chaseMusicDefaultVolume);
                }
            }
            else if (data.dead && !_wasDead)
            {
                FadeSound(gameplayMusicGroup, 0, 0.5f);
                FadeSound(chaseMusicGroup, 0, 0.5f);
                StopSound(footstepsGroup);
                PlaySound(caughtGroup);
            }

            if (_manager.levelCompleted && !_wasCompleted)
            {
                FadeSound(gameplayMusicGroup, 0, 1f);
                StopSound(footstepsGroup);
            }
        }

        private void LateUpdate()
        {
            _wasMoving = _isMoving;
            _wasUsingStairs = data.usingStair;
            _wasSeen = data.inSight;
            _wasDead = data.dead;
            _wasCompleted = _manager.levelCompleted;

            if (_manager.ghostsLeft.Count == 0)
            {
                StopSound(ghostsPresentGroup);
            }

            if (_manager.levelCompleted && MasterAudio.GetGroupVolume(gameplayMusicGroup) == 0)
            {
                StopSound(gameplayMusicGroup);
            }
        }

        public void PlaySound(string soundGroup)
        {
            MasterAudio.PlaySoundAndForget(soundGroup);
        }

        public void FadeSound(string soundGroup, float toVolume, float overTime)
        {
            MasterAudio.FadeSoundGroupToVolume(soundGroup, toVolume, overTime);
        }

        public void FadeGameMusic(float time)
        {
            MasterAudio.FadeSoundGroupToVolume(gameplayMusicGroup, 0.05f, time);
        }

        public void RestoreGameMusic(float time)
        {
            MasterAudio.FadeSoundGroupToVolume(gameplayMusicGroup, _gameplayMusicDefaultVolume, time);
        }

        public void StopSound(string soundGroup)
        {
            MasterAudio.StopAllOfSound(soundGroup);
        }

        [Button("Raise Event")]
        void Raise()
        {
            _event.Raise();
        }

    }
}
