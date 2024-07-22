using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private RectTransform backgroundRectTransform;

	[SerializeField] private RectTransform canvasRectTransform;

	private RectTransform rootRectTransfrom;

	private void Awake() {
		rootRectTransfrom = GetComponent<RectTransform>();
		SetText("hi there!");
	}

	private void Update() {
		Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

		// Check if the tooltip has left the screen
		if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width) {
			anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
		}
		if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height) {
			anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
		}

		rootRectTransfrom.anchoredPosition = anchoredPosition;
	}

	private void SetText(string tooltipText) {
		text.SetText(tooltipText);
		text.ForceMeshUpdate(); // Force the update to get the correct size below

		Vector2 textSize = text.GetRenderedValues(false);
		Vector2 padding = new Vector2(8, 8);
		backgroundRectTransform.sizeDelta = textSize + padding;
	}
}
