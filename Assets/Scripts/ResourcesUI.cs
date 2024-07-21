using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour {
	[SerializeField] private Transform resourceTemplate;

	private ResourceTypeListSO resourceTypeList;
	private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;

	private void Awake() {
		resourceTemplate.gameObject.SetActive(false);

		resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

		resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

		int count = 0;
		foreach (ResourceTypeSO resourceType in resourceTypeList.list) {
			Transform resourceTransform = Instantiate(resourceTemplate, transform);
			resourceTransform.gameObject.SetActive(true);

			float offsetAmount = 160;
			resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-offsetAmount * count, 0);
			count++;

			resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;

			resourceTypeTransformDictionary[resourceType] = resourceTransform;
		}
	}

	private void Start() {
		UpdateResourceAmount();
		ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
	}

	private void ResourceManager_OnResourceAmountChanged(object sender, EventArgs e) {
		UpdateResourceAmount();
	}

	private void UpdateResourceAmount() {
		foreach (ResourceTypeSO resourceType in resourceTypeList.list) {
			int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
			resourceTypeTransformDictionary[resourceType].Find("text").GetComponent<TextMeshProUGUI>().text = resourceAmount.ToString("N0");
		}
	}
}
