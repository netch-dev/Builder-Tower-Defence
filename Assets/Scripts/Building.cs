using System;
using UnityEngine;

public class Building : MonoBehaviour {
	private BuildingTypeSO buildingType;
	private HealthSystem healthSystem;

	private void Start() {
		buildingType = GetComponent<BuildingTypeHolder>().buildingType;

		healthSystem = GetComponent<HealthSystem>();

		healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

		healthSystem.OnDie += HealthSystem_OnDie;
	}

	private void HealthSystem_OnDie(object sender, EventArgs e) {
		Destroy(gameObject);
	}
}
