using System.Collections.Generic;
using UnityEngine;

namespace MineMiner
{
    public class MonoPool<T> where T : Component
    {
        private readonly Stack<T> used = new Stack<T>();
        private readonly Stack<T> free = new Stack<T>();
            
        private readonly Transform parent;
        private readonly T prefab;
        public MonoPool(Transform parent, T prefab = null)
        {
            this.parent = parent;
            if (prefab != null) this.prefab = prefab;
        }

        public T GetObject()
        {
            T component;
            if (free.Count == 0)
            {
                if (prefab == null)
                {
                    component = new GameObject().AddComponent<T>();
                    component.transform.SetParent(parent, false);

                    used.Push(component);
                    return component;
                }
                else
                {
                    GameObject newObject = Object.Instantiate(prefab.gameObject, parent);
                    component = newObject.GetComponent<T>();
                    used.Push(component);
                    return component;
                }
            }

            component = free.Pop();
            component.gameObject.SetActive(true);
            used.Push(component);
            return component;
        }


        public void ReleaseObject(T obj)
        {
            used.Pop();
            obj.gameObject.SetActive(false);
            free.Push(obj);
        }

        public void Dispose()
        {
            free.Clear();
            used.Clear();
        }
    }
}
