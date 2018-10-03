using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour {

    [SerializeField] Transform actor;
    [SerializeField] Vector3 offset;

    public void ChangeTarget(Transform newActor)
    {
        actor = newActor;
    }
	
	// Update is called once per frame
	void Update () {
        if (actor != null)
        {
        transform.position = actor.position + offset;

        }
		
	}
}
