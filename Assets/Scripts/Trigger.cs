using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Obstacule"))
        {
            Debug.Log("Game Log");
            //levelManager.LM.GameOver();
           
        }
    }
}
