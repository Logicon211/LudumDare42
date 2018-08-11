using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRobotBullet : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * 1000f * Time.deltaTime);
	}
}
