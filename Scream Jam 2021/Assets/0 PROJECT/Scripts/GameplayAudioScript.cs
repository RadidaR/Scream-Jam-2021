using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;


namespace ScreamJam
{
    public class GameplayAudioScript : MonoBehaviour
    {
        [SerializeField] GameData data;
        [SoundGroupAttribute][SerializeField] string footstepsGroup;

        bool _isMoving;
        bool _wasMoving;

        bool _wasUsingStairs;

        private void Update()
        {
            _isMoving = Mathf.Abs(data.velocity) > 0.025f;

            if ((!_wasMoving && _isMoving) || data.usingStair /*&& !_wasUsingStairs)*/)
            {
                MasterAudio.PlaySound(footstepsGroup, 1);
            }
            else if ((_wasMoving && !_isMoving) || (!data.usingStair && _wasUsingStairs))
            {
                MasterAudio.StopAllOfSound(footstepsGroup);
            }
        }

        private void LateUpdate()
        {
            _wasMoving = _isMoving;
            _wasUsingStairs = data.usingStair;
        }

        public void PlaySound(string soundGroup)
        {
            MasterAudio.PlaySoundAndForget(soundGroup);
        }

        public void StopSound(string soundGroup)
        {
            MasterAudio.StopAllOfSound(soundGroup);
        }

    }
}
