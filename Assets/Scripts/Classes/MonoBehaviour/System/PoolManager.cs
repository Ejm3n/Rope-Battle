using System.Collections.Generic;
using UnityEngine;



public class PoolManager : MonoBehaviour
{
    [System.Flags]
    public enum PoolPopInfo
    {
        failure = 0,
        done = 1,
        force = 2
    }
    public interface IPoolObject
    {
        string Specifier { get; }
        string PoolTag { get; }
        bool InPool { get; set; }
        GameObject GameObject { get; }
        void Pop();
        void Push();
        void SetParent(Transform parent);
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        void SetScale(Vector3 scale);
    }

    public class Pool
    {
        protected readonly Queue<IPoolObject> objects;
        protected GameObject prefab;
        protected Transform root;
        public Pool(GameObject prefab, Transform root)
        {
            this.prefab = prefab;
            this.root = root;
            objects = new Queue<IPoolObject>();
        }
        public bool Push(IPoolObject poolObject)
        {
            if (!poolObject.InPool)
            {
                AddPoolObject(poolObject, Vector3.zero, Quaternion.identity);
                return true;
            }
            else
                return false;
        }
        public IPoolObject Pop(Vector3 position, Quaternion rotation)
        {
            if (objects.Count > 0)
                return GetPoolObject(position, rotation);
            else
                return CreatePoolObject(position, rotation);            
        }
        private void AddPoolObject(IPoolObject poolObject, Vector3 position, Quaternion rotation)
        {
            poolObject.InPool = true;
            poolObject.GameObject.SetActive(false);
            poolObject.SetParent(root);
            poolObject.SetPosition(position);
            poolObject.SetRotation(rotation);
            poolObject.Push();
            objects.Enqueue(poolObject);
        }
        private IPoolObject GetPoolObject(Vector3 position, Quaternion rotation)
        {
            IPoolObject poolObject = objects.Dequeue();
            poolObject.InPool = false;
            poolObject.SetPosition(position);
            poolObject.SetRotation(rotation);
            poolObject.Pop();
            poolObject.GameObject.SetActive(true);
            return poolObject;
        }
        private IPoolObject CreatePoolObject(Vector3 position, Quaternion rotation)
        {
            GameObject go = Instantiate(prefab, position, rotation, root);
            go.name = go.name + go.GetInstanceID().ToString();
            IPoolObject poolObject = go.GetComponent<IPoolObject>();
            poolObject.InPool = false;
            poolObject.Pop();
            poolObject.GameObject.SetActive(true);
            return poolObject;
        }
    }

    public Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
    #region Singleton

    private static PoolManager _default;
    public static PoolManager Default => _default;
    #endregion
    private void Awake()
    {
        _default = this;
    }
    public bool AddPool(IPoolObject prefab)
    {
        if (prefab != null && !pools.ContainsKey(prefab.PoolTag) && prefab.PoolTag.Length > 0)
        {
            GameObject prefabGO = prefab.GameObject;
            Transform root = new GameObject(prefab.PoolTag).transform;
            root.parent = transform;
            Pool pool = new Pool(prefabGO, root);
            pools.Add(prefab.PoolTag, pool);
            return true;
        }
        return false;
    }
    public void Push(IPoolObject poolObject, bool addPoolOnFailure = true)
    {
        string tag = poolObject.PoolTag;
        Pool pool;
        if (pools.TryGetValue(tag, out pool))
        {
            pool.Push(poolObject);
        }
        else
        {
            if (addPoolOnFailure && AddPool(poolObject))
            {
                Push(poolObject, addPoolOnFailure);
            }
            else
            {
                poolObject.GameObject.SetActive(false);
                poolObject.Push();
            }
        }
    }
    public IPoolObject Pop(IPoolObject poolObject,Vector3 position, Quaternion rotation, bool addPoolOnFailure = true)
    {
        var tag = poolObject.PoolTag;
        Pool pool;
        if (pools.TryGetValue(tag, out pool))
        {
            return pool.Pop(position, rotation);
        }
        else
        {
            if (addPoolOnFailure && AddPool(poolObject))
            {
                return Pop(poolObject, position, rotation, addPoolOnFailure);
            }
            return null;
        }
    }
}