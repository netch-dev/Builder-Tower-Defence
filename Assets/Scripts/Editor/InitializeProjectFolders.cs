#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public static class InitializeProjectFolders {
	// List of required folders
	private static readonly string[] requiredFolders = {
		"Assets/Scripts",
		"Assets/Materials",
		"Assets/Models",
		"Assets/Prefabs",
		"Assets/Textures",
		"Assets/Animations",
		"Assets/Audio",
		"Assets/Resources",
		"Assets/Shaders",
		"Assets/Scenes"
	};

	[MenuItem("Project Tools/Initialize Project Folders")]
	public static void CreateRequiredFolders() {
		foreach (string folder in requiredFolders) {
			if (!Directory.Exists(folder)) {
				Directory.CreateDirectory(folder);
				Debug.Log($"Created folder: {folder}");
			} else {
				Debug.Log($"Folder already exists: {folder}");
			}
		}

		AssetDatabase.Refresh();
	}
}
#endif