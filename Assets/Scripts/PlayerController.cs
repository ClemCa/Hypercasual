using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[Header("Swiping")]
    [SerializeField] private float _minimumDistance = 10; // minimum distance to register input
	[SerializeField] private float _maximumDistance = 500;
	[SerializeField] private float _multiplier = 2;
	[SerializeField] private float _swipesReserve = 3;
	[SerializeField] private float _rechargeSpeed = 1;
	[SerializeField] private float _rechargeDelay = 1;
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
	private float _currentSwipes;
	private float _resetTimer;

	private Rigidbody _rigidbody;
	private MeshRenderer _meshRenderer;

    void Start()
    {
		_currentSwipes = _swipesReserve;
		_rigidbody = GetComponent<Rigidbody>();
		_meshRenderer = GetComponent<MeshRenderer>();
		Input.multiTouchEnabled = true;
    }


    void Update()
    {

		Swiping();

		Time.timeScale = Mathf.Lerp(Time.timeScale, GetTimescale(), Time.unscaledDeltaTime * _slowMotionTransitionSpeed);

		ExaggeratedGravity();

		_meshRenderer.material.color = Color.Lerp(new Color(0.75f, 0, 0), new Color(0, 0.75f, 0), _currentSwipes / _swipesReserve);

		_resetTimer += Time.deltaTime;
		if (_resetTimer >= _rechargeDelay)
		{
			_currentSwipes = Mathf.Min(_currentSwipes + Time.deltaTime * _rechargeSpeed, _swipesReserve);
		}
	}

    void OnBecameInvisible()
    {
		// DEATH
		SceneManager.LoadScene("LevelSelection");
	}

	public void GiveSwipe()
    {
		_currentSwipes = Mathf.Min(_currentSwipes + 1, _swipesReserve);
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
		if(touch.phase != TouchPhase.Stationary)
        {
			RunTouch(touch, 0);
		}
#endif

		for (int i = 0; i < Input.touchCount; i++)
		{
			RunTouch(Input.GetTouch(i), i);
		}
	}

	private void RunTouch(Touch touch, int i)
    {
		if (_currentSwipes < 1)
			return;
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
				_resetTimer = 0;
				break;
			case TouchPhase.Ended:
				var swipe = GetSwipe(touch);
				_rigidbody.velocity = swipe;
				_rigidbody.AddTorque(swipe * _torqueMultiplier, ForceMode.VelocityChange);
				_latest = -1;
				_currentSwipes--;
				_resetTimer = 0;
				break;
			case TouchPhase.Moved:
				if (_latest != i)
					break;
				break;
		}
	}

	private Vector2 GetSwipe(Touch touch)
    {
		if (_latest == -1)
			return Vector2.zero;
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
