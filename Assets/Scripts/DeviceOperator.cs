using System.Collections;
using UnityEngine;

public class DeviceOperator : MonoBehaviour {

	private const string INTERACTKEY = "Fire3";
	private const string USEFUNCTION = "Operate";

	public float useRange = 1.5f;

	void Update () {
		if (Input.GetButtonDown (INTERACTKEY)) {
			Collider[] hitColliders = Physics.OverlapSphere (transform.position, useRange);
			foreach (Collider hitCollider in hitColliders) {
				Vector3 direction = hitCollider.transform.position - transform.position;
				if (Vector3.Dot (transform.forward, direction.normalized) >.5f) {
					hitCollider.SendMessage (USEFUNCTION, SendMessageOptions.DontRequireReceiver);
					Debug.Log("use sent");
				}
			}
		}
	}

public void OnDrawGizmos() {
	Gizmos.color = Color.yellow;
	Gizmos.DrawWireSphere(transform.position, useRange);
}

}