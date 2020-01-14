using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : PoolObject
{
	public int Value = 5;
	private bool mPullBack;
	private Rigidbody Rigidbody;

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
	}

	private void OnEnable()
	{
		mPullBack = false;
		Invoke("PullBack", 0.5f);
	}

	protected override void Update()
	{
		base.Update();

		if (mPullBack)
		{
			transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.HipPosition, 0.5f);
		}
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			DisablePoolObject();
		}
	}

	private void PullBack()
	{
		mPullBack = true;
	}
}
