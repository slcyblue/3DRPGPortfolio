using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public bool Hit;

    public void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")){
            Hit = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            Hit = false;
        }
    }
}
