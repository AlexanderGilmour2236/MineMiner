using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MineMiner
{
    public class Loader : MonoBehaviour
    {
        private void Start()
        {
            App.Instance().StartGame();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                App.RestartApp();
            }
        }
    }
}