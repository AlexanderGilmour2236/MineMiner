namespace MineMiner
{
    public class MineMinerApp : App
    {
        private SceneNavigator _sceneNavigator;
        private Player _player;
        
        public override void StartGame(SceneType startScene)
        {
            base.StartGame(startScene);

            _player = LoadPlayer();
            if (startScene == SceneType.MINE_SCENE)
            {
                _sceneNavigator = new MineSceneNavigator(SceneNames.MINE_SCENE, _player);
                _sceneNavigator.Go(); 
            }
            else
            {
                _sceneNavigator = new MapEditorNavigator(SceneNames.MAP_EDITOR_SCENE);
                _sceneNavigator.Go();
            }
            
        }

        private Player LoadPlayer()
        {
            Player player = new Player();
            player.LoadPlayerData();
            return player;
        }
    }
}