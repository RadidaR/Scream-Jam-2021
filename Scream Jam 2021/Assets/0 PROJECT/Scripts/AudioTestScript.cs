using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;

namespace ScreamJam
{
    public class AudioTestScript : MonoBehaviour
    {
        [SoundGroupAttribute] public string gameMusic;
        [SoundGroupAttribute] public string footsteps;
        [MasterCustomEventAttribute] public string destroyGhostEvent;

        [SerializeField] bool isMoving;
        [SerializeField] bool wasMoving;

        bool _wasUsingStairs;

        [SerializeField] GameData data;
        // Start is called before the first frame update
        void Start()
        {
            //MasterAudio.PlaySound(gameMusic, 1);
        }

        private void Update()
        {
            isMoving = Mathf.Abs(data.velocity) > 0.025f;

            if ((!wasMoving && isMoving) || data.usingStair /*&& !_wasUsingStairs)*/)
            {
                MasterAudio.PlaySound(footsteps, 1);
            }
            else if ((wasMoving && !isMoving) || (!data.usingStair && _wasUsingStairs))
            {
                MasterAudio.StopAllOfSound(footsteps);
            }
        }

        private void LateUpdate()
        {
            wasMoving = isMoving;
            _wasUsingStairs = data.usingStair;
        }

        public void PlaySound(string soundGroup)
        {
            MasterAudio.PlaySound(soundGroup);
        }



    }
}
