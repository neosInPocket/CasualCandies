using System;
using System.Collections;
using UnityEngine;

public class EnemyCandyRotator : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rigid;
	[SerializeField] private new CircleCollider2D collider;
	[SerializeField] private GameObject explosion;
	[SerializeField] private Transform playerCandy;
	[SerializeField] private float yStartSpawnValue;
	[SerializeField] private float rotateSpeed;
	[SerializeField] private float breakSpeed;
	[SerializeField] private float shootSpeed;
	[SerializeField] private float yScreenTopBorder;
	[SerializeField] private Vector2 repulsiveForces;
	[SerializeField] private GameObject contactExplosion;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private GameObject explosionSystem;

	private float repulsiveForce;
	private float worldTopBorder;
	public bool Enabled { get; set; }
	private Vector3 currentRotation;
	private bool breaking;
	private Vector2 screen;
	private Vector2 direction;
	public Action Crushed { get; set; }
	public Rigidbody2D Rigidbody => rigid;
	private bool crashed;

	private void Start()
	{
		screen = new Vector2(Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height, Camera.main.orthographicSize);
		worldTopBorder = 2 * screen.y * yScreenTopBorder - screen.y;
		transform.up = Vector2.up;
		transform.position = new Vector2(0, 2 * screen.y * yStartSpawnValue - screen.y + collider.radius);
		repulsiveForce = Conserver.storeSideUpgrade == 1 ? repulsiveForces.y : repulsiveForces.x;
	}

	public void SetDefaults()
	{
		spriteRenderer.enabled = true;
		rigid.constraints = RigidbodyConstraints2D.None;
		transform.position = new Vector2(0, 2 * screen.y * yStartSpawnValue - screen.y + collider.radius);
		transform.up = Vector2.up;
		explosionSystem.gameObject.SetActive(false);
		crashed = false;
		rigid.angularVelocity = 0;
	}

	private void Update()
	{
		if (!Enabled) return;

		if (!crashed)
		{
			if (transform.position.x > screen.x - collider.radius ||
			transform.position.x < -screen.x + collider.radius ||
			transform.position.y > worldTopBorder - collider.radius ||
			transform.position.y < -screen.y + collider.radius)
			{
				Crash();
				crashed = true;

				return;
			}
		}

		if (rigid.velocity.magnitude > 0 && Mathf.Approximately(rigid.velocity.magnitude, 0))
		{
			rigid.velocity = Vector2.zero;
			Debug.Log("approx");
		}

		breaking = rigid.velocity.magnitude > 0;
		if (breaking)
		{
			rigid.velocity -= rigid.velocity.normalized * breakSpeed * Time.deltaTime;
			if (Vector2.Dot(rigid.velocity, direction) < 0)
			{
				rigid.velocity = Vector2.zero;
			}
		}

		currentRotation.z += rotateSpeed * Time.deltaTime;
		transform.eulerAngles = currentRotation;

		if (!breaking)
		{
			var playerRaycast = Physics2D.Raycast(transform.position + transform.up * collider.radius, transform.up);
			if (playerRaycast.collider && playerRaycast.collider.name == "CandyRotator")
			{
				rigid.velocity = transform.up * shootSpeed;
				direction = rigid.velocity.normalized;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var player = collision.collider.GetComponent<CandyRotator>();
		var direction = transform.position - player.transform.position;
		this.direction = direction;
		rigid.velocity = (direction).normalized * repulsiveForce;
		player.PushCandy(-direction);

		var contact = Instantiate(contactExplosion, collision.GetContact(0).point, Quaternion.identity, null);
		StartCoroutine(Contact(contact));
	}

	private IEnumerator Contact(GameObject obj)
	{
		yield return new WaitForSeconds(1f);
		Destroy(obj);
	}

	public void Crash()
	{
		if (crashed) return;
		crashed = true;

		Enabled = false;
		rigid.constraints = RigidbodyConstraints2D.FreezeAll;
		explosionSystem.gameObject.SetActive(true);
		spriteRenderer.enabled = false;
		Crushed?.Invoke();
	}
}
