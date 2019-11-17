using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	public float speed = 10.0f;
	public int damage = 1;

	void Update() {
		transform.Translate(0, 0, speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("oh no you're hit");
		Destroy(this.gameObject);
	}
}
