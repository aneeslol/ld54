using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    public static event Action OnFuelCollected;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var collider = other.GetComponent<Collider>();
        if (other.transform.gameObject.tag == "Mower" && !collider.isTrigger)
        {
            var controller = other.transform.parent.gameObject.GetComponent<MowerController>();
            controller.AddFuel(50);
            OnFuelCollected?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
