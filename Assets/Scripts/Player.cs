using ResourcesProvider;
using UnityEngine;

namespace MineMiner
{
    public class Player
    {
        private PlayerResources _playerResources = new PlayerResources();
        
        public Player()
        {
            Damage = 5f;
        }

        public void LoadPlayerData()
        {
            _playerResources.AddResource(BlockId.Golds, PlayerPrefs.GetInt(PlayerPrefsKeys.PlayersGolds));
        }
        
        public void SavePlayerData()
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.PlayersGolds, _playerResources.GetPlayerResource(BlockId.Golds).Amount);
        }
        
        public float Damage { get; set; }

        public PlayerResources PlayerResources
        {
            get { return _playerResources; }
        }
    }
}