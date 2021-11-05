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

        [SerializeField] GameData data;
        // Start is called before the first frame update
        void Start()
        {
            //MasterAudio.PlaySound(gameMusic, 1);
        }

        private void Update()
        {
            isMoving = Mathf.Abs(data.velocity) > 0.025f;

            if (!wasMoving && isMoving)
            {
                MasterAudio.PlaySound(footsteps, 1);
            }
            else if (wasMoving && !isMoving)
            {
                MasterAudio.StopAllOfSound(footsteps);
            }
        }

        private void LateUpdate()
        {
            wasMoving = isMoving;
        }

    }
}
