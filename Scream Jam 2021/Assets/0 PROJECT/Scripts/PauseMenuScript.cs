using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScreamJam
{
    public class PauseMenuScript : MonoBehaviour
    {
        PlayerInput player;

        [SerializeField] bool _paused = false;
        [SerializeField] GameObject pauseMenu;
        [SerializeField] GameObject controlsText;
        [SerializeField] GameObject resumeButton;
        [SerializeField] GameObject restartButton;
        [SerializeField] GameObject controlsButton;
        [SerializeField] GameObject menuButton;
        [SerializeField] GameEvent ePaused;
        [SerializeField] GameEvent eUnpaused;
        [SerializeField] GameManager manager;

        private void Awake()
        {
            player = new PlayerInput();

            player.input.Pause.performed += ctx => PauseUnpause();
        }

        public void PauseUnpause()
        {
            if (manager.levelCompleted || manager.levelLost)
                return;

            if (!_paused)
            {
                _paused = true;
                pauseMenu.SetActive(true);
                ePaused.Raise();
                Time.timeScale = 0f;                
            }
            else
            {
                Time.timeScale = 1f;
                eUnpaused.Raise();
                _paused = false;
                pauseMenu.SetActive(false);
                ResetPauseMenu();
            }
        }

        void ResetPauseMenu()
        {
            resumeButton.SetActive(true);
            restartButton.SetActive(true);
            controlsButton.SetActive(true);
            menuButton.SetActive(true);
            controlsText.SetActive(false);
        }

        private void OnEnable()
        {
            player.Enable();
        }

        private void OnDisable()
        {
            player.Disable();
        }
    }
}
