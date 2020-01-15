using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public int Health = 3;
	public float MoveSpeed = 10;
	public static PlayerController Instance;

	public delegate void OnTakeDamageEvent(int currentHealth, PoolObject collidingObject, Vector3 collisionPoint);
	public static OnTakeDamageEvent OnTakeDamage;

	public Vector3 HipPosition
	{
		get { return mHipTransform.position; }
	}
	private Animator mAnimator;
	private GameObject mEnemyInProximity;
	private GameObject mLaserInProximity;
	private Transform mWallInFront;	
	private Transform mHipTransform;
	private float mMoveToX;
	private int mHealth;
	private float mMoveSpeed;
	private Vector3 mInitPlayerPosition;
	private bool mIsControlActive;
	private TargetDetector TargetDetector;
	private HitDetector HitDetector;

	void Awake()
    {
		if (Instance != null)
			Destroy(this.gameObject);
		else
			Instance = this;

		DontDestroyOnLoad(this);
	}

	private void Start()
	{
		InputHandler.OnTapEvent += OnTap;
		InputHandler.OnSwipeUpEvent += OnSwipeUp;
		InputHandler.OnSwipeDownEvent += OnSwipeDown;
		InputHandler.OnSwipeLeftEvent += OnSwipeLeft;
		InputHandler.OnSwipeRightEvent += OnSwipeRight;
		InputHandler.OnHoldEvent += OnHold;

		mHipTransform = transform.GetChild(0);
		mAnimator = GetComponent<Animator>();
		mInitPlayerPosition = transform.position;
		TargetDetector = GetComponentInChildren<TargetDetector>();
		HitDetector = GetComponentInChildren<HitDetector>();
	}
	
	void Update()
    {
		transform.position += Vector3.forward * mMoveSpeed * Time.deltaTime;
	}

	private void OnTap()
	{
		if (mIsControlActive && mEnemyInProximity != null)
		{
			float dist = Mathf.Abs(mEnemyInProximity.transform.position.z - transform.position.z);
			if (dist < TargetDetector.Size.z / 3)
			{
				GameManager.Instance.HitType("Late");
			}
			else if (dist >= TargetDetector.Size.z / 3 && dist < 2 * TargetDetector.Size.z / 3)
			{
				GameManager.Instance.HitType("Perfect");
			}
			else
				GameManager.Instance.HitType("Early");

			mAnimator.SetTrigger("MeleeAttack");
			mEnemyInProximity.GetComponent<Enemy>().Kill();
			mEnemyInProximity = null;
		}
	}

	private void OnSwipeUp()
	{
	}

	private void OnSwipeDown()
	{
	}

	private void OnSwipeLeft()
	{
		if (mIsControlActive && transform.position.x > GameSettings.MoveLeftLimit)
		{
			mMoveToX = transform.position.x - GameSettings.SideMoveDistance;
			CheckForNearByMiss();
			StartCoroutine("MoveTo");
		}
	}

	private void OnSwipeRight()
	{
		if (mIsControlActive && transform.position.x < GameSettings.MoveRightLimit)
		{
			mMoveToX = transform.position.x + GameSettings.SideMoveDistance;
			CheckForNearByMiss();
			StartCoroutine("MoveTo");
		}			
	}

	private void OnHold(float holdTime)
	{
		Debug.Log("Held Time: " + holdTime);
	}

	private void CheckForNearByMiss()
	{
		if (mLaserInProximity != null)
			UIManager.Instance.ShowFeedback("Near By Miss");
	}
	
	private IEnumerator MoveTo()
	{
		while (transform.position.x != mMoveToX)
		{
			transform.position = Vector3.Slerp(transform.position, new Vector3(mMoveToX, transform.position.y, transform.position.z), 0.18f);

			if (Mathf.Abs(transform.position.x - mMoveToX) < 0.2f)
			{
				StopCoroutine("MoveTo");
				transform.position = new Vector3(mMoveToX, transform.position.y, transform.position.z);
			}
			yield return new WaitForEndOfFrame();
		}
	}

	public void SetEnemyInProximity(GameObject enemyInProximity)
	{
		mEnemyInProximity = enemyInProximity;
	}

	public void SetLaserInProximity(GameObject laserInProximity)
	{
		mLaserInProximity = laserInProximity;
	}

	public void HitByEnemy(Collider enemy)
	{
		HandleCollision(enemy);
	}

	public void HitByLaser(Collider laser)
	{
		mAnimator.SetTrigger("Hit");
		mLaserInProximity = null;
		HandleCollision(laser);
	}

	private void HandleCollision(Collider other)
	{
		if (--mHealth >= 0)
			OnTakeDamage?.Invoke(mHealth, other.GetComponent<PoolObject>(), other.ClosestPoint(transform.position));
	}

	public void ResetPlayer()
	{
		transform.position = mInitPlayerPosition;
		mMoveSpeed = MoveSpeed;
		mHealth = Health;
		mIsControlActive = true;
	}

	public void KilledByEnemy()
	{
		mMoveSpeed = 0;
		mAnimator.SetTrigger("Dying");
		mIsControlActive = false;
	}

	public void KilledByLaser()
	{
		mMoveSpeed = 0;
		mAnimator.SetTrigger("Dying");
		mIsControlActive = false;
	}

}
