using UnityEngine;

namespace MineMiner
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private SceneType _startScene;
        
        private void Start()
        {
            Application.targetFrameRate = 60;
            App.Instance().StartGame(_startScene);
        }

        private void Update()
        {
            App.Instance().Tick();
            if (Input.GetKeyUp(KeyCode.R))
            {
                App.RestartApp();
            }
        }
    }
}