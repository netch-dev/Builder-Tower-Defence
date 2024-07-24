using System;
using UnityEngine;

public class HealthBar : MonoBehaviour {
	[SerializeField] private HealthSystem healthSystem;
	[SerializeField] private Transform barTransform;
	[SerializeField] private Transform separatorContainer;
	[SerializeField] private Transform separatorTemplate;

	private void Start() {
		ConstructHealthbarSeparators();

		healthSystem.OnHealthAmountMaxChanged += HealthSystem_OnHealthAmountMaxChanged;
		healthSystem.OnDamaged += HealthSystem_OnDamaged;
		healthSystem.OnHealed += HealthSystem_OnHealed;
		UpdateBar();
		UpdateHealthbarVisibility();
	}

	private void HealthSystem_OnHealthAmountMaxChanged(object sender, EventArgs e) {
		ConstructHealthbarSeparators();
	}

	private void ConstructHealthbarSeparators() {
		separatorTemplate.gameObject.SetActive(false);

		foreach (Transform separatorTransform in separatorContainer) {
			if (separatorTransform == separatorTemplate) continue;
			Destroy(separatorTransform.gameObject);
		}

		int healthAmountPerSeparator = 10;
		float barSize = 3f;
		float barOneHealthAmountSize = barSize / healthSystem.GetHealthAmountMax();
		int healthSeparatorCount = Mathf.FloorToInt(healthSystem.GetHealthAmountMax() / healthAmountPerSeparator);
		for (int i = 1; i < healthSeparatorCount; i++) {
			Transform separatorTransform = Instantiate(separatorTemplate, separatorContainer);
			separatorTransform.gameObject.SetActive(true);
			separatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * healthAmountPerSeparator, 0, 0);
		}
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
