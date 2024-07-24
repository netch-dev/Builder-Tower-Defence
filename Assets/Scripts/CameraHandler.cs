using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
	public static CameraHandler Instance { get; private set; }

	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
	[SerializeField] private PolygonCollider2D cameraBoundsCollider2D;
	[SerializeField] private float moveSpeed = 30f;

	private float orthographicSize;
	private float targetOrthographicSize;
	private bool edgeScrolling;

	private void Awake() {
		Instance = this;

		edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
	}

	private void Start() {
		orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
		targetOrthographicSize = orthographicSize;
	}
	private void Update() {
		HandleMovement();

		HandleZoom();
	}

	private void HandleMovement() {
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		if (edgeScrolling) {
			float edgeScrollingSize = 20f;
			if (Input.mousePosition.x > Screen.width - edgeScrollingSize) {
				x = 1f;
			} else if (Input.mousePosition.x < edgeScrollingSize) {
				x = -1f;
			}
			if (Input.mousePosition.y > Screen.height - edgeScrollingSize) {
				y = 1f;
			} else if (Input.mousePosition.y < edgeScrollingSize) {
				y = -1f;
			}
		}

		Vector3 moveDir = new Vector3(x, y).normalized;

		Vector3 movementVector = transform.position + (moveDir * moveSpeed * Time.deltaTime);
		if (cameraBoundsCollider2D.bounds.Contains(movementVector)) {
			transform.position = movementVector;
		}
	}

	private void HandleZoom() {
		float zoomAmount = 2f;
		targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

		float minOrthographicSize = 10f;
		float maxOrthographicSize = 30f;
		targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

		// Lerp to target orthographic size to make it smooth
		float zoomSpeed = 5f;
		orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

		cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
	}

	public void SetEdgeScrolling(bool edgeScrolling) {
		this.edgeScrolling = edgeScrolling;
		PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
	}

	public bool GetEdgeScrolling() {
		return edgeScrolling;
	}
}
