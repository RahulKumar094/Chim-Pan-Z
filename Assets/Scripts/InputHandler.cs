using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
	public float DragToSwipeDuration = 0.6f;
	public float SwipeLength = 1f;

	public static PointerTap OnTapEvent;
	public delegate void PointerTap();
	public static SwipeLeft OnSwipeLeftEvent;
	public delegate void SwipeLeft();
	public static SwipeRight OnSwipeRightEvent;
	public delegate void SwipeRight();
	public static SwipeUp OnSwipeUpEvent;
	public delegate void SwipeUp();
	public static SwipeDown OnSwipeDownEvent;
	public delegate void SwipeDown();
	public static PointerHold OnHoldEvent;
	public delegate void PointerHold(float holdTime);

	private bool mPointerDown;
	private Vector3 mPointerDownPosition;
	private Vector3 mPointerDragPosition;
	private Vector3 mPointerUpPosition;
	private float mDragTime;

	private void Start()
	{
		Reset();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			mPointerDownPosition = Input.mousePosition;
			mPointerDown = true;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			mPointerDown = false;
			mPointerUpPosition = Input.mousePosition;
			TapAndSwipe();
			Reset();
		}

		if (mPointerDown)
		{
			mPointerDragPosition = Input.mousePosition;
			mDragTime += Time.deltaTime;
			if (mDragTime > DragToSwipeDuration)
			{
				OnHoldEvent(mDragTime - DragToSwipeDuration);
			}
		}
	}

	private void TapAndSwipe()
	{
		if (mDragTime <= DragToSwipeDuration)
		{
			if (Vector2.Distance(mPointerDownPosition, mPointerUpPosition) > SwipeLength)
			{
				Vector2 swipeDelta = mPointerUpPosition - mPointerDownPosition;
				if (Mathf.Abs(swipeDelta.x) >= Mathf.Abs(swipeDelta.y))
				{
					if (swipeDelta.x < 0)
						OnSwipeLeftEvent();
					else
						OnSwipeRightEvent();
				}
				else
				{
					if (swipeDelta.y < 0)
						OnSwipeDownEvent();
					else
						OnSwipeUpEvent();
				}
			}
			else
				OnTapEvent();
		}
	}

	private void Reset()
	{
		mPointerDownPosition = Vector3.zero;
		mPointerDragPosition = Vector3.zero;
		mPointerUpPosition = Vector3.zero;
		mDragTime = 0;
	}

}
