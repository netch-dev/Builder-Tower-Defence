using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
	public static ResourceManager Instance { get; private set; }

	private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

	public event EventHandler OnResourceAmountChanged;

	private void Awake() {
		Instance = this;

		resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

		ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

		foreach (ResourceTypeSO resourceType in resourceTypeList.list) {
			resourceAmountDictionary[resourceType] = 0;
		}
	}

	public void AddResource(ResourceTypeSO resourceType, int amount) {
		resourceAmountDictionary[resourceType] += amount;

		OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
	}

	public int GetResourceAmount(ResourceTypeSO resourceType) {
		return resourceAmountDictionary[resourceType];
	}

	private void TestLogResourceAmounts() {
		foreach (ResourceTypeSO resourceType in resourceAmountDictionary.Keys) {
			Debug.Log(resourceType.nameString + ": " + resourceAmountDictionary[resourceType]);
		}
	}
}
