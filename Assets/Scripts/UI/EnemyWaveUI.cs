using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveUI : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI waveNumberText;
	[SerializeField] private TextMeshProUGUI waveMessageText;
	[SerializeField] private RectTransform enemyWaveSpawnPositionIndicator;
	[SerializeField] private RectTransform enemyClosestPositionIndicator;

	[SerializeField] private EnemyWaveManager enemyWaveManager;

	private StringBuilder stringBuilder;
	private Camera mainCamera;

	private void Start() {
		stringBuilder = new StringBuilder();
		mainCamera = Camera.main;
		enemyWaveManager.OnWaveNumberChanged += OnWaveNumberChanged;
		OnWaveNumberChanged(null, null);
	}

	private void Update() {
		HandleNextWaveMessage();

		HandleNextWaveSpawnPositionIndicator();
		HandleEnemyClosestPositionIndicator();
	}

	private void HandleNextWaveMessage() {
		float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
		if (nextWaveSpawnTimer <= 0f) {
			SetMessageText(string.Empty);
		} else {
			stringBuilder.Clear();
			stringBuilder.Append("Next wave in: ");
			stringBuilder.Append(nextWaveSpawnTimer.ToString("F1"));

			SetMessageText(stringBuilder.ToString());
		}
	}

	private void HandleNextWaveSpawnPositionIndicator() {
		Vector3 directionToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position).normalized;

		enemyWaveSpawnPositionIndicator.anchoredPosition = directionToNextSpawnPosition * 300f;
		enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, Utils.GetAngleFromVector(directionToNextSpawnPosition));

		float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCamera.transform.position);
		enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f);
	}

	private void HandleEnemyClosestPositionIndicator() {
		float targetMaxRadius = 9999f;
		Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(mainCamera.transform.position, targetMaxRadius);

		Enemy targetEnemy = null;
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

		if (targetEnemy != null) {
			Vector3 directionToClosestEnemy = (targetEnemy.transform.position - mainCamera.transform.position).normalized;

			enemyClosestPositionIndicator.anchoredPosition = directionToClosestEnemy * 250f;
			enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, Utils.GetAngleFromVector(directionToClosestEnemy));

			float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, mainCamera.transform.position);
			enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5f);
		} else {
			enemyClosestPositionIndicator.gameObject.SetActive(false);
		}
	}

	private void OnWaveNumberChanged(object sender, EventArgs e) {
		stringBuilder.Clear();
		stringBuilder.Append("Wave: ");
		stringBuilder.Append(enemyWaveManager.GetWaveNumber());

		SetWaveNumberText(stringBuilder.ToString());
	}

	private void SetMessageText(string message) {
		waveMessageText.SetText(message);
	}

	private void SetWaveNumberText(string text) {
		waveNumberText.SetText(text);
	}
}
