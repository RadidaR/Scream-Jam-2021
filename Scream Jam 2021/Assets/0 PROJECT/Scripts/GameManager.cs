using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] List<PossessedItem> itemsLeft;
        [SerializeField] List<GhostScript> ghostsLeft;

        [SerializeField] TextMeshProUGUI itemsText;
        [SerializeField] TextMeshProUGUI ghostsText;
        [SerializeField] TextMeshProUGUI timerText;

        [SerializeField] float currentTime;
        [SerializeField] Light2D dayLight;
        [SerializeField] Light2D houseLight;
        float startingDaylight;
        float startingHouseLight;
        [SerializeField] float maxHouseLight;

        [SerializeField] bool levelCompleted;
        [SerializeField] bool levelLost;

        [SerializeField] GameObject gameWonScreen;
        [SerializeField] GameObject gameOverScreen;

        private void Awake()
        {
            Time.timeScale = 1;
            Timing.RunCoroutine(_LevelDuration(), Segment.Update);
            itemsTotal = FindObjectsOfType<PossessedItem>();
            ghostsTotal = FindObjectsOfType<GhostScript>();

            startingDaylight = dayLight.intensity;
            startingHouseLight = houseLight.intensity;

            if (itemsTotal != null)
            {
                foreach (PossessedItem item in itemsTotal)
                {
                    itemsLeft.Add(item);
                }
            }

            if (ghostsTotal != null)
            {
                foreach (GhostScript ghost in ghostsTotal)
                {
                    ghostsLeft.Add(ghost);
                }
            }
            Timing.RunCoroutine(_UpdateTasks(), Segment.LateUpdate);
        }

        void Update()
        {
            timerText.text = $"Time Left: {Mathf.Round(levelDuration - currentTime)}";
        }

        IEnumerator<float> _LevelDuration()
        {
            while (!levelCompleted && !levelLost)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime);
                currentTime += Time.deltaTime;
                dayLight.intensity = Mathf.Lerp(startingDaylight, 1, currentTime / levelDuration);
                houseLight.intensity = Mathf.Lerp(startingHouseLight, maxHouseLight, currentTime / levelDuration);

                if (levelCompleted)
                    break;

                if (currentTime > levelDuration)
                {
                    Lose();
                    break;
                }
            }
        }

        public void UpdateTasks()
        {
            //Timing.RunCoroutine(_StartUpdate(), Segment.LateUpdate);
            Timing.RunCoroutine(_UpdateTasks(), Segment.LateUpdate);
        }
        IEnumerator<float> _UpdateTasks()
        {
            yield return Timing.WaitForOneFrame;

            if (itemsLeft.Count != 0)
            {
                List<PossessedItem> items = new List<PossessedItem>();
                foreach (PossessedItem item in itemsLeft)
                {
                    if (!item.possessed)
                        items.Add(item);
                }

                if (items.Count != 0)
                {
                    foreach (PossessedItem item in items)
                    {
                        itemsLeft.Remove(item);
                    }
                }

            }

            for (int i = 0; i < ghostsLeft.Count; i++)
            {
                if (ghostsLeft[i] == null)
                    ghostsLeft.Remove(ghostsLeft[i]);
            }            

            UpdateUI();

            if (itemsLeft.Count == 0 && ghostsLeft.Count == 0)
                Win();
        }

        void UpdateUI()
        {
            if (itemsLeft.Count == 0)
                itemsText.gameObject.SetActive(false);
            else
                itemsText.gameObject.SetActive(true);

            if (ghostsLeft.Count == 0)
                ghostsText.gameObject.SetActive(false);
            else
                ghostsText.gameObject.SetActive(true);

            itemsText.text = $"Possessed Items: {itemsLeft.Count} / {itemsTotal.Length}";

            ghostsText.text = $"Ghosts: {ghostsLeft.Count} / {ghostsTotal.Length}";
        }
        void Win()
        {
            levelCompleted = true;
            gameWonScreen.SetActive(true);
            Time.timeScale = 0;
        }

        public void Lose()
        {
            levelLost = true;
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }

        public void LoadScene(int sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber);
        }

        public void ReloadLevel()
        {
            LoadScene(currentLevel);
        }

        public void NextLevel()
        {
            LoadScene(currentLevel + 1);
        }
    }
}
