using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour {
	public static EnemyWaveManager Instance { get; private set; }

	public EventHandler OnWaveNumberChanged;

	private enum State {
		WaitingToSpawnWave,
		SpawningWave,
	}

	[SerializeField] private List<Transform> spawnPositionTransformList;
	[SerializeField] private Transform nextWaveSpawnPositionTransform;
	private State state;
	private int waveNumber;
	private float nextWaveSpawnTimer;
	private float nextEnemySpawnTimer;
	private int remainingEnemySpawnAmount;
	private Vector3 spawnPosition;

	private void Awake() {
		Instance = this;
	}

	private void Start() {
		state = State.WaitingToSpawnWave;
		spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
		nextWaveSpawnPositionTransform.position = spawnPosition;
		nextWaveSpawnTimer = 3f;
	}

	private void Update() {

		switch (state) {
			case State.WaitingToSpawnWave:
				nextWaveSpawnTimer -= Time.deltaTime;
				if (nextWaveSpawnTimer <= 0) {
					SpawnWave();
				}
				break;

			case State.SpawningWave:
				if (remainingEnemySpawnAmount > 0) {
					nextEnemySpawnTimer -= Time.deltaTime;
					if (nextEnemySpawnTimer <= 0) {
						nextEnemySpawnTimer = UnityEngine.Random.Range(0, 0.2f);
						Enemy.Create(spawnPosition + (Utils.GetRandomDirection() * UnityEngine.Random.Range(0f, 10f)));
						remainingEnemySpawnAmount--;

						if (remainingEnemySpawnAmount <= 0) {
							state = State.WaitingToSpawnWave;
							spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
							nextWaveSpawnPositionTransform.position = spawnPosition;
							nextWaveSpawnTimer = 10f;
						}
					}
				}
				break;
		}
	}

	private void SpawnWave() {
		remainingEnemySpawnAmount = 33 + (3 * waveNumber);
		state = State.SpawningWave;
		waveNumber++;
		OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
	}

	public int GetWaveNumber() {
		return waveNumber;
	}

	public float GetNextWaveSpawnTimer() {
		return nextWaveSpawnTimer;
	}

	public Vector3 GetSpawnPosition() {
		return spawnPosition;
	}
}
