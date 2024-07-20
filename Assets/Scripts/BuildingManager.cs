using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
	[SerializeField] private Transform woodHarvesterPrefab;

	private Camera mainCamera;

	private void Start() {
		mainCamera = Camera.main;
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorldPostion = GetMouseWorldPosition();
			Instantiate(woodHarvesterPrefab, mouseWorldPostion, Quaternion.identity);
		}
	}

	private Vector3 GetMouseWorldPosition() {
		Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseWorldPosition.z = 0;
		return mouseWorldPosition;
	}
}
