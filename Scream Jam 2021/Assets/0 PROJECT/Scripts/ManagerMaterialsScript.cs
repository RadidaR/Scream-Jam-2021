using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using Sirenix.OdinInspector;

namespace ScreamJam
{
    public class ManagerMaterialsScript : MonoBehaviour
    {
        [SerializeField] GameObject gameOverScreen;
        Material gameOverScreenMaterial;
        int glowDissolveID;

        [SerializeField] GameObject gameWonScreen;
        Material gameWonScreenMaterial;
        int alphaDissolveID;

        [SerializeField] float alphaDissolveFadeMaxValue;



        [SerializeField] SpriteRenderer[] deadPlayerSprites;
        [SerializeField] SpriteRenderer[] winningPlayerSprites;

        [SerializeField] GameObject ghostPlayer;

        private void Awake()
        {
            gameOverScreen.GetComponent<Image>().material = new Material(gameOverScreen.GetComponent<Image>().material);
            gameOverScreenMaterial = gameOverScreen.GetComponent<Image>().material;

            glowDissolveID = Shader.PropertyToID("_FullGlowDissolveFade");

            gameWonScreen.GetComponent<Image>().material = new Material(gameWonScreen.GetComponent<Image>().material);
            gameWonScreenMaterial = gameWonScreen.GetComponent<Image>().material;

            alphaDissolveID = Shader.PropertyToID("_SourceAlphaDissolveFade");




            deadPlayerSprites[0].material = new Material(deadPlayerSprites[0].material);
            Material deadPlayerMaterial = deadPlayerSprites[0].material;

            foreach (SpriteRenderer sprite in deadPlayerSprites)
            {
                sprite.material = deadPlayerMaterial;
            }

            winningPlayerSprites[0].material = new Material(winningPlayerSprites[0].material);
            Material winningPlayerMaterial = winningPlayerSprites[0].material;

            foreach (SpriteRenderer sprite in winningPlayerSprites)
            {
                sprite.material = winningPlayerMaterial;
            }

        }
        [Button("GameOver")]
        public void GameOver()
        {
            gameOverScreen.SetActive(true);
            Timing.RunCoroutine(_GameOver().CancelWith(gameObject), Segment.Update);
        }

        IEnumerator<float> _GameOver()
        {
            float timer = 0;
            while (timer < 0.5f)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime); 
                float glowDissolveFadeValue = Mathf.Lerp(0, 1, timer / 0.5f);
                gameOverScreenMaterial.SetFloat(glowDissolveID, glowDissolveFadeValue);
                timer += Time.deltaTime;
            }
            gameOverScreenMaterial.SetFloat(glowDissolveID, 1);

            timer = 0;

            while (timer < 0.5f)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime);
                float glowDissolveFadeValue = Mathf.Lerp(0, 1, timer / 0.5f);
                deadPlayerSprites[0].material.SetFloat(glowDissolveID, glowDissolveFadeValue);
                timer += Time.deltaTime;
            }
            deadPlayerSprites[0].material.SetFloat(glowDissolveID, 1);

            ghostPlayer.SetActive(true);
        }

        [Button("GameWon")]
        public void GameWon()
        {
            gameWonScreen.SetActive(true);
            Timing.RunCoroutine(_GameWon().CancelWith(gameObject), Segment.Update);
        }

        IEnumerator<float> _GameWon()
        {
            float timer = 0;
            while (timer < 0.5f)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime);
                float alphaDissolveFadeValue = Mathf.Lerp(0, alphaDissolveFadeMaxValue, timer / 0.5f);
                gameWonScreenMaterial.SetFloat(alphaDissolveID, alphaDissolveFadeValue);
                timer += Time.deltaTime;
            }

            gameOverScreenMaterial.SetFloat(glowDissolveID, alphaDissolveFadeMaxValue);

            timer = 0;

            while (timer < 0.5f)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime);
                float glowDissolveFadeValue = Mathf.Lerp(0, 1, timer / 0.5f);
                winningPlayerSprites[0].material.SetFloat(glowDissolveID, glowDissolveFadeValue);
                timer += Time.deltaTime;
            }
            winningPlayerSprites[0].material.SetFloat(glowDissolveID, 1);
        }
    }
}
