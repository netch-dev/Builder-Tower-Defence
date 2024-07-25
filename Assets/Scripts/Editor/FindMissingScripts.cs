using System.IO;
using UnityEditor;
using UnityEngine;

public class FindMissingScripts : EditorWindow {
	[MenuItem("Project Tools/Find Missing Scripts in Prefabs")]
	public static void ShowWindow() {
		GetWindow(typeof(FindMissingScripts));
	}

	private void OnGUI() {
		if (GUILayout.Button("Find Missing Scripts in Prefabs")) {
			FindMissingScriptsInPrefabs();
		}
	}

	private static void FindMissingScriptsInPrefabs() {
		string[] allPrefabs = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
		int prefabCount = 0;
		foreach (string prefabPath in allPrefabs) {
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath.Replace(Application.dataPath, "Assets"));
			if (prefab != null) {
				Component[] components = prefab.GetComponentsInChildren<Component>(true);
				foreach (Component component in components) {
					if (component == null) {
						Debug.Log($"<color=red>Missing script</color> in prefab '<b><color=blue>{prefab.name}</color></b>' at path: {prefabPath.Replace(Application.dataPath, "Assets")}", prefab);
						prefabCount++;
						break;
					}
				}
			}
		}

		Debug.Log($"Finished checking prefabs. Found <b>{prefabCount}</b> prefabs with missing scripts.");
	}
}
