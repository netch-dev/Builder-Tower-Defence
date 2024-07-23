using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject {
	public string nameString;
	public Transform prefab;
	public bool hasResourceGeneratorData;
	public List<ResourceGeneratorData> resourceGeneratorData;
	public Sprite sprite;
	public float minConstructionRadius;
	public ResourceAmount[] constructionResourceCostArray;
	public int healthAmountMax;

	private string constructionResourceCost;
	public string GetConstructionResourceCostString() {
		if (string.IsNullOrEmpty(constructionResourceCost)) {
			constructionResourceCost = "";
			foreach (ResourceAmount resourceAmount in constructionResourceCostArray) {
				constructionResourceCost += $"<color=#{resourceAmount.resourceType.colourHex}>{resourceAmount.resourceType.shortname}{resourceAmount.amount}</color> ";
			}
		}

		return constructionResourceCost;
	}
}
