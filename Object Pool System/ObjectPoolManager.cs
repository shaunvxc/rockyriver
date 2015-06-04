using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ObjectPoolManager
// Author: William Ravaine - spk02@hotmail.com (Spk on Unity forums)
// Date: 15-11-09
//
// <LEGAL BLABLA>
// This code package is provided "as is" with no express or implied warranty of any kind. You may use this code for both
// commercial and non-commercial use without any restrictions. Any modification and/or redistribution of this code should
// include the original author's name, contact information and also this paragraph.
// </LEGAL BLABLA>
//
// The goal of this class is to avoid costly runtime memory allocation for objects that are created and destroyed very often during
// gameplay (e.g. projectile, enemies, etc). It achieves this by recycling "destroyed" objects from an internal cache instead of physically
// removing them from memory via Object.Destroy().
//
// To use the ObjectPoolManager, you simply need to replace your regular object creation & destruction calls by ObjectPoolManager.CreatePooled()
// and ObjectPoolManager.DestroyPooled(). Here's an exemple:
//
// 1) Without using the ObjectPoolManager:
// Projectile bullet = Instanciate( bulletPrefab, position, rotation ) as Projectile;
// Destroy( bullet.gameObject );
// 
// 2) Using the ObjetPoolManager:
// Projectile bullet = ObjectPoolManager.CreatePooled( bulletPrefab.gameObject, position, rotation ).GetComponent<Bullet>();
// ObjectPoolManager.DestroyPooled( bullet.gameObject );
//
// When a recycled object is revived from the cache, the ObjectPoolManager calls its Start() method again, so this object can reset itself as
// if it just got newly created.
//
// When using the ObjectPoolManager with your objects, you need to keep several things in mind:
// 1. You need to be in full control of the creation and destruction of the object (so they go through ObjectPoolManager). This means you shouldn't
//	  use it on objects that use exotic destruction methods (e.g. auto-destroy option on particle effects) because the ObjectPoolManager will
//	  not be able to recycle the object 
// 2. When they get revived from the ObjectPoolManager cache, the pooled objects are responsible for re-initializing themselves as if they had
//	  just been newly created via a regular call Instantiate(). So look out for any dynamic component additions and modifications of the initial
//	  object public fields during gameplay

public class ObjectPoolManager : MonoBehaviour
{
	// Only one ObjectPoolManager can exist. We use a singleton pattern to enforce this.
	#region Singleton Access

	static ObjectPoolManager instance = null;
	public static ObjectPoolManager Instance
	{
		get
		{
			if( !instance )
			{
				// check if an ObjectPoolManager is already available in the scene graph
				instance = FindObjectOfType( typeof( ObjectPoolManager ) ) as ObjectPoolManager;

				// nope, create a new one
				if( !instance )
				{
					GameObject obj = new GameObject( "ObjectPoolManager" );
					instance = obj.AddComponent<ObjectPoolManager>();
				}
			}

			return instance;
		}
	}

	void OnApplicationQuit()
	{
		// release reference on exit
		instance = null;
	}

	#endregion

	#region Public fields

	// turn this on to activate debugging information
	public bool debug = false;

	// the GUI block where the debugging info will be displayed
	public Rect debugGuiRect = new Rect( 5, 200, 160, 400 );

	#endregion

	#region Private fields

	// This maps a prefab to its ObjectPool
	Dictionary<GameObject, ObjectPool> prefab2pool;

	// This maps a game object instance to the ObjectPool that created/recycled it
	Dictionary<GameObject, ObjectPool> instance2pool;

	#endregion

	#region Public Interface (static for convenience)

	// Create a pooled object. This will either reuse an object from the cache or allocate a new one
	public static GameObject CreatePooled( GameObject prefab, Vector3 position, Quaternion rotation )
	{
		return Instance.InternalCreate( prefab, position, rotation );
	}

	// Destroy the object now
	public static void DestroyPooled( GameObject obj )
	{
		Instance.InternalDestroy( obj );
	}


	// Destroy the object after <delay> seconds have elapsed
	public static void DestroyPooled( GameObject obj, float delay )
	{
		Instance.StartCoroutine( Instance.InternalDestroy( obj, delay ) );
	}

	#endregion

	#region Private implementation

	// Constructor
	void Awake()
	{
		prefab2pool = new Dictionary<GameObject, ObjectPool>();
		instance2pool = new Dictionary<GameObject, ObjectPool>();
	}

	private ObjectPool CreatePool( GameObject prefab )
	{
		GameObject obj = new GameObject( prefab.name + " Pool" );
		ObjectPool pool = obj.AddComponent<ObjectPool>();
		pool.Prefab = prefab;
		return pool;
	}

	private GameObject InternalCreate( GameObject prefab, Vector3 position, Quaternion rotation )
	{
		ObjectPool pool;

		if( !prefab2pool.ContainsKey( prefab ) )
		{
			pool = CreatePool( prefab );
			pool.gameObject.transform.parent = this.gameObject.transform;
			prefab2pool[prefab] = pool;
		}
		else
		{
			pool = prefab2pool[prefab];
		}

		// create a new object or reuse an existing one from the pool
		GameObject obj = pool.Instanciate( position, rotation );

		// remember which pool this object was created from
		instance2pool[obj] = pool;

		return obj;
	}

	private void InternalDestroy( GameObject obj )
	{
		if( instance2pool.ContainsKey( obj ) )
		{
			//Debug.Log( "Recyling object " + obj.name );
			ObjectPool pool = instance2pool[obj];
			pool.Recycle( obj );
		}
		else
		{
			// This object was not created through the ObjectPoolManager, give a warning and destroy it the "old way"
			Debug.LogWarning( "Destroying non-pooled object " + obj.name );
			Object.Destroy( obj );
		}
	}

	// must be run as coroutine
	private IEnumerator InternalDestroy( GameObject obj, float delay )
	{
		yield return new WaitForSeconds( delay );
		InternalDestroy( obj );
	}

	#endregion
	/*
	void OnGUI()
	{

		Debug.Log ("Object Pool Manager Got here");
		if( debug )
		{
			GUILayout.BeginArea( debugGuiRect );
			GUILayout.BeginVertical();

			GUILayout.Label( "Pools: " + prefab2pool.Count );

			foreach( ObjectPool pool in prefab2pool.Values )
				GUILayout.Label( pool.Prefab.name + ": " + pool.Count );

			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
	*/
}