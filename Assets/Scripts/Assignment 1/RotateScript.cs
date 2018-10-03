using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour {

    [SerializeField] float fovAngle = 0;


    bool enabled = false;
    public bool Enabled
    {
        get
        {
            return enabled;
        }

        set
        {
            enabled = value;
        }
    }


    float angle = 0;
    float sinAngle;
    float clampedAngle;
    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            angle = angle - (50 * Time.deltaTime);
            angle = Mathf.Clamp(angle, -fovAngle, fovAngle);
            transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            angle = angle + (50 * Time.deltaTime);
            angle = Mathf.Clamp(angle, -fovAngle, fovAngle);
            transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);

        }


        

		
	}
}
