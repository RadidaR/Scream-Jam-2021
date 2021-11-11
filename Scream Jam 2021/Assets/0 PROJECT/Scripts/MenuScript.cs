using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using MEC;

namespace ScreamJam
{
    public class MenuScript : MonoBehaviour
    {
        [SoundGroupAttribute] public string menuMusic;
        [SoundGroupAttribute] public string thunder;
        [SerializeField] GameData data;
        [SerializeField] GameObject muteCross;

        bool coroutine = false;
        //[SerializeField] MasterAudio _audio;

        public void LoadScene(int sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber);
        }

        private void Start()
        {
            if (data.muted)
            {
                Mute();
            }
            else
            {
                Unmute();
            }
            PlaySound(menuMusic);
            PlaySound(thunder);
        }

        public void PlaySound(string soundGroupName)
        {
            MasterAudio.PlaySound(soundGroupName);
        }

        public void StopSound(string soundGroupName)
        {
            MasterAudio.StopAllOfSound(soundGroupName);
        }

        public void LoadSceneWithDelay(int sceneNumber)
        {
            if (!coroutine)
                Timing.RunCoroutine(_LoadSceneDelay(sceneNumber), Segment.LateUpdate);
        }



        IEnumerator<float> _LoadSceneDelay(int sceneNumber)
        {
            coroutine = true;
            yield return Timing.WaitForSeconds(1f);
            SceneManager.LoadScene(sceneNumber);
        }

        public void MuteUnmute()
        {
            if (!data.muted)
            {
                Mute();
            }
            else
            {
                Unmute();
            }
        }

        void Mute()
        {
            data.muted = true;
            MasterAudio.MuteEverything();
            muteCross.SetActive(true);
        }

        void Unmute()
        {
            data.muted = false;
            MasterAudio.UnmuteEverything();
            muteCross.SetActive(false);
        }

    }
}
