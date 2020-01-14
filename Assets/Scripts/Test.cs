using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public GameObject Orb;
	public Transform ObjForPosition;
	public int Count = 10;
	public float MoveTimeAfterBurst = 3;
	public float Speed = 5;

	private GameObject[] AllOrbs;
	private float Timer;

	private void Start()
	{
		AllOrbs = new GameObject[Count];

		for (int i = 0; i < AllOrbs.Length; i++)
		{
			AllOrbs[i] = Instantiate(Orb);
		}
	}
	
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			//BurstAt(ObjForPosition.position);
		}
	}

	
	
}
