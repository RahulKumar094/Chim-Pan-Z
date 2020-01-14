using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public int Health = 3;
	public float MoveSpeed = 10;
	public BoxCollider TargetDetector;
	public static PlayerController Instance;

	public delegate void OnTakeDamageEvent(int currentHealth, PoolObject collidingObject);
	public static OnTakeDamageEvent OnTakeDamage;

	public Vector3 HipPosition
	{
		get { return mHipTransform.position; }
	}
	private Animator mAnimator;
	private GameObject mTargetInProximity;
	private GameObject mLaserInProximity;
	private Transform mWallInFront;	
	private Transform mHipTransform;
	private float mMoveToX;
	private int mHealth;
	private float mMoveSpeed;
	private Vector3 mInitPlayerPosition;

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
	}
	
	void Update()
    {
		transform.position += Vector3.forward * mMoveSpeed * Time.deltaTime;
	}

	private void OnTap()
	{
		if (mTargetInProximity != null)
		{
			float dist = Mathf.Abs(mTargetInProximity.transform.position.z - transform.position.z);
			if (dist < TargetDetector.size.z / 3)
			{
				GameManager.Instance.HitType("Late");
			}
			else if (dist >= TargetDetector.size.z / 3 && dist < 2 * TargetDetector.size.z / 3)
			{
				GameManager.Instance.HitType("Perfect");
			}
			else
				GameManager.Instance.HitType("Early");

			mAnimator.SetTrigger("MeleeAttack");
			mTargetInProximity.GetComponent<Enemy>().Kill();
			mTargetInProximity = null;
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
		if (transform.position.x > GameSettings.MoveLeftLimit)
		{
			mMoveToX = transform.position.x - GameSettings.SideMoveDistance;
			CheckForNearByMiss();
			StartCoroutine("MoveTo");
		}
	}

	private void OnSwipeRight()
	{
		if (transform.position.x < GameSettings.MoveRightLimit)
		{
			mMoveToX = transform.position.x + GameSettings.SideMoveDistance;
			CheckForNearByMiss();
			StartCoroutine("MoveTo");
		}			
	}

	private void CheckForNearByMiss()
	{
		if (mLaserInProximity != null)
		{
			if (Mathf.Abs(mLaserInProximity.transform.position.z - transform.position.z) < TargetDetector.size.z)
				UIManager.Instance.ShowFeedback("Near By Miss");
		}
	}

	private void OnHold(float holdTime)
	{
		Debug.Log("Held Time: " + holdTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		System.Type type = other.GetType();

		if (type == typeof(CapsuleCollider) && (other.CompareTag("Target") || other.CompareTag("Laser")))
		{
			if(--mHealth >= 0)
				OnTakeDamage?.Invoke(mHealth, other.GetComponent<PoolObject>());
		}
		else if (type == typeof(BoxCollider))
		{
			if (other.CompareTag("Target"))
			{
				mTargetInProximity = other.gameObject;
			}
			else if (other.CompareTag("Laser"))
			{
				mLaserInProximity = other.gameObject;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		System.Type type = other.GetType();

		if (type == typeof(BoxCollider))
		{
			if (other.tag == "Target")
			{
				mTargetInProximity = null;
			}
			else if (other.CompareTag("Laser"))
			{
				mLaserInProximity = null;
			}
		}
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

	public void ResetPlayer()
	{
		transform.position = mInitPlayerPosition;
		mMoveSpeed = MoveSpeed;
		mHealth = Health;
	}

	public void KilledByEnemy()
	{
		mMoveSpeed = 0;
		mAnimator.SetTrigger("Dying");
	}

	public void KilledByLaser()
	{
		mMoveSpeed = 0;
		mAnimator.SetTrigger("Dying");
	}

	public void HitByLaser()
	{
		mAnimator.SetTrigger("Hit");
	}

}
