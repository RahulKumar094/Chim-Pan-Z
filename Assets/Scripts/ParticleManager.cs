using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
	SmallSmoke,
	BotBlast,
	ElectricSparks
}

public class ParticleManager : MonoBehaviour
{
	public CustomParticleCreator CustomParticleCreator;
	public static ParticleManager Instance;

    void Awake()
    {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
    }

	public void CreateParticle(ParticleType type, Vector3 position)
	{

	}

	public void CreateBurstParticlesAt(Vector3 position)
	{
		CustomParticleCreator.CreateExplosion(position);
	}
}
