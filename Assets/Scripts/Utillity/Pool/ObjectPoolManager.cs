using UnityEngine;
using System.Collections.Generic;
using Utility;

namespace Utillity.Pool {
    /// <summary>
    /// 对象池管理器，分普通类对象池+资源游戏对象池
    /// </summary>
    public class ObjectPoolManager : MonoBehaviourSingle<ObjectPoolManager> {
        private readonly Dictionary<string, GameObjectPool> m_GameObjPoolDict = new Dictionary<string, GameObjectPool>();
        private readonly List<GameObjectPool> m_GameObjPoolList = new List<GameObjectPool>();

        private readonly Dictionary<int, List<GameObjectPool>> m_GameObjMap =
            new Dictionary<int, List<GameObjectPool>>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poolName"></param>
        /// <param name="poolType">对象池大类别，便于管理，0表示无类别</param>
        /// <param name="prefab"></param>
        /// <param name="cullAbove">最大缓存数量</param>
        /// <param name="cullDelay">超出部分自动清除间隔</param>
        /// <param name="cullMaxPerPass">每次清除数量</param>
        /// <param name="limitAmount">最多激活多少个GameObject</param>
        /// <param name="initCount">预先初始化指定数量</param>
        /// <returns></returns>
        public GameObjectPool CreatePool(string poolName, int poolType, GameObject prefab, int cullAbove, float cullDelay = 0.01f, uint cullMaxPerPass = 3, int limitAmount = int.MaxValue, int initCount = 0)
        {
            GameObjectPool pool;
            if (m_GameObjPoolDict.TryGetValue(poolName, out pool))
            {
                pool.SetParams(prefab, cullAbove, cullDelay, cullMaxPerPass, limitAmount);
                return pool;
            }
            
            pool = new GameObjectPool(prefab, cullAbove, cullDelay, cullMaxPerPass, limitAmount, initCount);
            m_GameObjPoolDict[poolName] = pool;
            m_GameObjPoolList.Add(pool);

            if (poolType != 0)
            {
                List<GameObjectPool> list;
                if (!m_GameObjMap.TryGetValue(poolType, out list))
                {
                    list = new List<GameObjectPool>(5);
                    m_GameObjMap.Add(poolType, list);
                }
                list.Add(pool);
            }
            return pool;
        }

        public GameObjectPool GetPool(string poolName) {
            if (m_GameObjPoolDict.ContainsKey(poolName)) {
                return m_GameObjPoolDict[poolName];
            }
            return null;
        }

        public GameObject Get(string poolName) {
            GameObjectPool pool;
            GameObject result = null;
            if (m_GameObjPoolDict.TryGetValue(poolName, out pool)) {
                result = pool.NextAvailableObject();
                if (result == null) {
                    Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
                }
            } else {
                Debug.LogError("Invalid pool name specified: " + poolName);
            }
            return result;
        }

        public void Release(string poolName, GameObject go) {
            GameObjectPool pool;
            if (m_GameObjPoolDict.TryGetValue(poolName, out pool)) {
                pool.ReturnObjectToPool(go);
            } else {
                Debug.LogWarning("No pool available with name: " + poolName);
            }
        }


        protected override void OnUpdate(float deltaTime)
        {
            float curTime = Time.time;
            foreach (GameObjectPool pool in m_GameObjPoolList)
            {
                if (pool.cullingActive)
                {
                    pool.CullingUpdate(curTime);
                }
            }
        }
    }
}