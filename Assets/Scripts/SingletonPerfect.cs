using UnityEngine;
using System.Collections;


public class SingletonPerfect<T> : MonoBehaviour where T : SingletonPerfect<T> 
{
	private static 	T mInstance;

	public static 	T Instance {
		get {
			if(mInstance == null) {
				T[] instances = GameObject.FindObjectsOfType(typeof(T)) as T[];
				if(instances.Length != 0) {
					if(instances.Length == 1) {
						mInstance = instances[0];
						mInstance.gameObject.name = typeof(T).Name;
						return mInstance;
					}
					else {
						Debug.LogError("More than one instance of " + typeof(T).Name + " in scene.");
						foreach(T inst in instances)
							Destroy(inst.gameObject);
					}
				}

				GameObject go = new GameObject(typeof(T).Name, typeof(T));
				mInstance = go.GetComponent<T>();
				DontDestroyOnLoad(go);
			}
			return mInstance;
		}

		set { mInstance = value as T; }

	}
}
