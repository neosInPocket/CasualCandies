using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CandyRotator : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rigid;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private ParticleSystem explosionSystem;
	[SerializeField] private float[] rotationAmplitudes;
	[SerializeField] private float pushSpeed;
	[SerializeField] private float breakSpeed;
	[SerializeField] private CircleCollider2D circleCollider;
	[SerializeField] private float yScreenTopBorder;
	[SerializeField] private float yInitialPosition;
	[SerializeField] private float[] repulsiveForces;
	private float repulsiveForce;
	private float worldTopBorder;
	private float rotation;
	private bool enabledRotation;
	private Vector3 currentRotation;
	private int currentDirection = 1;
	private Vector2 screen;
	private Vector2 direction;
	public Rigidbody2D Rigidbody => rigid;
	public Action Crushed { get; set; }
	private bool crashed;

	private void Awake()
	{
		EnhancedTouchSupport.Enable();
		TouchSimulation.Enable();
	}

	private void Start()
	{
		screen = new Vector2(Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height, Camera.main.orthographicSize);
		worldTopBorder = 2 * screen.y * yScreenTopBorder - screen.y;
		rotation = rotationAmplitudes[Conserver.storeUpgrade];
		currentRotation = transform.rotation.eulerAngles;
		repulsiveForce = repulsiveForces[Conserver.storeSideUpgrade];
		transform.position = new Vector2(0, 2 * screen.y * yScreenTopBorder * yInitialPosition - screen.y - circleCollider.radius);
		transform.up = Vector2.down;
	}

	public void SetDefaults()
	{
		spriteRenderer.enabled = true;
		rigid.constraints = RigidbodyConstraints2D.None;
		transform.position = new Vector2(0, 2 * screen.y * yScreenTopBorder * yInitialPosition - screen.y - circleCollider.radius);
		transform.up = Vector2.down;
		explosionSystem.gameObject.SetActive(false);
		crashed = false;
		rigid.angularVelocity = 0;
	}

	private void Update()
	{
		if (!enabledRotation) return;

		if (!crashed)
		{
			if (transform.position.x > screen.x - circleCollider.radius ||
			transform.position.x < -screen.x + circleCollider.radius ||
			transform.position.y > worldTopBorder - circleCollider.radius ||
			transform.position.y < -screen.y + circleCollider.radius)
			{
				Crash();
				return;
			}
		}

		currentRotation.z += rotation * Time.deltaTime;
		transform.eulerAngles = currentRotation;

		if (rigid.velocity.magnitude > 0)
		{
			rigid.velocity -= rigid.velocity.normalized * breakSpeed * Time.deltaTime;
			if (Vector2.Dot(rigid.velocity, direction) < 0)
			{
				rigid.velocity = Vector2.zero;
			}
			return;
		}
	}

	public void EnableCandy(bool value)
	{
		enabledRotation = value;

		if (value)
		{
			Touch.onFingerDown += PushCandyForward;
		}
		else
		{
			Touch.onFingerDown -= PushCandyForward;
		}
	}

	public void Crash()
	{
		if (crashed) return;
		crashed = true;

		EnableCandy(false);
		rigid.constraints = RigidbodyConstraints2D.FreezeAll;
		explosionSystem.gameObject.SetActive(true);
		spriteRenderer.enabled = false;
		Crushed?.Invoke();
	}

	public void PushCandyForward(Finger finger)
	{
		if (!Mathf.Approximately(rigid.velocity.magnitude, 0)) return;
		rigid.velocity = transform.up * pushSpeed;
		direction = rigid.velocity.normalized;
	}

	public void PushCandy(Vector2 direction)
	{
		rigid.velocity = direction * repulsiveForce;
		this.direction = direction;
	}

	private void OnDestroy()
	{
		Touch.onFingerDown -= PushCandyForward;
	}
}
