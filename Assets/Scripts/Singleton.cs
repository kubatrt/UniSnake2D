using UnityEngine;
using System.Collections;


public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
	
	private static T _instance;


	public static T Instance {
		get {
			if(_instance == null) 
			{
				GameObject go = new GameObject(typeof(T).Name, typeof(T));
				_instance = go.GetComponent<T>();
			}
			return _instance;
		}
	}


	public void OnApplicationQuit()
	{
		DestroyInstance();
	}
	
	public void DestroyInstance() 
	{
		_instance = null;
	}
}
