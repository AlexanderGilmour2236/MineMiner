using UnityEngine;

namespace MineMiner
{
    public class Navigator
    {
        private Navigator _parentNavigator;

        public virtual void Go()
        {
            App.Instance().SetActiveNavigator(this);
        }

        public virtual void ExitNavigator()
        {
            if (_parentNavigator != null)
            {
                _parentNavigator.Go();
            }
            else
            {
                Debug.LogError("Last navigator has been exited");
            }
        }

        public virtual void SetNavigator(Navigator parentNavigator)
        {
            _parentNavigator = parentNavigator;
        }

        public virtual void Tick()
        {
        }

        public Navigator ParentNavigator
        {
            get { return _parentNavigator; }
        }
    }
}