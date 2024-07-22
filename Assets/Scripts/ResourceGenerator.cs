using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {
	private List<ResourceGeneratorData> resourceGeneratorDataList;
	private List<ResourceTimer> resourceTimerList;

	private void Awake() {
		resourceGeneratorDataList = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
		resourceTimerList = new List<ResourceTimer>();
	}

	private void Start() {
		bool hasResourcesNearby = false;

		foreach (ResourceGeneratorData resourceGeneratorData in resourceGeneratorDataList) {
			Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, resourceGeneratorData.resourceDetectionRadius);

			int nearbyResourceAmount = 0;
			foreach (Collider2D collider2D in collider2DArray) {
				ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
				if (resourceNode != null && resourceNode.resourceType == resourceGeneratorData.resourceType) {
					nearbyResourceAmount++;
					hasResourcesNearby = true;
				}
			}

			nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

			float timerMax = (resourceGeneratorData.timerMax / 2f) + (resourceGeneratorData.timerMax * (1 - ((float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount)));
			resourceTimerList.Add(new ResourceTimer(timerMax, resourceGeneratorData.resourceType));

			Debug.Log("ResourceGenerator: " + resourceGeneratorData.resourceType.name + " - " + nearbyResourceAmount + " - " + timerMax);
		}

		if (!hasResourcesNearby) enabled = false;
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
	}
}
