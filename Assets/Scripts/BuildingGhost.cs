using Unity.VisualScripting;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {
	[SerializeField] private GameObject spriteGameObject;
	[SerializeField] private ResourceNearbyOverlay resourceNearbyOverlay;

	private SpriteRenderer spriteRenderer;

	private void Awake() {
		spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer>();

		Hide();
	}

	private void Start() {
		BuildingManager.Instance.OnSelectedBuildingChanged += BuildingManager_OnSelectedBuildingChanged;
	}

	private void Update() {
		transform.position = Utils.GetMouseWorldPosition();
	}

	private void BuildingManager_OnSelectedBuildingChanged(object sender, BuildingManager.OnSelectedBuildingChangedEventArgs e) {
		if (e.selectedBuildingType == null) {
			Hide();
			resourceNearbyOverlay.Hide();
		} else {
			Show(e.selectedBuildingType.sprite);

			//TODO: support multiple resources in the building ghost
			if (e.selectedBuildingType.hasResourceGeneratorData && e.selectedBuildingType.resourceGeneratorData.Count > 0) {
				resourceNearbyOverlay.Show(e.selectedBuildingType.resourceGeneratorData[0]);
			} else {
				resourceNearbyOverlay.Hide();
			}
		}
	}

	private void Show(Sprite ghostSprite) {
		spriteRenderer.sprite = ghostSprite;
		spriteGameObject.SetActive(true);
	}

	private void Hide() {
		spriteGameObject.SetActive(false);
	}
}
