using Zenject;

namespace MineMiner
{
    public class MineMinerApp : App
    {
        [Inject] private MineSceneNavigator _mineSceneNavigator;


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