using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : PoolObject
{
	public int Value = 5;

	private void OnEnable()
	{
		Invoke("PullBack", 0.5f);
	}

	private void PullBack()
	{
		StartCoroutine("PullBackMech");
	}

	private IEnumerator PullBackMech()
	{
		float dist = Vector3.Distance(transform.position, PlayerController.Instance.HipPosition);
		while (dist > 0.5f)
		{
			transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.HipPosition, 0.5f);
			dist = Vector3.Distance(transform.position, PlayerController.Instance.HipPosition);

			if (dist < 0.5f)
			{
				StopCoroutine("PullBackMech");
				DisablePoolObject();
			}

			yield return new WaitForEndOfFrame();
		}
	}
}
