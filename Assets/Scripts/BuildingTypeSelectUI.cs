using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour {
	[SerializeField] private Transform buttonTemplate;
	[SerializeField] private Sprite arrowSprite;
	[SerializeField] private List<BuildingTypeSO> ignoredBuildingTypeList;

	private Dictionary<BuildingTypeSO, Transform> btnTypeTransformDictionary;
	private BuildingTypeListSO buildingTypeList;
	private Transform arrowBtn;

	private void Awake() {
		buttonTemplate.gameObject.SetActive(false);

		btnTypeTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();
		buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

		int index = 0;

		arrowBtn = Instantiate(buttonTemplate, transform);
		arrowBtn.gameObject.SetActive(true);

		float offsetAmount = 130f;
		arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

		arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
		arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);

		arrowBtn.GetComponent<Button>().onClick.AddListener(() => {
			BuildingManager.Instance.SetActiveBuildingType(null);
		});

		index++;

		foreach (BuildingTypeSO buildingType in buildingTypeList.list) {
			if (ignoredBuildingTypeList.Contains(buildingType)) continue;

			Transform btnTransform = Instantiate(buttonTemplate, transform);
			btnTransform.gameObject.SetActive(true);

			btnTypeTransformDictionary[buildingType] = btnTransform;

			btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

			btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

			btnTransform.GetComponent<Button>().onClick.AddListener(() => {
				BuildingManager.Instance.SetActiveBuildingType(buildingType);
			});

			index++;
		}
	}

	private void Start() {
		BuildingManager.Instance.OnSelectedBuildingChanged += BuildingManager_OnSelectedBuildingChanged;

		UpdateActiveBuildingTypeButton();
	}

	private void BuildingManager_OnSelectedBuildingChanged(object sender, EventArgs e) {
		UpdateActiveBuildingTypeButton();
	}

	private void UpdateActiveBuildingTypeButton() {
		arrowBtn.Find("selected").gameObject.SetActive(false);

		foreach (BuildingTypeSO buildingType in btnTypeTransformDictionary.Keys) {
			Transform btnTransform = btnTypeTransformDictionary[buildingType];
			btnTransform.Find("selected").gameObject.SetActive(false);
		}

		BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
		if (activeBuildingType == null) {
			arrowBtn.Find("selected").gameObject.SetActive(true);
		} else {
			btnTypeTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
		}
	}
}
