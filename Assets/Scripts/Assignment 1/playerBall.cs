using UnityEngine;
using UnityEngine.UI;

public class playerBall : MonoBehaviour
{
    [Header("Slider Options")]
    public Slider slider;
    public float sliderSpeed = 1f;
    public float sliderDeadzone = 0.2f;

    [Header("Destination Location")]
    public Transform destinationTransform = null;

    [Header("Behaviour Parameters")]
    private Rigidbody m_rb = null;
    private bool m_isRunning = false;
    private float m_timeElapsed = 0f;

    [Header("Physics Equation Variables ")]
    private float coFriction;
    private float m_totalDistance = 0f;
    private float acceleration = 0f;
    private float pushForce = 0;

    private float Sinwave = 0;
    private bool activePlayer;

    // Use this for initialization
    private void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();

        if (destinationTransform != null)
        {
            m_totalDistance = Vector3.Distance(transform.position, destinationTransform.position);

            coFriction = GetComponent<Collider>().material.dynamicFriction;

            acceleration = CalculateFinalVelocity();

        }

      
    }

    // Update is called once per frame
    private void Update()
    {
        if (activePlayer)
        {
            Sinwave += Time.deltaTime * sliderSpeed;
            slider.value = Remap(Mathf.Sin(Sinwave), -1, 1, slider.minValue, slider.maxValue);
        }
    }

    public void EnableBall(Slider playerSlider , Transform destination)
    {
        activePlayer = true;
        slider = playerSlider;
        destinationTransform = destination;
    }

    public void Activate()
    {
        if (slider.value >= ((slider.maxValue / 2) - sliderDeadzone) && slider.value <= ((slider.maxValue / 2) + sliderDeadzone))
        {
            pushForce = 1;
        }
        else
        {
            pushForce = slider.value;
        }
        Vector3 force =  transform.forward * acceleration * m_rb.mass;
        m_rb.AddForce(force * pushForce, ForceMode.Impulse);
    }

    public void DisableBall()
    {
        activePlayer = false;
        slider = null;
    }

    private float CalculateFinalVelocity()
    {
        float acc = 0f;
        float forceNormal = coFriction * Physics.gravity.y;

        float finalVelocity = (0 - (2 * forceNormal * (m_totalDistance)));
        acc = Mathf.Sqrt(finalVelocity);
        return acc;
    }

    private float Remap(float value, float minRange1, float maxRange1, float minRange2, float maxRange2)
    {
        return minRange2 + (value - minRange1) * ((maxRange2 - minRange2) / (maxRange1 - minRange1));
    }
}