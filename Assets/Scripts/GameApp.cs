using Zenject;

namespace MineMiner
{
    public class GameApp : App
    {
        [Inject] private MineSceneNavigator _mineSceneNavigator;
        
        private Player _player;

        public override void StartGame()
        {
            base.StartGame();
            _player = new Player();
            _mineSceneNavigator.Go();
        }
        
        public Player Player
        {
            get { return _player; }
        }
    }
}