using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPool : MonoBehaviour
{
	public GameObject _PfEnemy;
	public GameObject _PfLaser;
	public GameObject _PfOrb;
	public int MaxEnemyCount = 20;	
	public int MaxLaserCount = 20;	
	public int MaxOrbsCount = 40;

	public static SpawnPool Instance;

	private List<GameObject> mEnemyList;
	private List<GameObject> mLaserList;
	private List<GameObject> mOrbsList;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this.gameObject);

		DontDestroyOnLoad(this);
	}

	void Start()
    {
		mEnemyList = new List<GameObject>();
		mLaserList = new List<GameObject>();
		mOrbsList = new List<GameObject>();
	}

	public GameObject GetEnemyFromPool()
	{
		return GetObjectFromPool(ref mEnemyList, _PfEnemy, MaxEnemyCount);
	}

	public GameObject GetLaserFromPool()
	{
		return GetObjectFromPool(ref mLaserList, _PfLaser, MaxLaserCount);
	}

	public GameObject GetOrbFromPool()
	{
		return GetObjectFromPool(ref mOrbsList, _PfOrb, MaxOrbsCount);
	}

	private GameObject GetObjectFromPool(ref List<GameObject> objs, GameObject prefab, int maxCount)
	{
		for (int i = 0; i < objs.Count; i++)
		{
			if (!objs[i].activeSelf)
			{
				objs[i].SetActive(true);
				return objs[i];
			}
		}

		if (objs.Count < maxCount)
		{
			GameObject obj = Instantiate(prefab, transform);
			objs.Add(obj);
			return obj;
		}

		return null;
	}
}
