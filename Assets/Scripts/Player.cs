using System.Runtime.Serialization;

namespace MineMiner
{
    public class Player
    {
        public Player()
        {
            Damage = 1f;
        }
        
        public float Damage { get; set; }
    }
}