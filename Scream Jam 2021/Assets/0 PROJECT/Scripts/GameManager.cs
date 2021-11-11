using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MEC;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;
using MPUIKIT;

namespace ScreamJam
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameData data;

        public int currentLevel;
        public float levelDuration;

        [SerializeField] PossessedItem[] itemsTotal;
        [SerializeField] GhostScript[] ghostsTotal;
        public List<PossessedItem> itemsLeft;
        public List<GhostScript> ghostsLeft;

        [SerializeField] GameObject itemBackground;
        [SerializeField] GameObject ghostBackground;
        [SerializeField] TextMeshProUGUI itemsText;
        [SerializeField] TextMeshProUGUI ghostsText;
        //[SerializeField] TextMeshProUGUI timerText;
        [SerializeField] MPImage timerImage;
        [SerializeField] Color startColor;
        [SerializeField] Color midColor;
        [SerializeField] Color endColor;

        [SerializeField] float currentTime;
        [SerializeField] Light2D dayLight;
        [SerializeField] Light2D houseLight;
        float startingDaylight;
        float startingHouseLight;
        [SerializeField] float maxHouseLight;

        public bool levelCompleted;
        public bool levelLost;

        //[SerializeField] GameObject gameWonScreen;
        //[SerializeField] GameObject gameOverScreen;

        [SerializeField] GameEvent eGameOver;
        [SerializeField] GameEvent eGameWon;
        [SerializeField] GameEvent eStartTicking;

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
            //timerText.text = $"Time Left: {Mathf.Round(levelDuration - currentTime)}";
            //timerImage.fillAmount = Mathf.Lerp()
            //timerImage.color = startColor;
        }

        IEnumerator<float> _LevelDuration()
        {
            bool timeAlmostGone = false;

            while (!levelCompleted && !levelLost)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime);
                currentTime += Time.deltaTime;
                float percent = currentTime / levelDuration;

                dayLight.intensity = Mathf.Lerp(startingDaylight, 1, percent);
                houseLight.intensity = Mathf.Lerp(startingHouseLight, maxHouseLight, percent);
                timerImage.fillAmount = Mathf.Lerp(1, 0, percent);

                if (percent < 0.5f)
                    timerImage.color = startColor.LerpToColor(midColor, percent * 2);
                else
                {
                    timerImage.color = midColor.LerpToColor(endColor, (percent - 0.5f) / 0.5f);
                }

                if (!timeAlmostGone && percent > 0.75f)
                {
                    timeAlmostGone = true;
                    eStartTicking.Raise();
                }

                if (levelCompleted)
                    break;

                if (currentTime >= levelDuration)
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
            {
                itemBackground.SetActive(false);
                itemsText.gameObject.SetActive(false);
            }
            else
            {
                itemsText.text = $" {itemsTotal.Length - itemsLeft.Count} / {itemsTotal.Length}";
                itemBackground.SetActive(true);
                itemsText.gameObject.SetActive(true);
            }

            if (ghostsLeft.Count == 0)
            {
                ghostBackground.SetActive(false);
                ghostsText.gameObject.SetActive(false);
            }
            else
            {
                ghostsText.text = $" {ghostsTotal.Length - ghostsLeft.Count} / {ghostsTotal.Length}";
                ghostBackground.SetActive(true);
                ghostsText.gameObject.SetActive(true);
            }
        }

        void Win()
        {
            levelCompleted = true;
            Timing.RunCoroutine(_LevelCompleted().CancelWith(gameObject), Segment.LateUpdate);
        }

        IEnumerator<float> _LevelCompleted()
        {
            yield return Timing.WaitForSeconds(1);
            eGameWon.Raise();
        }

        public void Lose()
        {
            levelLost = true;
            data.dead = true;
            eGameOver.Raise();
        }

        public void LoadScene(int sceneNumber)
        {
            Time.timeScale = 1f;
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
