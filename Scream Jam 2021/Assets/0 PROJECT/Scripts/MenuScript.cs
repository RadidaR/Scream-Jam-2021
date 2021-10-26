using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreamJam
{
    public class MenuScript : MonoBehaviour
    {
        public void LoadScene(int sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }
}
