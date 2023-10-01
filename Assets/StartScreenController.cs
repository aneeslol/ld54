using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour
{
    [SerializeField] public Image AccelerateImage;
    [SerializeField] public Image ReverseImage;
    [SerializeField] public Image LeftImage;
    [SerializeField] public Image RightImage;
    [SerializeField] public Image BoostImage;
    [SerializeField] public Toggle HardModeToggle;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetButton("A"))
        {
            AccelerateImage.color = Color.red;
        }
        else
        {
            AccelerateImage.color = Color.white;
        }
        
        if (Input.GetButton("B"))
        {
            ReverseImage.color = Color.red;
        }
        else
        {
            ReverseImage.color = Color.white;
        }
        
        if (Input.GetButton("C"))
        {
            BoostImage.color = Color.red;
        }
        else
        {
            BoostImage.color = Color.white;
        }
        
        var horizontal = Input.GetAxis("Horizontal");
        if (horizontal < -.4f)
        {
            LeftImage.color = Color.red;
        }
        else
        {
            LeftImage.color = Color.white;
        }
        
        if (horizontal > .4f)
        {
            RightImage.color = Color.red;
        }
        else
        {
            RightImage.color = Color.white;
        }
    }

    public void StartGame()
    {
        if (HardModeToggle.isOn)
            LevelController.HardMode = true;
        
        SceneManager.LoadScene(1);
    }
}
