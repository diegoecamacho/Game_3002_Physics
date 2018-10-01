using UnityEngine;
using UnityEngine.UI;

public class Forces_Week2 : MonoBehaviour
{


    enum STATES {
        STATIONARY,
        ACCELERATING,
        DECELERATING
    }


    STATES currState = STATES.STATIONARY;
    public Slider slider;

    public Transform destinationTransform = null;
    public float acceleration = 0f;
    public float forceDuration = 5f;

    float pushForce = 0;

    public float distanceRatio = 0.5f;

    public float deadZone = 0.4f;

    [Header("Coefficient of Friction")]
    public float coFriction = 0.6f;

    private Rigidbody m_rb = null;
    private bool m_isRunning = false;
    private float m_timeElapsed = 0f;

    private float m_totalDistance = 0f;

    private float originalVelocity = 0f;



    float angle = 0;



    // Use this for initialization
    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();

        m_totalDistance = Vector3.Distance(transform.position, destinationTransform.position);

        coFriction = GetComponent<Collider>().material.dynamicFriction;

        acceleration = CalculateAcceleration() * 2.0f ;
    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime;
        slider.value = Remap(Mathf.Sin(angle), -1, 1, 0, 2);

        if (Input.GetKeyDown(KeyCode.Space))
        {
           //if (slider.value  > (1f - deadZone) && slider.value < (1f + deadZone))
           //{
		   //
           //    pushForce = 1;
		   //
           //}
           //else{
                pushForce = slider.value;
            //}

            Debug.Log(pushForce);
            currState = STATES.ACCELERATING;
        }
    }

    private void FixedUpdate()
    {
        switch (currState)
        {
            case STATES.ACCELERATING:
                {
                    Vector3 force = transform.forward * acceleration * m_rb.mass;
                    m_rb.AddForce(force * pushForce, ForceMode.Force);
                    if (Vector3.Distance(transform.position, destinationTransform.position) <= m_totalDistance / 2)
                    {
                     
                        currState = STATES.DECELERATING; 
                    }
                    break;
                }
        }
    }

    float CalculateFinalVelocity(){

        float frictionalAcc = 0f;
        float mass =  m_rb.mass;
        float forceNormal = coFriction * Physics.gravity.y * mass;
        frictionalAcc = forceNormal / mass;

        float IntialVelocityMidpoint = (0 - (2 * frictionalAcc * (m_totalDistance / 2f)));
        IntialVelocityMidpoint = Mathf.Sqrt(IntialVelocityMidpoint);
        return IntialVelocityMidpoint;
    }

    float CalculateAcceleration(){
        float acc = 0f;
        float finalVelocity = CalculateFinalVelocity();
        acc = (Mathf.Pow(finalVelocity, 2) - 0) / (2 * (m_totalDistance / 2));
        return acc;

    }

    float Remap(float value, float minRange1, float maxRange1, float minRange2, float maxRange2){
        return minRange2 + (value - minRange1) * ((maxRange2 - minRange2) / (maxRange1 - minRange1));
    }

}
