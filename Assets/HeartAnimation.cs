using UnityEngine;
using System.Collections;

public class HeartAnimation: MonoBehaviour {

    public Vector3 rotationAngle;
    public float rotationSpeed;

	
	void Update () {

        transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);

    }
}
