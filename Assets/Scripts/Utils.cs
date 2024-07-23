using UnityEngine;

public static class Utils {

	private static Camera mainCamera;

	public static Vector3 GetMouseWorldPosition() {
		if (mainCamera == null) mainCamera = Camera.main;

		Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseWorldPosition.z = 0;
		return mouseWorldPosition;
	}

	public static Vector3 GetRandomDirection() {
		return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
	}
}
