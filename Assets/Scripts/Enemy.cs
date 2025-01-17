using UnityEngine;

public class Enemy : MonoBehaviour {

	public static Enemy Create(Vector3 position) {
		Transform enemyTransform = Instantiate(GameAssets.Instance.enemyPrefab, position, Quaternion.identity);

		Enemy enemy = enemyTransform.GetComponent<Enemy>();
		return enemy;
	}

	private Rigidbody2D rb;
	private Transform targetTransform;
	private HealthSystem healthSystem;
	private float lookForTargetTimer;
	private float lookForTargetTimerMax = 0.2f;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		if (BuildingManager.Instance.GetHQBuilding() != null) {
			targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
		}

		healthSystem = GetComponent<HealthSystem>();
		healthSystem.OnDamaged += HealthSystem_OnDamaged;
		healthSystem.OnDie += HealthSystem_OnDie;

		lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
	}

	private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
		SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
		CinemachineShake.Instance.ShakeCamera(3f, 0.1f);
		ChromaticAbberationEffect.Instance.SetWeight(0.5f);
	}
	private void HealthSystem_OnDie(object sender, System.EventArgs e) {
		SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
		CinemachineShake.Instance.ShakeCamera(7f, 0.15f);
		ChromaticAbberationEffect.Instance.SetWeight(0.5f);
		Instantiate(GameAssets.Instance.enemyDieParticlesPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		Building building = collision.gameObject.GetComponent<Building>();
		if (building != null) {
			HealthSystem buildingHealthSystem = building.GetComponent<HealthSystem>();
			buildingHealthSystem.Damage(10);
			this.healthSystem.Damage(9999);
		}
	}

	private void Update() {
		HandleMovement();
		HandleTargeting();
	}

	private void HandleMovement() {
		if (targetTransform == null) {
			rb.velocity = Vector2.zero;
			return;
		}

		Vector3 moveDirection = (targetTransform.transform.position - transform.position).normalized;
		float moveSpeed = 5f;
		rb.velocity = moveDirection * moveSpeed;
	}

	private void HandleTargeting() {
		lookForTargetTimer -= Time.deltaTime;
		if (lookForTargetTimer < 0) {
			lookForTargetTimer += lookForTargetTimerMax;
			LookForTargets();
		}
	}

	private void LookForTargets() {
		float targetMaxRadius = 10f;
		Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

		foreach (Collider2D collider2D in collider2DArray) {
			Building building = collider2D.GetComponent<Building>();
			if (building != null) {
				if (targetTransform == null) {
					targetTransform = building.transform;
				} else { // Check if this building is closer than the current target
					if (Vector3.Distance(building.transform.position, transform.position) < Vector3.Distance(targetTransform.position, transform.position)) {
						targetTransform = building.transform;
					}
				}
			}
		}

		if (targetTransform == null) { // No valid targets in range
			if (BuildingManager.Instance.GetHQBuilding() != null) {
				targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
			}
		}
	}
}
