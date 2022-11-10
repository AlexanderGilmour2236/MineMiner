using MineMiner;

namespace ResourcesProvider
{
    public class PlayerResource
    {
        private BlockId _blockId;
        private int _amount;

        public PlayerResource(BlockId blockBlockId, int amount)
        {
            _blockId = blockBlockId;
            _amount = amount;
        }

        public BlockId BlockId
        {
            get { return _blockId; }
        }

        public int Amount
        {
            set { _amount = value;}
            get { return _amount; }
        }

    }
}