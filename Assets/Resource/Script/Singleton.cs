using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	[SerializeField] private bool _dontDestroyOnLoad = false;

	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.LogError($"[{typeof(T).Name}] 인스턴스가 존재하지 않습니다.");
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this as T;

		if (_dontDestroyOnLoad)
		{
			DontDestroyOnLoad(gameObject);
		}
	}

	protected virtual void OnDestroy()
	{
		if (_instance == this as T)
		{
			_instance = null;
		}
	}
}