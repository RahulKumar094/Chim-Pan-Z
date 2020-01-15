using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Target"))
		{
			PlayerController.Instance.HitByEnemy(other);
		}
		else if (other.CompareTag("Laser"))
		{
			PlayerController.Instance.HitByLaser(other);
		}
	}
}
