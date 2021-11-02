using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;

namespace ScreamJam
{
    public class TutorialScript : MonoBehaviour
    {
        [SerializeField] GameData data;
        GameManager manager;

        [SerializeField] TextMeshProUGUI tutorialText;
        [SerializeField] GameObject tutorialPanel;
        Image panelImage;

        [SerializeField] string[] tutorialOneTexts;
        [SerializeField] string[] tutorialTwoTexts;
        [SerializeField] string[] tutorialThreeTexts;

        private void Awake()
        {
            manager = GetComponent<GameManager>();
            panelImage = tutorialPanel.GetComponent<Image>();
            if (manager.currentLevel == 1)
                Timing.RunCoroutine(_Level1(), Segment.Update);
            else if (manager.currentLevel == 2)
                Timing.RunCoroutine(_Level2(), Segment.Update);
            else if (manager.currentLevel == 3)
                Timing.RunCoroutine(_Level3(), Segment.Update);
        }

        private void LateUpdate()
        {
            if (data.dead || manager.levelLost)
                Timing.RunCoroutine(_PanelOff(), Segment.Update);
        }

        IEnumerator<float> _Level1()
        {
            tutorialText.text = tutorialOneTexts[0];
            Timing.RunCoroutine(_PanelOn(), Segment.Update);

            yield return Timing.WaitForSeconds(3f);

            tutorialText.text = tutorialOneTexts[1];

            float timer = 0;
            while (timer < manager.levelDuration)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForSeconds(Time.deltaTime);

                if (tutorialText.text == tutorialOneTexts[1])
                    if (data.canGoUp || data.canGoDown)
                        tutorialText.text = tutorialOneTexts[2];

                if (data.usingStair)
                    break;
            }

            Timing.RunCoroutine(_PanelOff(), Segment.Update);

            timer = 0f;
            while (timer < manager.levelDuration)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForSeconds(Time.deltaTime);

                if (tutorialText.text == tutorialOneTexts[2])
                    if (data.canExorcise)
                    {
                        tutorialText.text = tutorialOneTexts[3];
                        break;
                    }
            }

            Timing.RunCoroutine(_PanelOn(), Segment.Update);

            timer = 0f;
            while (timer < manager.levelDuration)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForSeconds(Time.deltaTime);

                if (manager.itemsLeft.Count < 2)
                    break;
            }

            Timing.RunCoroutine(_PanelOff(), Segment.Update);

        }
        IEnumerator<float> _Level2()
        {
            yield return Timing.WaitForSeconds(1);

            tutorialText.text = tutorialTwoTexts[0];
            Timing.RunCoroutine(_PanelOn(), Segment.LateUpdate);

            if (data.dead)
                yield break;

            float timer = 0;
            while (timer < manager.levelDuration)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime);
                timer += Time.deltaTime;

                if (data.dead)
                    yield break;

                if (tutorialText.text != tutorialTwoTexts[1])
                {
                    if (data.canStab)
                        tutorialText.text = tutorialTwoTexts[1];
                }
                else
                {
                    if (manager.ghostsLeft.Count == 2)
                        break;
                }
            }
            Timing.RunCoroutine(_PanelOff(), Segment.LateUpdate);

            yield return Timing.WaitForSeconds(2);
            tutorialText.text = tutorialTwoTexts[2];

            if (data.dead)
                yield break;

            Timing.RunCoroutine(_PanelOn(), Segment.LateUpdate);

            timer = 0;
            while (timer < manager.levelDuration)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime);
                timer += Time.deltaTime;

                if (data.dead)
                    yield break;

                if (tutorialText.text != tutorialTwoTexts[3])
                {
                    if (data.inSight)
                        tutorialText.text = tutorialTwoTexts[3];
                }
                else
                {
                    if (!data.inSight)
                        break;
                }
            }

            if (data.dead)
                yield break;

            yield return Timing.WaitForSeconds(3);

            Timing.RunCoroutine(_PanelOff(), Segment.LateUpdate);
        }
        IEnumerator<float> _Level3()
        {
            yield return Timing.WaitForSeconds(1);

            tutorialText.text = tutorialThreeTexts[0];
            Timing.RunCoroutine(_PanelOn(), Segment.Update);

            float timer = 0;
            while (timer < manager.levelDuration)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForSeconds(Time.deltaTime);

                if (data.dead)
                    yield break;

                if (data.inSight)
                {
                    tutorialText.text = tutorialThreeTexts[1];
                    break;
                }

                if (tutorialText.text != tutorialThreeTexts[2])
                    if (data.canHide)
                    {
                        tutorialText.text = tutorialThreeTexts[2];
                        break;
                    }
            }

            if (data.dead)
                yield break;

            timer = 0;
            while (timer < manager.levelDuration)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForSeconds(Time.deltaTime);

                if (data.inSight)
                {
                    tutorialText.text = tutorialThreeTexts[1];
                    yield return Timing.WaitForSeconds(2);
                    break;
                }

                if (data.dead)
                    yield break;

                if (data.hiding)
                    break;

            }


            Timing.RunCoroutine(_PanelOff(), Segment.Update);

            if (data.dead)
                yield break;

            yield return Timing.WaitForSeconds(1.5f);

            if (!data.inSight)
            {
                tutorialText.text = tutorialThreeTexts[3];
                Timing.RunCoroutine(_PanelOn(), Segment.Update);
            }

            yield return Timing.WaitForSeconds(4f);
            Timing.RunCoroutine(_PanelOff(), Segment.Update);
        }

        IEnumerator<float> _PanelOn()
        {
            panelImage.color = panelImage.color.SetAlpha(a: 0);
            tutorialPanel.SetActive(true);
            float timer = 0;
            while (timer < 0.3f)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForSeconds(Time.deltaTime);
                tutorialText.color = tutorialText.color.SetAlpha(a: Mathf.Lerp(0, 1, timer / 0.3f));
                panelImage.color = panelImage.color.SetAlpha(a: Mathf.Lerp(0, 1, timer / 0.3f));
            }
            panelImage.color = panelImage.color.SetAlpha(a: 1);
        }
        IEnumerator<float> _PanelOff()
        {
            panelImage.color = panelImage.color.SetAlpha(a: 1);
            float timer = 0;
            while (timer < 0.2f)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForSeconds(Time.deltaTime);
                tutorialText.color = tutorialText.color.SetAlpha(a: Mathf.Lerp(1, 0, timer / 0.2f));
                panelImage.color = panelImage.color.SetAlpha(a: Mathf.Lerp(1, 0, timer / 0.2f));
            }
            panelImage.color = panelImage.color.SetAlpha(a: 0);
            tutorialPanel.SetActive(false);
        }

    }
}
