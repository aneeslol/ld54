using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrassController : MonoBehaviour
{
    [SerializeField] ParticleSystem MowParticle;
    [SerializeField] GameObject Grass;

    public bool Mowed;
    
    void Start()
    {
        var rotation = Random.Range(0, 360);
        Grass.transform.localRotation = Quaternion.Euler(new Vector3(-90, rotation, 0));
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "Mower")
        {
            Mow();
        }
    }

    public void Mow()
    {
        if (!Mowed)
        {
            Mowed = true;
            Grass.gameObject.SetActive(false);
            MowParticle.Play();
        }
    }
}
