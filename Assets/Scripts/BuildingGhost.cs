using UnityEngine;

public class BuildingGhost : MonoBehaviour {
	[SerializeField] private GameObject spriteGameObject;

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
		} else {
			Show(e.selectedBuildingType.sprite);
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
