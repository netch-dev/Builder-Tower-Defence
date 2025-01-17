using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
	public event EventHandler OnHealthAmountMaxChanged;
	public event EventHandler OnDamaged;
	public event EventHandler OnHealed;
	public event EventHandler OnDie;

	[SerializeField] private int healthAmountMax = 100;
	private int currentHealthAmount;

	private void Awake() {
		currentHealthAmount = healthAmountMax;
	}

	public void Damage(int damageAmount) {
		currentHealthAmount -= damageAmount;
		currentHealthAmount = Mathf.Clamp(currentHealthAmount, 0, healthAmountMax);
		OnDamaged?.Invoke(this, EventArgs.Empty);

		if (IsDead()) {
			OnDie?.Invoke(this, EventArgs.Empty);
		}
	}

	public void Heal(int healAmount) {
		currentHealthAmount += healAmount;
		currentHealthAmount = Mathf.Clamp(currentHealthAmount, 0, healthAmountMax);
		OnHealed?.Invoke(this, EventArgs.Empty);
	}

	public void HealFull() {
		currentHealthAmount = healthAmountMax;
		OnHealed?.Invoke(this, EventArgs.Empty);
	}

	public bool IsDead() {
		return currentHealthAmount == 0;
	}

	public bool IsFullHealth() {
		return currentHealthAmount == healthAmountMax;
	}
	public int GetHealthAmount() {
		return currentHealthAmount;
	}

	public int GetHealthAmountMax() {
		return healthAmountMax;
	}

	public float GetHealthAmountNormalized() {
		return (float)currentHealthAmount / healthAmountMax;
	}

	public void SetHealthAmountMax(int healthAmountMax, bool updateCurrentHealthAmount = false) {
		this.healthAmountMax = healthAmountMax;

		if (updateCurrentHealthAmount) currentHealthAmount = healthAmountMax;

		OnHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
	}
}
