using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{
	[Header("Swiping")]
    [SerializeField] private float _minimumDistance = 10; // minimum distance to register input
	[SerializeField] private float _maximumDistance = 500;
	[SerializeField] private float _multiplier = 2;
	[Header("Slow motion")]
	[SerializeField] private float _slowMotionSpeed = 0.1f;
	[SerializeField] private float _slowMotionTransitionSpeed = 1;
	[Header("Style")]
	[SerializeField] private float _torqueMultiplier = 10;
	[Header("Gravity")]
	[SerializeField] private float _idealFallSpeed = -10;
	[SerializeField] private float _fallFactor = 2;
	[SerializeField] private float _slowDownFactor = 2;



	private Vector2 _touchStartPos;
	private int _latest = -1;


	private Rigidbody _rigidbody;

    void Start()
    {
		_rigidbody = GetComponent<Rigidbody>();
		Input.multiTouchEnabled = true;
    }


    void Update()
    {
		Swiping();

		Time.timeScale = Mathf.Lerp(Time.timeScale, GetTimescale(), Time.unscaledDeltaTime * _slowMotionTransitionSpeed);

		ExaggeratedGravity();

	}

	private void ExaggeratedGravity()
    {
		var current = _rigidbody.velocity;
		if (current.y <= 0)
        {
			// we want to get to the fastest fall non-linearly
			current.y = Mathf.Lerp(current.y, _idealFallSpeed, Time.deltaTime * _fallFactor);
        }
        else
        {
			// we want to slow down to 0 non-linearly
			current.y = Mathf.Lerp(current.y, _idealFallSpeed, Time.deltaTime * _slowDownFactor);
		}
		_rigidbody.velocity = current;
	}

	private float GetTimescale()
    {
		return _latest == -1 ? 1 : _slowMotionSpeed;
    }

	private void Swiping()
	{
#if (UNITY_EDITOR)
		// DEBUG CODE
		var touch = new Touch();

		if (Input.GetMouseButtonDown(0))
			touch.phase = TouchPhase.Began;
		else if (Input.GetMouseButtonUp(0))
			touch.phase = TouchPhase.Ended;
		else
			touch.phase = Input.GetMouseButton(0) ? TouchPhase.Moved : TouchPhase.Stationary;
		touch.position = Input.mousePosition;
		RunTouch(touch, 0);
#endif

		for (int i = 0; i < Input.touchCount; i++)
		{
			RunTouch(Input.GetTouch(i), i);
		}
	}

	private void RunTouch(Touch touch, int i)
    {
		switch (touch.phase)
		{
			case TouchPhase.Began:
				if (i == 0)
				{
					_touchStartPos = touch.position;
					_latest = 0;
				}
				break;
			case TouchPhase.Canceled:
				_latest = -1;
				break;
			case TouchPhase.Ended:
				var swipe = GetSwipe(touch);
				_rigidbody.velocity = swipe;
				_rigidbody.AddTorque(swipe * _torqueMultiplier, ForceMode.VelocityChange);
				_latest = -1;
				break;
			case TouchPhase.Moved:
				if (_latest != i)
					break;
				break;
		}
	}

	private Vector2 GetSwipe(Touch touch)
    {
		Vector2 swipe = _touchStartPos - touch.position;
		if(swipe.magnitude >= _minimumDistance)
        {
			if (swipe.magnitude > _maximumDistance)
				swipe = swipe * _maximumDistance / swipe.magnitude;
			return swipe * _multiplier;
        }
        else
        {
			return Vector2.zero;
        }
	}
}
