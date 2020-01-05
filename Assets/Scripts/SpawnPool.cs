using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPool : MonoBehaviour
{
	public GameObject _PfEnemy;
	public int MaxEnemyCount = 20;

	public static SpawnPool Instance;

	private List<GameObject> EnemyList;

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
		EnemyList = new List<GameObject>();
    }

	public GameObject GetNewEnemy()
	{
		for (int i = 0; i < EnemyList.Count; i++)
		{
			if (!EnemyList[i].activeSelf)
			{
				EnemyList[i].SetActive(true);
				return EnemyList[i];
			}
		}

		if (EnemyList.Count < MaxEnemyCount)
		{
			GameObject enemy = Instantiate(_PfEnemy, transform);
			EnemyList.Add(enemy);
			return enemy;
		}

		return null;
	}
}
