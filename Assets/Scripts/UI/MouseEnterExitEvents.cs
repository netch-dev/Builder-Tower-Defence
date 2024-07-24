using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseEnterExitEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public event EventHandler OnMouseEnterEvent;
	public event EventHandler OnMouseExitEvent;

	public void OnPointerEnter(PointerEventData eventData) {
		OnMouseEnterEvent?.Invoke(this, EventArgs.Empty);
	}

	public void OnPointerExit(PointerEventData eventData) {
		OnMouseExitEvent?.Invoke(this, EventArgs.Empty);
	}
}
