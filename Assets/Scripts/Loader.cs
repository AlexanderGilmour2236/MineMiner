using UnityEngine;

namespace MineMiner
{
    public class Loader : MonoBehaviour
    {
        private void Start()
        {
            App.Instance().StartGame();
        }
    }
}