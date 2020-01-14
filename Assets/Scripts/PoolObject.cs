using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
	private float mDestroyDistanceBehindPlayer = 10f;

	protected virtual void Update()
	{
		if (transform.position.z - PlayerController.Instance.transform.position.z < -mDestroyDistanceBehindPlayer)
			DisablePoolObject();
	}

	protected virtual void DisablePoolObject()
	{
		gameObject.SetActive(false);
	}

}
