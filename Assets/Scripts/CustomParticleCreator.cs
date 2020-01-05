using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParticleCreator : MonoBehaviour
{
	public float shapeSize = 0.2f;
	public Vector3 shapeCount = new Vector3(5,5,2);
	public Material[] materials;

	[Space]
	public float explosionForce = 200f;
	public float explosionRadius = 4f;
	public float explosionUpward = 0.4f;

	private float cubesPivotDistance;
	private Vector3 particlesPivot;
	private Vector3 position;

	void Start()
    {
		cubesPivotDistance = shapeSize * shapeCount.x / 2;
		particlesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
	}

	public void CreateExplosion(Vector3 position)
	{
		Explode(position, materials);
	}

	private void Explode(Vector3 position, Material[] materials)
	{
		this.position = position;
		List<GameObject> pieces = new List<GameObject>();

		//particle 5X5X2
		for (int x = 0; x < shapeCount.x; x++)
		{
			for (int y = 0; y < shapeCount.y; y++)
			{
				for (int z = 0; z < shapeCount.z; z++)
				{
					int mat = Random.Range(0, materials.Length);
					GameObject piece = CreatePiece(x, y, z, materials[mat]);
					if(piece != null) pieces.Add(piece);
				}
			}
		}

		foreach (GameObject piece in pieces)
		{
			Rigidbody rb = piece.GetComponent<Rigidbody>();
			if (rb != null)
			{
				rb.velocity = new Vector3(Random.Range(-0.5f, 0.5f), 0, 0.8f) * 10;
				rb.AddExplosionForce(explosionForce, piece.transform.position, explosionRadius, explosionUpward, ForceMode.Acceleration);
			}
		}
	}

	private GameObject CreatePiece(int x, int y, int z, Material material)
	{
		GameObject piece = ParticleSpawnPool.Instance.GetRandomParticleInstance();

		if (piece != null)
		{
			piece.GetComponent<Renderer>().material = material;
			piece.transform.position = position + new Vector3(shapeSize * x, shapeSize * y, shapeSize * z) - particlesPivot;
			piece.transform.localScale = new Vector3(shapeSize, shapeSize, shapeSize);

			AutoDestruct ad = piece.GetComponent<AutoDestruct>();
			if (ad == null)
				ad = piece.AddComponent<AutoDestruct>();

			Rigidbody rb = piece.GetComponent<Rigidbody>();
			if (rb == null)
				rb = piece.AddComponent<Rigidbody>();

			rb.mass = shapeSize;

			float f = Random.Range(1.5f, 3f);
			ad.Destruct(f);

			return piece;
		}

		return null;
	}

}
