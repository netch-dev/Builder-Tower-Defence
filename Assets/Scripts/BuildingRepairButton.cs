using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairButton : MonoBehaviour {
	[SerializeField] private Button repairButton;
	[SerializeField] private HealthSystem healthSystem;
	[SerializeField] private ResourceTypeSO repairResourceType;

	private void Awake() {
		repairButton.onClick.AddListener(() => {
			int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
			int repairCost = missingHealth / 2;
			if (ResourceManager.Instance.CanAfford(new ResourceAmount { resourceType = repairResourceType, amount = repairCost })) {
				ResourceManager.Instance.TakeResource(repairResourceType, repairCost);
				healthSystem.HealFull();
			} else {
				TooltipUI.Instance.Show("Cannot afford repair cost!", new TooltipUI.TooltipTimer { timer = 2f });
			}
		});
	}
}
