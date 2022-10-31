namespace ResourcesProvider
{
    public class PlayerResource
    {
        private int _id;
        private int _amount;

        public PlayerResource(int resourceId, int amount)
        {
            _id = resourceId;
            _amount = amount;
        }

        public int ID
        {
            get { return _id; }
        }

        public int Amount
        {
            set { _amount = value;}
            get { return _amount; }
        }

    }
}