using System;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class BuildingManager : MonoBehaviour {
	public static BuildingManager Instance { get; private set; }

	public EventHandler<OnSelectedBuildingChangedEventArgs> OnSelectedBuildingChanged;
	public class OnSelectedBuildingChangedEventArgs : EventArgs {
		public BuildingTypeSO selectedBuildingType;
	}

	[SerializeField] private Building hqBuilding;

	private BuildingTypeSO selectedBuildingType;

	private void Awake() {
		Instance = this;
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			if (selectedBuildingType == null) return;
			if (!ResourceManager.Instance.CanAfford(selectedBuildingType.constructionResourceCostArray)) {
				TooltipUI.Instance.Show($"Not enough resources! {selectedBuildingType.GetConstructionResourceCostString()}", new TooltipUI.TooltipTimer { timer = 2f });
				return;
			}

			Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition();

			if (!CanSpawnBuilding(selectedBuildingType, mouseWorldPosition, out string errorMessage)) {
				TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f });
				return;
			}

			ResourceManager.Instance.SpendResources(selectedBuildingType.constructionResourceCostArray);

			Instantiate(selectedBuildingType.prefab, mouseWorldPosition, Quaternion.identity);
		}

		if (Input.GetMouseButtonDown(1)) {
			SetActiveBuildingType(null);
		}

		//TODO: Remove this debug code
		if (Input.GetKeyDown(KeyCode.T)) {
			Enemy.Create(Utils.GetMouseWorldPosition() + (Utils.GetRandomDirection() * 5f));
		}
	}

	public void SetActiveBuildingType(BuildingTypeSO buildingType) {
		selectedBuildingType = buildingType;
		OnSelectedBuildingChanged?.Invoke(this, new OnSelectedBuildingChangedEventArgs { selectedBuildingType = buildingType });
	}

	public BuildingTypeSO GetActiveBuildingType() {
		return selectedBuildingType;
	}

	private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage) {
		BoxCollider2D buildingCollider = buildingType.prefab.GetComponent<BoxCollider2D>();

		// Make sure the area is clear
		Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)buildingCollider.offset, buildingCollider.size, 0);
		bool isAreaClear = collider2DArray.Length == 0;
		if (!isAreaClear) {
			errorMessage = "Area is not clear!";
			return false;
		}

		// Make sure no buildings of the same type are nearby
		collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
		foreach (Collider2D collider2D in collider2DArray) {
			collider2D.TryGetComponent(out BuildingTypeHolder buildingTypeHolder);
			if (buildingTypeHolder != null) {
				if (buildingTypeHolder.buildingType == buildingType) {
					errorMessage = "Too close to another building of the same type!";
					return false;
				}
			}
		}

		// Make sure this building is not being placed too far from other buildings
		// The game starts with a HQ, so there will always be a building nearby
		float maxConstructionRadius = 25f;
		collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
		foreach (Collider2D collider2D in collider2DArray) {
			collider2D.TryGetComponent(out BuildingTypeHolder buildingTypeHolder);
			if (buildingTypeHolder != null) {
				errorMessage = string.Empty;
				return true;
			}
		}

		errorMessage = "Too far from other buildings!";
		return false;
	}

	public Building GetHQBuilding() {
		return hqBuilding;
	}
}
