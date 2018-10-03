using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KinematicPhysicsSim : MonoBehaviour {

    enum State
    {
        SLIDER_MOVEMENT,
        BALL_MOVEMENT
    }

    State currState = State.SLIDER_MOVEMENT;

    [Header("Slider Settings")]

    public float sliderStep = 0.05f;
    public float deadZone = 0.1f;

    [Header("Ball Settings")]

    public float desiredTime = 10.0f;

    public Transform destinationTransform;

   

    float tempValue = 0.0f;
    int sliderDir = 1;



    [Header("Internal Values")]

    [SerializeField] Slider slider;

    public float acceleration = 0.0f;
    private float timeElapsed = 0.0f;
    bool isRunning = false;
    private new Rigidbody rigidbody;


	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        acceleration = CalculateAcceleration();
       
    }
	
	// Update is called once per frame
	void Update () {

        switch (currState)
        {
            case State.SLIDER_MOVEMENT:
                SliderMovement();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    acceleration = (1 - Mathf.Abs(slider.value)) * acceleration;
                    currState = State.BALL_MOVEMENT;
                }
                break;
            case State.BALL_MOVEMENT:
                isRunning = true;
                break;
            default:
                break;
        }
            
        
       
       

    }

    private void SliderMovement()
    {
        slider.value =  Mathf.Lerp(slider.value, slider.maxValue * sliderDir , sliderStep);

        if (slider.value > slider.maxValue - deadZone) {
            sliderDir = -1;
        }
        else {
         if (slider.value < slider.minValue + deadZone) {
                sliderDir = 1;
         }
        }
    }

    float CalculateAcceleration()
    {
        float acc;

        float distance = Vector3.Distance(transform.position, destinationTransform.position);

        float top = 2 * (distance - rigidbody.velocity.z * desiredTime);

        acc = top / Mathf.Pow(desiredTime, 2);

        return acc;
    }

    private void FixedUpdate()
    {
        if (isRunning)
        {
            timeElapsed += Time.fixedDeltaTime;
            rigidbody.AddForce(transform.forward * acceleration, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Renderer>().material.color = Color.green;
        Debug.Log("Time Elapsed: " + timeElapsed);
        Debug.Log("Velocity: " + rigidbody.velocity);
        isRunning = false;
        
    }
}
