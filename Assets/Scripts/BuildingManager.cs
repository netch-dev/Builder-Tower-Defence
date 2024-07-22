using System;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class BuildingManager : MonoBehaviour {
	public static BuildingManager Instance { get; private set; }

	public EventHandler<OnSelectedBuildingChangedEventArgs> OnSelectedBuildingChanged;
	public class OnSelectedBuildingChangedEventArgs : EventArgs {
		public BuildingTypeSO selectedBuildingType;
	}

	private BuildingTypeSO selectedBuildingType;

	private void Awake() {
		Instance = this;
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			if (selectedBuildingType == null) return;
			Vector3 mouseWorldPostion = Utils.GetMouseWorldPosition();
			Instantiate(selectedBuildingType.prefab, mouseWorldPostion, Quaternion.identity);
		}

		if (Input.GetMouseButtonDown(1)) {
			SetActiveBuildingType(null);
		}
	}

	public void SetActiveBuildingType(BuildingTypeSO buildingType) {
		selectedBuildingType = buildingType;
		OnSelectedBuildingChanged?.Invoke(this, new OnSelectedBuildingChangedEventArgs { selectedBuildingType = buildingType });
	}

	public BuildingTypeSO GetActiveBuildingType() {
		return selectedBuildingType;
	}
}
