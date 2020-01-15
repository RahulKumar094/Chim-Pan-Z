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

	public void CollidedAtPoint(Vector3 point)
	{
		DisablePoolObject();
		ParticleManager.Instance.CreateHitParticlesAt(point);
	}

}