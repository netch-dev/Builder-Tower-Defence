using UnityEngine;

public class Tower : MonoBehaviour {
	[SerializeField] private Transform projectileSpawn;
	private Vector3 projectileSpawnPosition;

	[SerializeField] private float shootTimerMax = 1f;
	private float shootTimer;

	private Enemy targetEnemy;
	private float lookForTargetTimer;
	private float lookForTargetTimerMax = 0.2f;

	private void Awake() {
		projectileSpawnPosition = projectileSpawn.position;
	}

	private void Update() {
		HandleTargeting();
		HandleShooting();
	}

	private void HandleShooting() {
		shootTimer -= Time.deltaTime;
		if (shootTimer < 0) {
			shootTimer += shootTimerMax;
			if (targetEnemy != null) {
				ArrowProjectile.Create(projectileSpawnPosition, targetEnemy);
			}
		}
	}

	private void HandleTargeting() {
		lookForTargetTimer -= Time.deltaTime;
		if (lookForTargetTimer < 0) {
			lookForTargetTimer += lookForTargetTimerMax;
			LookForTargets();
		}
	}

	private void LookForTargets() {
		float targetMaxRadius = 20f;
		Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

		foreach (Collider2D collider2D in collider2DArray) {
			Enemy enemy = collider2D.GetComponent<Enemy>();
			if (enemy != null) {
				if (targetEnemy == null) {
					targetEnemy = enemy;
				} else { // Check if this enemy is closer than the current target
					if (Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(targetEnemy.transform.position, transform.position)) {
						targetEnemy = enemy;
					}
				}
			}
		}
	}
}
