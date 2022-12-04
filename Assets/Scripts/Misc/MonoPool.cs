using System.Collections.Generic;
using UnityEngine;

namespace MineMiner
{
    public class MonoPool<T> where T : Component
    {
        private readonly List<T> _used = new List<T>();
        private readonly List<T> _free = new List<T>();
            
        private readonly Transform _parent;
        private readonly T _prefab;

        public MonoPool(Transform parent, T prefab = null)
        {
            this._parent = parent;
            if (prefab != null) this._prefab = prefab;
        }

        public T GetObject()
        {
            T component;
            if (_free.Count == 0)
            {
                if (_prefab == null)
                {
                    component = new GameObject().AddComponent<T>();
                    component.transform.SetParent(_parent, false);

                    _used.Add(component);
                    return component;
                }
                else
                {
                    GameObject newObject = Object.Instantiate(_prefab.gameObject, _parent);
                    component = newObject.GetComponent<T>();
                    _used.Add(component);
                    return component;
                }
            }

            component = _free[0];
            _free.Remove(component);
            component.gameObject.SetActive(true);
            _used.Add(component);
            return component;
        }


        public void ReleaseObject(T obj)
        {
            _used.Remove(obj);
            obj.gameObject.SetActive(false);
            _free.Add(obj);
        }

        public void Dispose()
        {
            _free.Clear();
            _used.Clear();
        }

        public bool ContainsItem(T item)
        {
            return _used.Contains(item) || _free.Contains(item);
        }
    }
}
