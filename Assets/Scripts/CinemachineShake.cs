using Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour {
	public static CinemachineShake Instance { get; private set; }

	private CinemachineVirtualCamera cinemachineVirtualCamera;
	private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

	private float timer;
	private float timerMax;
	private float startingIntensity;

	private void Awake() {
		Instance = this;
		cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
		cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
	}

	private void Update() {
		if (timer < timerMax) {
			timer += Time.deltaTime;
			float amplitude = Mathf.Lerp(startingIntensity, 0f, timer / timerMax);
			cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
		}
	}

	public void ShakeCamera(float intensity, float timerMax) {
		this.timerMax = timerMax;
		timer = 0f;

		this.startingIntensity = intensity;
		cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
	}
}
