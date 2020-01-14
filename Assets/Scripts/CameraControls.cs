using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
	[HideInInspector]
	public float CameraSpeed;
	private PlayerController mPlayer;
	private float CameraNormalSpeed;	

    void Awake()
    {
		mPlayer = PlayerController.Instance;
		if (mPlayer != null)
		{
			CameraNormalSpeed = mPlayer.MoveSpeed;
		}
	}
	
    void LateUpdate()
    {
		transform.position += new Vector3(0, 0, CameraSpeed * Time.deltaTime);
	}

	public void ResetCamera()
	{
		CameraSpeed = CameraNormalSpeed;
	}
}
