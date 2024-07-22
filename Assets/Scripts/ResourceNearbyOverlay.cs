using TMPro;
using UnityEngine;

public class ResourceNearbyOverlay : MonoBehaviour {
	private ResourceGeneratorData resourceGeneratorData;
	private TextMeshPro text;

	private void Awake() {
		text = transform.Find("text").GetComponent<TextMeshPro>();
		Hide();
	}

	public void Show(ResourceGeneratorData resourceGeneratorData) {
		this.resourceGeneratorData = resourceGeneratorData;
		gameObject.SetActive(true);

		transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	private void Update() {
		int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position);
		float percent = Mathf.RoundToInt(nearbyResourceAmount / (float)resourceGeneratorData.maxResourceAmount * 100);
		text.SetText(percent.ToString() + "%");
	}
}
