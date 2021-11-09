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


        [SerializeField] GameEvent _event;

        bool _isMoving;
        bool _wasMoving;

        bool _wasUsingStairs;

        bool _wasSeen;

        private void Start()
        {
            PlaySound(gameplayMusicGroup);
            _gameplayMusicDefaultVolume = MasterAudio.GetGroupVolume(gameplayMusicGroup);
            _chaseMusicDefaultVolume = MasterAudio.GetGroupVolume(chaseMusicGroup);
        }

        private void Update()
        {
            _isMoving = Mathf.Abs(data.velocity) > 0.025f;

            if ((!_wasMoving && _isMoving) || data.usingStair /*&& !_wasUsingStairs)*/)
            {
                PlaySound(footstepsGroup);
            }
            else if ((_wasMoving && !_isMoving) || (!data.usingStair && _wasUsingStairs))
            {
                StopSound(footstepsGroup);
            }

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

        private void LateUpdate()
        {
            _wasMoving = _isMoving;
            _wasUsingStairs = data.usingStair;
            _wasSeen = data.inSight;
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
