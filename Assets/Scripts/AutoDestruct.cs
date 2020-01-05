using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    public void Destruct(float time)
    {
		Invoke("DisableObject", time);
    }

	private void DisableObject()
	{
		gameObject.SetActive(false);
	}
}
