using System;
using UnityEngine;

namespace RTC
{
	public class CircularObjectPool : IDisposable
	{
		#region Variables
		private GameObject[] _pool;
		private GameObject _parent;

		private int _pointer;
		#endregion

		#region Methods
		public CircularObjectPool(int capacity, GameObject prefab)
		{
			_parent = new GameObject("CircularObjectPool");

			_pool = new GameObject[capacity];
			for (int i = 0; i < capacity; i++)
			{
				GameObject obj = GameObject.Instantiate(prefab);
				_pool[i] = obj;
				Pool(obj);
			}
		}

		public GameObject GetObject()
		{
			for (int i = 0; i < _pool.Length; i++)
			{
				if (!_pool[i].activeInHierarchy)
				{
					_pool[i].SetActive(true);
					
					return _pool[i];
				}
			}

			_pointer = _pointer % _pool.Length;

			return _pool[_pointer++];
		}

		public void Pool(GameObject obj)
		{
			obj.SetActive(false);
			obj.transform.SetParent(_parent.transform, false);
		}

		public void Clear()
		{
			for (int i = 0; i < _pool.Length; i++)
			{
				Pool(_pool[i]);	
			}
		}

		public void Dispose()
		{
			for (int i = 0; i < _pool.Length; i++)
			{
				UnityEngine.Object.Destroy(_pool[i]);
			}
			UnityEngine.Object.Destroy(_parent);
		}
		#endregion
	}
}
