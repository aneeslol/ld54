using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class MowerController : MonoBehaviour
{
    [SerializeField] public Rigidbody Body;
    [SerializeField] public float SpeedFactor;
    [SerializeField] public float TurnFactor;

    [SerializeField] public float Fuel = 100;
    [SerializeField] public float MaxFuel = 100;
    [SerializeField] public float FuelDrainFactor = 2f;

    [SerializeField] public ParticleSystem BoostParticle;
    [SerializeField] public Collider BoostCollider;

    [SerializeField] public List<GameObject> FuelIndicators;
    [SerializeField] public GameObject FuelIndicatorParent;

    public static event Action OnFuelEmpty;
    public float BoostFactor = 1;

    private float CameraShakeTimer = 0;
    private float CameraShakeFactor = 1;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (!LevelController.GameOver && !LevelController.Complete)
        {
            HandleMovement();
            DrainFuel();
            SetCameraPosition();

            var bodyPos = Body.transform.position;
            FuelIndicatorParent.transform.position = new Vector3(bodyPos.x, 0, bodyPos.z);

            if (BoostFactor > 1)
            {
                BoostParticle.Play();
                BoostCollider.enabled = true;
            }
            else
            {
                BoostParticle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                BoostCollider.enabled = false;
            }
        }
        else
        {
            BoostParticle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void ShakeCamera(float amount, float duration)
    {
        CameraShakeFactor = amount;
        CameraShakeTimer = duration;
    }

    private void SetCameraPosition()
    {
        var localPosition = Body.transform.localPosition;

        CameraShakeTimer -= Time.deltaTime;
        var cameraShakeOffset = Vector3.zero;
        if (BoostFactor > 1)
        {
            CameraShakeFactor = Math.Max(.1f, CameraShakeFactor);
        }
        if (CameraShakeTimer > 0 || BoostFactor > 1)
        {
            cameraShakeOffset = Vector3.one * Random.Range(-CameraShakeFactor, CameraShakeFactor);
        }
        else
        {
            CameraShakeFactor = 0f;
        }
        
        Camera.main.transform.localPosition = cameraShakeOffset + new Vector3(
            localPosition.x + .75f,
            27,
            localPosition.z - 19
        );
    }

    private void DrainFuel()
    {
        var fuelBoostDrainFactor = BoostFactor == 1 ? 1 : 5;
        Fuel -= (FuelDrainFactor * fuelBoostDrainFactor * Time.deltaTime);
        if (Fuel <= 0)
        {
            OnFuelEmpty?.Invoke();
        }
    }

    private void HandleMovement()
    {
        var moving = false;
        var forward = Body.transform.forward;
        BoostFactor = 1;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                BoostFactor = 3f;
            }
            Body.AddForce(forward * SpeedFactor * BoostFactor);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Body.AddForce(forward * -SpeedFactor);
            moving = true;
        }

        if (moving)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Body.AddRelativeTorque(Vector3.up * -TurnFactor * BoostFactor, ForceMode.Acceleration);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Body.AddRelativeTorque(Vector3.up * TurnFactor * BoostFactor, ForceMode.Acceleration);
            }
        }
    }

    public void AddFuel(int amount)
    {
        Fuel += amount;
        if (Fuel > MaxFuel)
        {
            Fuel = MaxFuel;
        }
    }

    public void ShowFuelIndicator(int i)
    {
        HideFuelIndicators();
        FuelIndicators[i].gameObject.SetActive(true);
    }

    public void HideFuelIndicators()
    {
        FuelIndicators.ForEach(f => f.gameObject.SetActive(false));
    }
}
