using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {
	private List<ResourceGeneratorData> resourceGeneratorDataList;
	private List<ResourceTimer> resourceTimerList;

	private void Awake() {
		resourceGeneratorDataList = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
		resourceTimerList = new List<ResourceTimer>();
		SetupResourcerTimerList();
	}


	private void Start() {

	}

	private void SetupResourcerTimerList() {
		bool hasResourcesNearby = false;

		foreach (ResourceGeneratorData resourceGeneratorData in resourceGeneratorDataList) {
			int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);
			if (nearbyResourceAmount > 0) hasResourcesNearby = true;

			float timerMax = (resourceGeneratorData.timerMax / 2f) + (resourceGeneratorData.timerMax * (1 - ((float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount)));
			resourceTimerList.Add(new ResourceTimer(timerMax, resourceGeneratorData.resourceType));
		}

		if (!hasResourcesNearby) enabled = false;
	}

	public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position) {
		Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

		int nearbyResourceAmount = 0;
		foreach (Collider2D collider2D in collider2DArray) {
			ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
			if (resourceNode != null && resourceNode.resourceType == resourceGeneratorData.resourceType) {
				nearbyResourceAmount++;
			}
		}

		nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);
		return nearbyResourceAmount;
	}

	private void Update() {
		foreach (ResourceTimer resourceData in resourceTimerList) {
			resourceData.Tick(Time.deltaTime);
		}
	}

	private class ResourceTimer {
		private float timer;
		private float timerMax;
		private ResourceTypeSO resourceType;

		public ResourceTimer(float timerMax, ResourceTypeSO resourceType) {
			this.timer = timerMax;
			this.timerMax = timerMax;
			this.resourceType = resourceType;
		}

		public void Tick(float deltaTime) {
			timer -= deltaTime;
			if (timer <= 0) {
				timer += timerMax;
				ResourceManager.Instance.AddResource(resourceType, 1);
			}
		}

		public float GetTimerNormalized() {
			return 1 - (timer / timerMax);
		}

		public float GetAmountGeneratedPerSecond() {
			return 1 / timerMax;
		}
	}

	public ResourceGeneratorData GetResourceGeneratorData() {
		//TODO: add support for multiple resource entries
		return resourceGeneratorDataList[0];
	}

	public float GetTimerNormalized() {
		//TODO: add support for multiple resource entries
		return resourceTimerList[0].GetTimerNormalized();
	}

	public float GetAmountGeneratedPerSecond() {
		return resourceTimerList[0].GetAmountGeneratedPerSecond();
	}
}
