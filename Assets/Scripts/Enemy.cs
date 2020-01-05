using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Transform _LaserShooter;
	public GameObject _PfLaser;
	private float mTimer;

    void Start()
    {
		mTimer = 0;
	}

	private void OnEnable()
	{
		float ShootAfter = Random.Range(0, 7f);
		Invoke("Shoot", ShootAfter);
	}

	void Update()
    {
		if (transform.position.z - PlayerController.Instance.transform.position.z < -10f)
			DisableEnemy();
	}

	private void Animate()
	{
		mTimer += Time.deltaTime;
		float f = (Mathf.Sin(mTimer * 5) + 1) / 4 + 0.5f;
		transform.localScale = new Vector3(f, f, f);
	}

	public void Kill()
	{
		ParticleManager.Instance.CreateBurstParticlesAt(transform.position);
		DisableEnemy();
	}

	public void Shoot()
	{
		GameObject laser = Instantiate(_PfLaser);
		laser.transform.position = _LaserShooter.position;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Player")
		{
			ParticleManager.Instance.CreateBurstParticlesAt(transform.position);
			DisableEnemy();
		}
	}

	private void DisableEnemy()
	{
		gameObject.SetActive(false);
	}
}
