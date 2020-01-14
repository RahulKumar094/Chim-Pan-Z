using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : PoolObject
{
	public float MoveSpeed = 8;

    protected override void Update()
    {
		base.Update();
		transform.position += new Vector3(0, 0, -MoveSpeed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Player")
		{
			DisablePoolObject();
			ParticleManager.Instance.CreateParticle(ParticleType.SmallSmoke, collision.contacts[0].point);
		}
	}

}