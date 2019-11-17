using UnityEngine;
using System.Collections;

public class RayShooter : MonoBehaviour {
	//audio shit
	[SerializeField] private AudioSource soundSource; // what's making the sound
	[SerializeField] private AudioClip hitWallSound;  // the two sound files
	[SerializeField] private AudioClip hitEnemySound; // that play via dif events

	private Camera _camera;

	//public string playerTag = "Player";

	void Start() {
		_camera = GetComponent<Camera>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void OnGUI() {
		int size = 12;
		float posX = _camera.pixelWidth/2 - size/4;
		float posY = _camera.pixelHeight/2 - size/2;
		GUI.Label(new Rect(posX, posY, size, size), "*");
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 point = new Vector3(_camera.pixelWidth/2, _camera.pixelHeight/2, 0);
			Ray ray = _camera.ScreenPointToRay(point);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				GameObject hitObject = hit.transform.gameObject;
				ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
				if (target != null) { // if the target isn't null, the player has hit an enemy, so...
					target.ReactToHit();
					soundSource.PlayOneShot(hitEnemySound); // call playoneshot() to play the Hit An Enemy sound, as defined in the inspector
				} else {
					StartCoroutine(SphereIndicator(hit.point));
					soundSource.PlayOneShot(hitWallSound);
					//soundSource.clip = hitWallSound; soundSource.Play();
				}
			}
		}
	}

	private IEnumerator SphereIndicator(Vector3 pos) {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = pos;

		yield return new WaitForSeconds(1);

		Destroy(sphere);
	}
}