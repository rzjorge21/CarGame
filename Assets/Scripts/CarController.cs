using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CarController : MonoBehaviour
{
    public int pointsToWin;
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public Text timerText;
    private float startTime;
    private bool finishedTime = false;
    public Transform restartPoint;

    public AudioClip crashSound;

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;
    public int max_vida = 100;
    public int vida_actual;
    public int max_moneda = 5;
    public int moneda_actual = 0;
    public Text money;
 
    public ControllerVida controllervida; 

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;


    private void Start()
    {
        startTime = Time.time;
        vida_actual = max_vida;
    }

    void Update()
    {
        if(transform.position.y <= -25f)
        {
            transform.position = new Vector3(restartPoint.position.x, restartPoint.position.y, restartPoint.position.z);
        }
        if (finishedTime)
        {
            return;
        }
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timerText.text = minutes + ":" + seconds;
        if (Input.GetKeyDown(KeyCode.V))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
            transform.rotation = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
            //transform.localRotation = new Vector3(0f, transform.localRotation.y, transform.localRotation.z);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    void PlaySound()
    {
        if (crashSound)
            AudioSource.PlayClipAtPoint(crashSound, transform.position);
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();       
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot
;       wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.CompareTag("police"))
        {
            collision.gameObject.GetComponent<PoliceController>().PlaySound();
            Destroy(collision.gameObject);
            ReducirVida(20);
           
        }

        

        if (collision.gameObject.transform.CompareTag("vida"))
        {
            Debug.Log("mas vida");
            collision.gameObject.GetComponent<SimpleCollectibleScript>().Collect();
            AumentarVida(20);



        }

        if (collision.gameObject.transform.CompareTag("RedBall"))
        {
            Destroy(collision.gameObject);
            ReducirVida(10);
        }

        if(collision.gameObject.transform.CompareTag("Obstacule") || collision.gameObject.transform.CompareTag("Wall") || collision.gameObject.transform.CompareTag("RedBall"))
        {
            PlaySound();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("money"))
        {
            SumarMonedas(1);
            other.gameObject.GetComponent<SimpleCollectibleScript>().Collect();
        }
    }

    public void ReducirVida(int reducir)
    {
        vida_actual -= reducir;
        controllervida.SetVida(vida_actual);
        if (vida_actual < 1)
        {
            levelManager.LM.GameOver();
        }
    }

    public void AumentarVida(int aumentar)
    {
        
        if (vida_actual < 100)
        {
            vida_actual += aumentar;
            controllervida.SetVida(vida_actual);

        }
        else{
            Debug.Log("tope");
        }
    }

    public void SumarMonedas(int sumar)
    {
        moneda_actual = moneda_actual + sumar;
        money.text = moneda_actual.ToString();
        if (moneda_actual == max_moneda)
        {
            //Debug.Log("listo");
            
        }
        if (moneda_actual == pointsToWin)
        {
            finishedTime = true;
            float t = Time.time - startTime;
            float recordTime = PlayerPrefs.GetFloat("RECORD_TIME");
            if(recordTime == 0)
            {
                PlayerPrefs.SetFloat("RECORD_TIME", 99999f);
            }
            if(PlayerPrefs.GetFloat("RECORD_TIME") != null && t < PlayerPrefs.GetFloat("RECORD_TIME"))
            {
                recordTime = t;
                PlayerPrefs.SetFloat("RECORD_TIME", recordTime);
            }
            Debug.Log("GANASTE");
            levelManager.LM.WinGame(t.ToString("f2"), recordTime.ToString("f2"));
        }
    }

}
