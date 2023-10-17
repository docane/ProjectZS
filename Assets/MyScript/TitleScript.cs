using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public GameObject HelpImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartOnClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitOnClick()
    {
        Application.Quit();
    }

    public void HelpOnClick()
    {
        if(HelpImage.activeSelf == true)
        {
            HelpImage.SetActive(false);
        }
        else
        {
            HelpImage.SetActive(true);
        }
    }
}
