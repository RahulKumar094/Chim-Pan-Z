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
	public static ParticleManager Instance;
	public Material _GreenOrb;
	public Material _RedOrb;
	public float MoveTimeAfterBurst = 3;
	public float OrbBurstSpeed = 5;

	private float Timer;
	private List<GameObject> mAllOrbs;

    void Awake()
    {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
    }

	void Start()
	{
		mAllOrbs = new List<GameObject>();
	}

	public void CreateParticle(ParticleType type, Vector3 position)
	{

	}

	public void CreateRedOrbs(int count, Vector3 position)
	{
		CreateOrbs(count, position,_RedOrb);
	}

	public void CreateGreenOrbs(int count, Vector3 position)
	{
		CreateOrbs(count, position,_GreenOrb);
	}

	private void CreateOrbs(int count, Vector3 position, Material material)
	{
		mAllOrbs.Clear();
		GameObject[] orbs = new GameObject[count];
		for (int i = 0; i < orbs.Length; i++)
		{
			orbs[i] = SpawnPool.Instance.GetOrbFromPool();
			if (orbs[i] != null)
			{
				orbs[i].GetComponent<MeshRenderer>().material = material;
				float scale = Random.Range(0.1f, 0.3f);
				orbs[i].transform.localScale = new Vector3(scale, scale, scale * 1.2f);
				mAllOrbs.Add(orbs[i]);
			}
		}
		BurstAt(position);
	}

	private void BurstAt(Vector3 position)
	{
		foreach (GameObject orb in mAllOrbs)
		{
			orb.transform.position = position;
			float f = Random.Range(-90f, 90f);
			orb.transform.rotation = Quaternion.Euler(0, f, 0);
		}
		Timer = 0;
		StartCoroutine("Burst");
	}

	private IEnumerator Burst()
	{
		Quaternion[] fRots = new Quaternion[mAllOrbs.Count];
		for (int i = 0; i < fRots.Length; i++)
		{
			fRots[i] = Quaternion.Euler(-60, Utilities.GetPositiveAngle(mAllOrbs[i].transform.rotation.eulerAngles.y), 0);
		}

		while (Timer < MoveTimeAfterBurst)
		{
			Timer += Time.deltaTime;

			for (int i = 0; i < mAllOrbs.Count; i++)
			{
				mAllOrbs[i].transform.position += mAllOrbs[i].transform.forward * Time.deltaTime * OrbBurstSpeed / (Timer * 0.5f);

				if (Timer >= MoveTimeAfterBurst * 0.1f)
					mAllOrbs[i].transform.rotation = Quaternion.Lerp(mAllOrbs[i].transform.rotation, fRots[i], Time.deltaTime * OrbBurstSpeed / (Timer * 0.5f));
			}

			if (Timer >= MoveTimeAfterBurst)
			{
				StopCoroutine("Burst");
			}

			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}

}
