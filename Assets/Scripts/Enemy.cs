using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolObject
{
	public Transform LaserShooter;
	private float mTimer;

    void Start()
    {
		mTimer = 0;
	}

	private void Animate()
	{
		mTimer += Time.deltaTime;
		float f = (Mathf.Sin(mTimer * 5) + 1) / 4 + 0.5f;
		transform.localScale = new Vector3(f, f, f);
	}

	public void Kill()
	{
		ParticleManager.Instance.CreateRedOrbs(10, transform.position);
		DisablePoolObject();
	}

	public void KillPlayer()
	{

	}

	public void Shoot()
	{
		int chance = Random.Range(0, 10);
		if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) >= 15f)
		{
			GameObject laser = SpawnPool.Instance.GetLaserFromPool();
			if (laser != null)
				laser.transform.position = LaserShooter.position;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Player")
		{
			//ParticleManager.Instance.CreateBurstParticlesAt(transform.position);
			DisablePoolObject();
		}
	}

}
