using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
	private Camera mainCamera;
	private BuildingTypeListSO buildingTypeList;
	private BuildingTypeSO buildingType;

	private void Start() {
		mainCamera = Camera.main;

		buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
		buildingType = buildingTypeList.list[0];
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorldPostion = GetMouseWorldPosition();
			Instantiate(buildingType.prefab, mouseWorldPostion, Quaternion.identity);
		}

		if (Input.GetKeyDown(KeyCode.T)) {
			buildingType = buildingTypeList.list[0];
		}

		if (Input.GetKeyDown(KeyCode.Y)) {
			buildingType = buildingTypeList.list[1];
		}
	}

	private Vector3 GetMouseWorldPosition() {
		Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseWorldPosition.z = 0;
		return mouseWorldPosition;
	}
}
