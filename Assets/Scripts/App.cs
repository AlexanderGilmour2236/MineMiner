using Zenject;

namespace MineMiner
{
    public class App : ITickable
    {
        [Inject] private MineSceneNavigator _mineSceneNavigator;
        
        private static App _instance;
        private Navigator _activeNavigator;
        private Player _player;

        public void StartGame()
        {
            _player = new Player();
            _mineSceneNavigator.Go();
        }

        public App()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public static App Instance()
        {
            if (_instance == null)
            {
                _instance = new App();
            }

            return _instance;
        }

        public void Tick()
        {
            _activeNavigator.Tick();
        }

        public void SetActiveNavigator(Navigator navigator)
        {
            _activeNavigator = navigator;
        }

        public Player Player
        {
            get { return _player; }
        }
    }
}