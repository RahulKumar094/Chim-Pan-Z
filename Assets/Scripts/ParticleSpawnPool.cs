using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawnPool : MonoBehaviour
{
	public Transform Container;
	public int MaxParticles;

	private List<GameObject> Particles;
	public static ParticleSpawnPool Instance;

	private int[] PrimitiveTypeIndex;

	void Awake()
    {
		if (Instance != null)
			Destroy(this.gameObject);
		else
			Instance = this;
    }

	private void Start()
	{
		Particles = new List<GameObject>();
		PrimitiveTypeIndex = new int[3];
		PrimitiveTypeIndex[0] = 0;
		PrimitiveTypeIndex[1] = 3;
		PrimitiveTypeIndex[2] = 2;

		for (int j = 0; j < MaxParticles; j++)
		{
			PrimitiveType type = (PrimitiveType)PrimitiveTypeIndex[Random.Range(0, 3)];
			GameObject inst = GameObject.CreatePrimitive(type);
			inst.transform.parent = Container;
			inst.tag = type.ToString();
			inst.SetActive(false);
			Particles.Add(inst);
		}
	}

	public GameObject GetRandomParticleInstance()
	{
		PrimitiveType type = (PrimitiveType)PrimitiveTypeIndex[Random.Range(0, 3)];
		GameObject inst = Particles.Find(x => !x.activeSelf && x.CompareTag(type.ToString()));
		if (inst != null)
		{
			inst.SetActive(true);
			return inst;
		}

		return null;
	}

}
