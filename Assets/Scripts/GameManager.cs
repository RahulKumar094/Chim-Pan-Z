using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;	
	public float EnemySpawnInterval = 10;
	public int ComboCount;

	private CameraControls CameraControls;
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
		PlayerController.OnTakeDamage += OnPlayerTookDamage;
		CameraControls = FindObjectOfType<CameraControls>();
		ResetGame();
	}

	void ResetGame()
	{
		ComboCount = 0;
		PlayerController.Instance.ResetPlayer();
		UIManager.Instance.ShowComboCount(ComboCount);
		CameraControls.ResetCamera();
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
		GameObject enemy = SpawnPool.Instance.GetEnemyFromPool();
		if (enemy != null)
		{
			Vector3 playersPosition = PlayerController.Instance.transform.position;
			Vector3 enemySpawnPosition = new Vector3(Random.Range((int)GameSettings.MoveLeftLimit, (int)GameSettings.MoveRightLimit), 1.275f, playersPosition.z + 42f);
			enemy.transform.position = enemySpawnPosition;
		}
	}

	private void OnPlayerTookDamage(int currentHealth, PoolObject collidingObject, Vector3 collisionPoint)
	{
		ComboCount = 0;

		if (collidingObject is Enemy)
		{
			Enemy enemy = (Enemy)collidingObject;
			currentHealth = 0;
			enemy.KillPlayer();
			PlayerController.Instance.KilledByEnemy();
			CameraControls.CameraSpeed = 0;
			Debug.LogError("Killed By Enemy");
		}
		else if (collidingObject is Laser)
		{
			if (currentHealth <= 0)
			{
				PlayerController.Instance.KilledByLaser();
				CameraControls.CameraSpeed = 0;
				Debug.LogError("Killed By Laser");
			}
			else
			{
				Laser laser = (Laser)collidingObject;
				laser.CollidedAtPoint(collisionPoint);
			}
		}
			
	}

	public void HitType(string type)
	{
		UIManager.Instance.ShowFeedback(type);

		if (System.StringComparer.OrdinalIgnoreCase.Compare(type, "perfect") == 0)
			ComboCount++;
		else
			ComboCount = 0;

		UIManager.Instance.ShowComboCount(ComboCount);
	}
}
