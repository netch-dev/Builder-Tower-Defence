using UnityEngine;

public class ArrowProjectile : MonoBehaviour {
	public static ArrowProjectile Create(Vector3 position, Enemy targetEnemy) {
		Transform arrowTransform = Instantiate(GameAssets.Instance.arrowProjectilePrefab, position, Quaternion.identity);

		ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
		arrowProjectile.SetTarget(targetEnemy);
		return arrowProjectile;
	}

	private Enemy targetEnemy;
	private Vector3 lastMoveDirection;
	private float lifetime = 2f;
	private void SetTarget(Enemy targetEnemy) {
		this.targetEnemy = targetEnemy;
	}

	private void Update() {
		Vector3 moveDirection;
		if (targetEnemy != null) {
			moveDirection = (targetEnemy.transform.position - transform.position).normalized;
			lastMoveDirection = moveDirection;
		} else {
			moveDirection = lastMoveDirection;
		}

		float moveSpeed = 20f;
		transform.position += moveDirection * Time.deltaTime * moveSpeed;

		transform.eulerAngles = new Vector3(0, 0, Utils.GetAngleFromVector(moveDirection));

		lifetime -= Time.deltaTime;
		if (lifetime < 0) {
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		Enemy enemy = collision.GetComponent<Enemy>();
		if (enemy != null) {
			enemy.GetComponent<HealthSystem>().Damage(10);
			Destroy(gameObject);
		}
	}
}
