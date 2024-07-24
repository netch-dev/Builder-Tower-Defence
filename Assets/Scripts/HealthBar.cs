using System;
using UnityEngine;

public class HealthBar : MonoBehaviour {
	[SerializeField] private HealthSystem healthSystem;
	[SerializeField] private Transform barTransform;

	private void Start() {
		healthSystem.OnDamaged += HealthSystem_OnDamaged;
		healthSystem.OnHealed += HealthSystem_OnHealed;
		UpdateBar();
		UpdateHealthbarVisibility();
	}

	private void HealthSystem_OnHealed(object sender, EventArgs e) {
		UpdateBar();
		UpdateHealthbarVisibility();
	}

	private void HealthSystem_OnDamaged(object sender, EventArgs e) {
		UpdateBar();
		UpdateHealthbarVisibility();
	}

	private void UpdateBar() {
		barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
	}

	private void UpdateHealthbarVisibility() {
		if (healthSystem.IsFullHealth()) {
			gameObject.SetActive(false);
		} else {
			gameObject.SetActive(true);
		}
	}
}
