using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {
	[SerializeField] private SoundManager soundManager;
	[SerializeField] private MusicManager musicManager;

	private TextMeshProUGUI soundVolumeText;
	private TextMeshProUGUI musicVolumeText;

	private void Awake() {
		soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
		musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();

		transform.Find("soundIncreaseButton").GetComponent<Button>().onClick.AddListener(() => {
			soundManager.IncreaseVolume();
			UpdateSoundText();
		});
		transform.Find("soundDecreaseButton").GetComponent<Button>().onClick.AddListener(() => {
			soundManager.DecreaseVolume();
			UpdateSoundText();
		});

		transform.Find("musicIncreaseButton").GetComponent<Button>().onClick.AddListener(() => {
			musicManager.IncreaseVolume();
			UpdateMusicText();
		});
		transform.Find("musicDecreaseButton").GetComponent<Button>().onClick.AddListener(() => {
			musicManager.DecreaseVolume();
			UpdateMusicText();
		});

		transform.Find("mainMenuButton").GetComponent<Button>().onClick.AddListener(() => {
			Time.timeScale = 1f;
			GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
		});
	}

	private void Start() {
		UpdateSoundText();
		UpdateMusicText();
		gameObject.SetActive(false);
	}

	private void UpdateSoundText() {
		soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume() * 10).ToString());
	}

	private void UpdateMusicText() {
		musicVolumeText.SetText(Mathf.RoundToInt(musicManager.GetVolume() * 10).ToString());
	}

	public void ToggleVisible() {
		gameObject.SetActive(!gameObject.activeSelf);

		Time.timeScale = gameObject.activeSelf ? 0f : 1f;
	}
}
