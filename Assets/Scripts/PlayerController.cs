using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public int Health = 3;
	public float MoveSpeed = 10;
	public BoxCollider TargetDetector;
	public static PlayerController Instance;

	private Animator mAnimator;
	private GameObject mTarget;
	private Transform mWallInFront;
	private float mMoveToX;
	private bool mIsTargetInRange;

	private int mHealth;
	private float mMoveSpeed;

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

		mAnimator = GetComponent<Animator>();
		ResetPlayer();
	}
	
	void Update()
    {
		transform.position += Vector3.forward * mMoveSpeed * Time.deltaTime;
	}

	private void OnTap()
	{
		if (mIsTargetInRange && mTarget != null)
		{
			float dist = Vector3.Distance(mTarget.transform.position, transform.position);
			if (dist < TargetDetector.size.z / 3)
			{
				UIManager.Instance.ShowFeedback("Late");
			}
			else if (dist >= TargetDetector.size.z / 3 && dist < 2 * TargetDetector.size.z / 3)
			{
				UIManager.Instance.ShowFeedback("Perfect");
			}
			else
				UIManager.Instance.ShowFeedback("Early");


			mAnimator.SetTrigger("MeleeAttack");
			mTarget.GetComponent<Enemy>().Kill();
			mIsTargetInRange = false;
			mTarget = null;
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
			StartCoroutine("MoveTo");
		}			
	}

	private void OnSwipeRight()
	{
		if (transform.position.x < GameSettings.MoveRightLimit)
		{
			mMoveToX = transform.position.x + GameSettings.SideMoveDistance;
			StartCoroutine("MoveTo");
		}			
	}

	private void OnHold(float holdTime)
	{
		Debug.Log("Held Time: " + holdTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Target")
		{
			mIsTargetInRange = true;
			mTarget = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Target")
		{
			mIsTargetInRange = false;
			mTarget = null;
		}		
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Target" || collision.transform.tag == "Laser")
		{
			mHealth--;

			if (mHealth == 0)
			{
				mAnimator.SetTrigger("Dying");
				mMoveSpeed = 0;
			}

			Debug.LogError("Health: " + mHealth);
		}
	}

	private IEnumerator MoveTo()
	{
		while (transform.position.x != mMoveToX)
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(mMoveToX, transform.position.y, transform.position.z), 0.18f);

			if (Mathf.Abs(transform.position.x - mMoveToX) < 0.2f)
			{
				StopCoroutine("MoveTo");
				transform.position = new Vector3(mMoveToX, transform.position.y, transform.position.z);
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private void ResetPlayer()
	{
		transform.position = new Vector3(0, transform.position.y, 5f);
		mMoveSpeed = MoveSpeed;
		mHealth = Health;
	}

}
