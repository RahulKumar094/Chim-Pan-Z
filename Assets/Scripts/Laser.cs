using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	// Start is called before the first frame update
	public float MoveSpeed = 8;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (transform.position.z - PlayerController.Instance.transform.position.z < -10f)
			gameObject.SetActive(false);

		transform.position += new Vector3(0, 0, -MoveSpeed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Player")
		{
			Destroy(this.gameObject);
			ParticleManager.Instance.CreateParticle(ParticleType.SmallSmoke, collision.contacts[0].point);
		}
	}
}
