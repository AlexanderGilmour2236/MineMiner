using ResourcesProvider;

namespace MineMiner
{
    public class Player
    {
        private PlayerResources _playerResources = new PlayerResources();
        
        public Player()
        {
            Damage = 5f;
        }
        
        public float Damage { get; set; }

        public PlayerResources PlayerResources
        {
            get { return _playerResources; }
        }
    }
}