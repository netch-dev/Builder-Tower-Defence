using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishButton : MonoBehaviour {
	[SerializeField] private Button demolishButton;
	[SerializeField] private Building building;

	private void Awake() {
		demolishButton.onClick.AddListener(() => {
			BuildingTypeSO buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;
			foreach (ResourceAmount resourceAmount in buildingType.constructionResourceCostArray) {
				ResourceManager.Instance.AddResource(resourceAmount.resourceType, Mathf.FloorToInt(resourceAmount.amount * 0.6f));
			}

			Destroy(building.gameObject);
		});
	}
}
