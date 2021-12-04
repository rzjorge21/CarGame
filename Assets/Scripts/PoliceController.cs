using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceController : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform carro;
    public float velocidad_policia;
    private Rigidbody rb;
    public float distanceToCar;
    public AudioClip dieSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, carro.position);
        if(dist < distanceToCar)
        {
            transform.LookAt(carro);
            transform.position = Vector3.MoveTowards(transform.position, carro.transform.position, velocidad_policia * Time.deltaTime);
        }
    }

    public void PlaySound()
    {
        if (dieSound)
            AudioSource.PlayClipAtPoint(dieSound, transform.position);
    }
}
