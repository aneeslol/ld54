using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour
{
    [SerializeField] ParticleSystem MowParticle;
    [SerializeField] GameObject Flower;

    public bool Mowed;
    public static event Action OnMow;
    
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
            if (!Mowed)
            {
                var controller = other.transform.parent.gameObject.GetComponent<MowerController>();
                controller.AddFuel(-10);
                controller.ShakeCamera(.1f, .2f);
                Mow();
            }
        }
    }

    public void Mow()
    {
        Mowed = true;
        Flower.gameObject.SetActive(false);
        MowParticle.Play();
        OnMow?.Invoke();
    }
}
