using System;
using UnityEngine;

public class Building : MonoBehaviour {
	private BuildingTypeSO buildingType;
	private HealthSystem healthSystem;
	private Transform buildingDemolishButton;

	private void Awake() {
		buildingDemolishButton = transform.Find("BuildingDemolishButton");
		HideBuildingDemolishBtn();
	}

	private void Start() {
		buildingType = GetComponent<BuildingTypeHolder>().buildingType;

		healthSystem = GetComponent<HealthSystem>();

		healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

		healthSystem.OnDie += HealthSystem_OnDie;
	}

	private void HealthSystem_OnDie(object sender, EventArgs e) {
		Destroy(gameObject);
	}

	private void OnMouseEnter() {
		ShowBuildingDemolishBtn();
	}

	private void OnMouseExit() {
		HideBuildingDemolishBtn();
	}

	private void ShowBuildingDemolishBtn() {
		if (buildingDemolishButton != null) {
			buildingDemolishButton.gameObject.SetActive(true);
		}
	}

	private void HideBuildingDemolishBtn() {
		if (buildingDemolishButton != null) {
			buildingDemolishButton.gameObject.SetActive(false);
		}
	}
}
