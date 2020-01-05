using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public float EnemySpawnInterval = 10;

	private float mTimer = 0;

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
        
    }

    // Update is called once per frame
    void Update()
    {
		mTimer += Time.deltaTime;

		if (mTimer > EnemySpawnInterval)
		{
			mTimer = 0;
			SpawnEnemies();
		}
    }

	private void SpawnEnemies()
	{
		GameObject enemy = SpawnPool.Instance.GetNewEnemy();
		if (enemy != null)
		{
			Vector3 playersPosition = PlayerController.Instance.transform.position;
			Vector3 enemySpawnPosition = new Vector3(Random.Range((int)GameSettings.MoveLeftLimit, (int)GameSettings.MoveRightLimit), 2.12f, playersPosition.z + 42f);
			enemy.transform.position = enemySpawnPosition;
		}
	}
}
