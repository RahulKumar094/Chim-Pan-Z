using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
	// Start is called before the first frame update
	int x = 5;
    void Start()
    {
		if (x-- > 0)
		{
			Debug.LogError(x);
		}
		Debug.LogError("Old x: " + x);
	}

    // Update is called once per frame
    void Update()
    {
		//transform.rotation = Quaternion.RotateTowards(transform.rotation, fRot, 0.5f);
    }
}
