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
			if (!ResourceManager.Instance.CanAfford(selectedBuildingType.constructionResourceCostArray)) return;

			Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition();

			if (!CanSpawnBuilding(selectedBuildingType, mouseWorldPosition)) return;

			ResourceManager.Instance.SpendResources(selectedBuildingType.constructionResourceCostArray);

			Instantiate(selectedBuildingType.prefab, mouseWorldPosition, Quaternion.identity);
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

	private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position) {
		BoxCollider2D buildingCollider = buildingType.prefab.GetComponent<BoxCollider2D>();

		// Make sure the area is clear
		Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)buildingCollider.offset, buildingCollider.size, 0);
		bool isAreaClear = collider2DArray.Length == 0;
		if (!isAreaClear) return false;

		// Make sure no buildings of the same type are nearby
		collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
		foreach (Collider2D collider2D in collider2DArray) {
			collider2D.TryGetComponent(out BuildingTypeHolder buildingTypeHolder);
			if (buildingTypeHolder != null) {
				if (buildingTypeHolder.buildingType == buildingType) return false;
			}
		}

		// Make sure this building is not being placed too far from other buildings
		// The game starts with an HQ, so there will always be a building nearby
		float maxConstructionRadius = 25f;
		collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
		foreach (Collider2D collider2D in collider2DArray) {
			collider2D.TryGetComponent(out BuildingTypeHolder buildingTypeHolder);
			if (buildingTypeHolder != null) return true;
		}

		return false;
	}
}
