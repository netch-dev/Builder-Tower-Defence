using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour {
	[SerializeField] private bool runOnlyOnce;
	[SerializeField] private float positionOffsetY;

	private SpriteRenderer spriteRenderer;
	private float precisionMultiplier = 5f;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void LateUpdate() {
		spriteRenderer.sortingOrder = (int)((-transform.position.y - positionOffsetY) * precisionMultiplier);

		if (runOnlyOnce) {
			Destroy(this);
		}
	}
}
