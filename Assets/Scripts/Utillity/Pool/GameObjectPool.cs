using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using Utility.Internal;
using Debugger = UnityEngine.Debugger;


namespace Utillity.Pool {

	/// <summary>
	/// 一种资源一种池;
	/// </summary>
	public class GameObjectPool {
        private int limitAmount;
        private Transform poolRoot;
        private GameObject prefab;
        private Stack<GameObject> cacheStack = new Stack<GameObject>();

        /// <summary>
        /// 最多缓存数量;
        /// </summary>
        private int cullAbove;
        /// <summary>
        /// 自动Destroy间隔;
        /// </summary>
        private float cullDelay;
        /// <summary>
        /// 自动Destroy数量;
        /// </summary>
        private uint cullMaxPerPass;
        
        public bool cullingActive = false;
        private float cullTime = 0;

        public float startCullDelay = 0;
        
        /// <summary>
        /// 实例化、销毁都是对象池完成的，不考虑外部销毁的情况，理论也不应该存在这种情况;
        /// </summary>
        private int totalInstanceCnt = 0;

        public GameObjectPool(GameObject prefab, int cullAbove, float cullDelay, uint cullMaxPerPass, int limitAmount, int initCount)
        {
	        SetParams(prefab, cullAbove, cullDelay, cullMaxPerPass, limitAmount);
            
	        poolRoot = RootUtility.poolRootTran;
	        startCullDelay = this.cullDelay;
            for(int index = 0; index < initCount; index++) {
				AddObjectToPool(NewObjectInstance());
			}
		}
        
        public void SetParams(GameObject prefab, int cullAbove, float cullDelay, uint cullMaxPerPass, int limitAmount)
        {
	        this.prefab = prefab;
			
	        this.cullAbove = cullAbove;
	        this.cullDelay = cullDelay;
	        this.cullMaxPerPass = cullMaxPerPass;
	        this.limitAmount = limitAmount;
        }

		//o(1)
        private void AddObjectToPool(GameObject go) {
	        if(poolRoot == null)
		        return;
	        
	        //add to pool
            go.SetActive(false);
            cacheStack.Push(go);
            go.transform.SetParent(poolRoot, false);

            if (!cullingActive && cacheStack.Count > cullAbove)
            {
	            cullingActive = true;
	            cullTime = Time.time + startCullDelay;
            }
		}

        private GameObject NewObjectInstance()
        {
	        if (prefab == null)
	        {
		        Debugger.LogError("@========== pool templete prefab is null ~~~");
		        return null;
	        }
	        
	        totalInstanceCnt++;
	        // Debugger.LogErrorFormat("@========== key:{0} init ~~~", prefab.name);
	        return GameObject.Instantiate(prefab);
		}

		public GameObject NextAvailableObject() {
			if(cacheStack.Count > 0) 
			{
				GameObject gameObj = cacheStack.Pop();
				gameObj.SetActive(true);
				return gameObj;
			}

			GameObject go = NewObjectInstance();
			//go.SetActive(false);
			
			if (totalInstanceCnt > limitAmount)
			{
	            #if UNITY_EDITOR
				Debugger.LogWarningFormat("CLONE {0} limitAmount:{1} totalCnt:{2}", prefab.name, limitAmount, totalInstanceCnt);
				#endif
			}
			
			// 用于自己设置;
            go.SetActive(true);
            return go;
		} 
		
		//o(1)
        public void ReturnObjectToPool(GameObject po) {
	        AddObjectToPool(po);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public void CullingUpdate(float curTime)
        {
	        if (curTime < cullTime)
		        return;

	        int cnt = 0;
	        cullTime += cullDelay;
	        while (cacheStack.Count > cullAbove)
	        {
		        GameObject.Destroy(cacheStack.Pop());
		        totalInstanceCnt--;

		        // Debugger.LogErrorFormat("@========== key:{0} count:{1} destroy ~~~", prefab.name, cacheStack.Count);
		        cnt++;
		        if (cnt >= cullMaxPerPass)
		        {
			        break;
		        }
	        }

	        if (cacheStack.Count <= cullAbove)
	        {
		        cullingActive = false;
	        }
        }

        public void ClearAllCache()
        {
	        while (cacheStack.Count > 0)
	        {
		        GameObject.Destroy(cacheStack.Pop());
	        }
	        totalInstanceCnt = 0;
        }

        public int cacheCount
        {
	        get { return cacheStack.Count; }
        }

        /// <summary>
        /// 是否还有使用;
        /// </summary>
        /// <returns></returns>
        public bool IsUsing()
        {
	        return cacheStack.Count == totalInstanceCnt;
        }
	}
}
