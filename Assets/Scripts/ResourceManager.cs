using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
	public static ResourceManager Instance { get; private set; }

	private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

	public event EventHandler OnResourceAmountChanged;

	[SerializeField] private List<ResourceAmount> startingResourceAmountList;

	private void Awake() {
		Instance = this;

		resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

		ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

		foreach (ResourceTypeSO resourceType in resourceTypeList.list) {
			resourceAmountDictionary[resourceType] = 0;
		}

		foreach (ResourceAmount resourceAmount in startingResourceAmountList) {
			AddResource(resourceAmount.resourceType, resourceAmount.amount);
		}
	}

	public void AddResource(ResourceTypeSO resourceType, int amount) {
		resourceAmountDictionary[resourceType] += amount;

		OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
	}

	public int GetResourceAmount(ResourceTypeSO resourceType) {
		return resourceAmountDictionary[resourceType];
	}

	public bool CanAfford(ResourceAmount[] resourceAmountArray) {
		foreach (ResourceAmount resourceAmount in resourceAmountArray) {
			if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount) {
				continue;
			}

			return false;
		}

		return true;
	}

	public void SpendResources(ResourceAmount[] resourceAmountArray) {
		foreach (ResourceAmount resourceAmount in resourceAmountArray) {
			resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
		}

		OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
	}
}
