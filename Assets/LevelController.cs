using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    [SerializeField] public int Level;
    [SerializeField] public GameObject GrassContainer;
    [SerializeField] public TMP_Text GameOverText;
    [SerializeField] public TMP_Text CompleteText;
    [SerializeField] public TMP_Text TimerText;
    [SerializeField] public TMP_Text TimesText;
    [SerializeField] public MowerController Mower;

    [SerializeField] public GameObject FuelCanPrefab;
    [SerializeField] public float FuelSpawnTimer = 10;
    [SerializeField] public float FuelSpawnTimerMax = 10;
    private bool FuelSpawned = false;
    public GameObject FuelCan = null;
    public static bool GameOver = false;
    public static bool Complete = false;

    [SerializeField] public MeterController ProgressMeter;
    [SerializeField] public MeterController FuelMeter;

    private List<GrassController> Grass = new List<GrassController>();
    private int TotalGrass = 0;
    private float ElapsedTime;
    
    void Start()
    {
        ElapsedTime = 0;
        foreach (Transform g in GrassContainer.transform)
        {
            Grass.Add(g.GetComponent<GrassController>());
        }

        TotalGrass = Grass.Count;
        FuelController.OnFuelCollected += ResetFuelTimer;
        MowerController.OnFuelEmpty += SetGameOver;

        ProgressMeter.SetColor(new Color(0, .69f, .05f));
        FuelMeter.SetColor(new Color(.69f, .05f, .05f));
    }

    private void OnDestroy()
    {
        FuelController.OnFuelCollected -= ResetFuelTimer;
        MowerController.OnFuelEmpty -= SetGameOver;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TimesText.gameObject.SetActive(!TimesText.gameObject.activeInHierarchy);
        }
        
        if (GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameOver = false;
                Time.timeScale = 1;
                SceneManager.LoadScene(Level);
            }
        }
        else if (Complete)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Complete = false;
                Time.timeScale = 1;
                SceneManager.LoadScene(Level + 1);
            }
        }
        else
        {
            ElapsedTime += Time.deltaTime;
            UpdateDisplays();
            SpawnFuel();
            UpdateFuelIndicator();
        }
    }

    private void SpawnFuel()
    {
        FuelSpawnTimer -= Time.deltaTime;
        if (FuelSpawnTimer <= 0 && !FuelSpawned)
        {
            var index = Random.Range(0, TotalGrass);
            FuelCan = Instantiate(FuelCanPrefab, Grass[index].transform.position, Quaternion.identity);
            FuelSpawnTimer = FuelSpawnTimerMax;
            FuelSpawned = true;
        }
    }

    private void UpdateFuelIndicator()
    {
        if (FuelCan != null)
        {
            var a = Mower.Body.transform.position;
            var b = FuelCan.transform.position;
            if (Vector3.Distance(b, a) < 15)
            {
                Mower.HideFuelIndicators();
                return;
            }
            var angle = Mathf.Rad2Deg * (Mathf.Atan2(b.z - a.z, b.x - a.x));
            Debug.Log(angle);

            if (b.x >= a.x && b.z <= a.z)
            {
                if (angle > -100 && angle < -75) //2
                    Mower.ShowFuelIndicator(4);
                else if (Math.Abs(angle) < 20) //6
                    Mower.ShowFuelIndicator(2);
                else
                    Mower.ShowFuelIndicator(3); //3
            }
            else if (b.x >= a.x && b.z >= a.z)
            {
                if (angle > 75 && angle < 100) //8
                    Mower.ShowFuelIndicator(0);
                else if (Math.Abs(angle) < 20) //6
                    Mower.ShowFuelIndicator(2);
                else //9
                    Mower.ShowFuelIndicator(1);
            }
            else if (b.x <= a.x && b.z >= a.z)
            {
                if (Math.Abs(angle) > 150) //4
                    Mower.ShowFuelIndicator(6);
                else if (angle > 75 && angle < 100) //8
                    Mower.ShowFuelIndicator(0);
                else  //7
                    Mower.ShowFuelIndicator(7);
            }
            else if (b.x <= a.x && b.z <= a.z)
            {
                if (angle > -100 && angle < -75) //2
                    Mower.ShowFuelIndicator(4);
                else if (Math.Abs(angle) > 150) //4
                    Mower.ShowFuelIndicator(6);
                else //1
                    Mower.ShowFuelIndicator(5);
            }
        }
    }

    void ResetFuelTimer()
    {
        Mower.HideFuelIndicators();
        FuelSpawnTimer = FuelSpawnTimerMax;
        FuelSpawned = false;
    }

    void SetGameOver()
    {
        GameOverText.gameObject.SetActive(true);
        Time.timeScale = 0;
        GameOver = true;
    }

    private void UpdateDisplays()
    {
        var count = Grass.Count(g => g.Mowed);
        var percentage = Math.Floor(((float)count / TotalGrass) * 100);
        ProgressMeter.SetMeter((float)percentage);
        ProgressMeter.SetText($"{percentage}/100%");

        if (count == TotalGrass)
        {
            CompleteText.gameObject.SetActive(true);
            Data.LevelTimes[Level] = ElapsedTime;
            Time.timeScale = 0;
            Complete = true;
        }

        var fuelPercentage = Math.Floor(Mower.Fuel);
        FuelMeter.SetMeter((float)fuelPercentage);
        FuelMeter.SetText($"{fuelPercentage}/{Mower.MaxFuel}");

        TimerText.text = GetTimeString(ElapsedTime);

        var timesText = "Times:\n";
        for(var i = 0; i < 10; i++)
            if (Data.LevelTimes[i] != -1)
                timesText += (i + 1) + " - " + GetTimeString(Data.LevelTimes[i]) + "\n";

        TimesText.text = timesText;
    }

    private string GetTimeString(float seconds)
    {
        var time = TimeSpan.FromSeconds(seconds);
        var minutes = Math.Floor((time.TotalMinutes));
        return minutes + ":" + time.ToString(@"ss\.ff");
    }
}
