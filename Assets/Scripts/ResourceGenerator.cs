using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {

	private BuildingTypeSO buildingType;
	private List<ResourceTimer> resourceTimers;

	private void Awake() {
		buildingType = GetComponent<BuildingTypeHolder>().buildingType;
		resourceTimers = new List<ResourceTimer>();
		foreach (ResourceGeneratorData resourceData in buildingType.resourceGeneratorData) {
			resourceTimers.Add(new ResourceTimer(resourceData.timerMax, resourceData.resourceType));
		}
	}

	private void Update() {
		foreach (ResourceTimer resourceData in resourceTimers) {
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
				AddResource();
			}
		}

		public void AddResource() {
			ResourceManager.Instance.AddResource(resourceType, 1);
		}
	}
}
