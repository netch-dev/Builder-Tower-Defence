using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour {
	public static BuildingManager Instance { get; private set; }

	private Camera mainCamera;
	private BuildingTypeListSO buildingTypeList;
	private BuildingTypeSO selectedBuildingType;

	private void Awake() {
		Instance = this;
		buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
	}

	private void Start() {
		mainCamera = Camera.main;
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			if (selectedBuildingType == null) return;
			Vector3 mouseWorldPostion = GetMouseWorldPosition();
			Instantiate(selectedBuildingType.prefab, mouseWorldPostion, Quaternion.identity);
		}
	}

	private Vector3 GetMouseWorldPosition() {
		Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseWorldPosition.z = 0;
		return mouseWorldPosition;
	}

	public void SetActiveBuildingType(BuildingTypeSO buildingType) {
		selectedBuildingType = buildingType;
	}

	public BuildingTypeSO GetActiveBuildingType() {
		return selectedBuildingType;
	}
}
