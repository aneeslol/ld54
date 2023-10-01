using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    [SerializeField] public AudioSource RockAudio;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.gameObject.tag == "Mower" && !other.collider.isTrigger)
        {
            var controller = other.transform.parent.gameObject.GetComponent<MowerController>();
            var contact = other.contacts[0];
            controller.Body.AddForceAtPosition(controller.transform.position - transform.position * 1.7f, contact.point, ForceMode.Impulse);
            controller.ShakeCamera(.2f, .2f);
            controller.AddFuel(-20);
            RockAudio.Play();
        }
    }
}
