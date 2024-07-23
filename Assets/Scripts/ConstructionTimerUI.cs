using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour {
	[SerializeField] private BuildingConstruction buildingConstruction;
	[SerializeField] private Image constructionProgressImage;

	private void Update() {
		constructionProgressImage.fillAmount = buildingConstruction.GetConstructionTimerNormalized();
	}
}
