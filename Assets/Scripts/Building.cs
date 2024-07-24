using System;
using UnityEngine;

public class Building : MonoBehaviour {
	private BuildingTypeSO buildingType;
	private HealthSystem healthSystem;
	private Transform buildingDemolishButton;
	private Transform buildingRepairButton;

	private void Awake() {
		buildingDemolishButton = transform.Find("BuildingDemolishButton");
		buildingRepairButton = transform.Find("BuildingRepairButton");
		HideBuildingDemolishBtn();
		HideBuildingRepairBtn();
	}

	private void Start() {
		buildingType = GetComponent<BuildingTypeHolder>().buildingType;

		healthSystem = GetComponent<HealthSystem>();

		healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
		healthSystem.OnDamaged += HealthSystem_OnDamaged;
		healthSystem.OnHealed += HealthSystem_OnHealed;

		healthSystem.OnDie += HealthSystem_OnDie;
	}

	private void HealthSystem_OnDamaged(object sender, EventArgs e) {
		ShowBuildingRepairBtn();
	}

	private void HealthSystem_OnHealed(object sender, EventArgs e) {
		if (healthSystem.IsFullHealth()) {
			HideBuildingRepairBtn();
		}
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

	private void ShowBuildingRepairBtn() {
		if (buildingRepairButton != null) {
			buildingRepairButton.gameObject.SetActive(true);
		}
	}

	private void HideBuildingRepairBtn() {
		if (buildingRepairButton != null) {
			buildingRepairButton.gameObject.SetActive(false);
		}
	}
}
