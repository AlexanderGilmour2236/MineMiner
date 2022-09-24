using UnityEngine;

namespace MineMiner
{
    public class Navigator
    {
        protected Navigator _parentNavigator;
        protected bool _isControllersInitiated;
        
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

        public virtual void SetParentNavigator(Navigator parentNavigator)
        {
            _parentNavigator = parentNavigator;
        }

        public virtual void Tick()
        {
        }

        public Navigator parentNavigator
        {
            get { return _parentNavigator; }
        }
    }
}