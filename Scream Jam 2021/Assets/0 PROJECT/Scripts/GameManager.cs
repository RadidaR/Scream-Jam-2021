using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

namespace ScreamJam
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameData data;

        [SerializeField] int currentLevel;
        [SerializeField] float levelDuration;

        [SerializeField] PossessedItem[] itemsTotal;
        [SerializeField] GhostScript[] ghostsTotal;

        [SerializeField] TextMeshProUGUI itemsText;
        [SerializeField] TextMeshProUGUI ghostsText;

        [SerializeField] float currentTime;
        [SerializeField] Light2D dayLight;
        float startingLight;

        [SerializeField] bool levelCompleted;
        [SerializeField] bool levelLost;

        private void Awake()
        {
            Timing.RunCoroutine(_LevelDuration(), Segment.Update);
            itemsTotal = FindObjectsOfType<PossessedItem>();
            ghostsTotal = FindObjectsOfType<GhostScript>();

            startingLight = dayLight.intensity;

            if (itemsTotal != null)
            {
                itemsText.text = $"Possessed Items: {itemsTotal.Length} / {itemsTotal.Length}";
                itemsText.gameObject.SetActive(true);
            }

            if (ghostsTotal != null)
            {
                ghostsText.gameObject.SetActive(true);
                ghostsText.text = $"Ghosts: {ghostsTotal.Length} / {ghostsTotal.Length}";
            }

            UpdateUI();
        }

        IEnumerator<float> _LevelDuration()
        {
            while (!levelCompleted && !levelLost)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime);
                currentTime += Time.deltaTime;
                dayLight.intensity = Mathf.Lerp(startingLight, 1, currentTime / levelDuration);

                if (levelCompleted)
                    break;

                if (currentTime > levelDuration)
                {
                    levelLost = true;
                    break;
                }
            }
        }

        void UpdateUI()
        {
            //itemsText.text = $"Possessed Items: {}"
        }
        void Win()
        {

        }


    }
}
