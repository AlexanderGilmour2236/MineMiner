using System.Collections.Generic;
using UnityEngine;

namespace MineMiner
{
    public class MonoPool<T> where T : Component
    {
        private readonly Stack<T> _used = new Stack<T>();
        private readonly Stack<T> _free = new Stack<T>();
            
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

                    _used.Push(component);
                    return component;
                }
                else
                {
                    GameObject newObject = Object.Instantiate(_prefab.gameObject, _parent);
                    component = newObject.GetComponent<T>();
                    _used.Push(component);
                    return component;
                }
            }

            component = _free.Pop();
            component.gameObject.SetActive(true);
            _used.Push(component);
            return component;
        }


        public void ReleaseObject(T obj)
        {
            _used.Pop();
            obj.gameObject.SetActive(false);
            _free.Push(obj);
        }

        public void Dispose()
        {
            _free.Clear();
            _used.Clear();
        }
    }
}
