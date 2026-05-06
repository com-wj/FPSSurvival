using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
	private readonly Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>(); // 다중 풀

	private readonly Dictionary<GameObject, Transform> _roots = new Dictionary<GameObject, Transform>(); // 하이어라키 정리용 트랜스폼

	public void Push(GameObject prefab, GameObject go)
	{
		if (go == null) return;

		go.SetActive(false);

		if (!_pools.ContainsKey(prefab))
		{
			_pools.Add(prefab, new Queue<GameObject>());
			CreateRoot(prefab);
		}

		go.transform.SetParent(_roots[prefab]);
		_pools[prefab].Enqueue(go);
	}

	public GameObject Pop(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (!_pools.ContainsKey(prefab))
		{
			_pools.Add(prefab, new Queue<GameObject>());
			CreateRoot(prefab);
		}

		GameObject go;

		if (_pools[prefab].Count > 0)
		{
			go = _pools[prefab].Dequeue();
			go.transform.position = position;
			go.transform.rotation = rotation;
			go.transform.SetParent(null);
			go.SetActive(true);
		}
		else
		{
			go = Instantiate(prefab, position, rotation);
			// go.Init(); 생성 시점에 originPrefab 주입.
		}

		return go;
	}

	// 하이어라키 루트 트랜스폼 생성
	private void CreateRoot(GameObject prefab)
	{
		if (_roots.ContainsKey(prefab)) return;

		GameObject root = new GameObject($"pool_{prefab.name}");
		_roots.Add(prefab, root.transform);
	}
}