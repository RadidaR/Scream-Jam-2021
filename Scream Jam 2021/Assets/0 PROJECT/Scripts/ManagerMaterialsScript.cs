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

        [SerializeField] SpriteRenderer[] deadPlayerSprites;

        [SerializeField] GameObject ghostPlayer;

        private void Awake()
        {
            gameOverScreen.GetComponent<Image>().material = new Material(gameOverScreen.GetComponent<Image>().material);
            gameOverScreenMaterial = gameOverScreen.GetComponent<Image>().material;

            glowDissolveID = Shader.PropertyToID("_FullGlowDissolveFade");
            deadPlayerSprites[0].material = new Material(deadPlayerSprites[0].material);
            Material player = deadPlayerSprites[0].material;

            foreach (SpriteRenderer sprite in deadPlayerSprites)
            {
                sprite.material = player;
            }

        }
        [Button("GameOver")]
        public void GameOver()
        {
            gameOverScreen.SetActive(true);
            Timing.RunCoroutine(_GameOver(), Segment.Update);
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
    }
}
