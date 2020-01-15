using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
	public Vector3 Size
	{
		get { return Detector.size; }
	}

	private BoxCollider Detector;

	private void Awake()
	{
		Detector = GetComponent<BoxCollider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Target"))
		{
			PlayerController.Instance.SetEnemyInProximity(other.gameObject);
		}
		else if (other.CompareTag("Laser"))
		{
			PlayerController.Instance.SetLaserInProximity(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Target")
		{
			PlayerController.Instance.SetEnemyInProximity(null);
		}
		else if (other.CompareTag("Laser"))
		{
			PlayerController.Instance.SetLaserInProximity(null);
		}
	}
}
