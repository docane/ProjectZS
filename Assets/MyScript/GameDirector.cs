using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject temp;
    PlayerMovementScript player;
    GameObject gun;
    public GameObject result;
    public GameObject replay;
    public GameObject gototitle;
    GameObject friend;
    // Use this for initialization
    void Start()
    {
        temp = GameObject.Find("Player");
        player = GameObject.Find("Player").GetComponent<PlayerMovementScript>();
        friend = GameObject.Find("Friend");
    }

    // Update is called once per frame
    void Update()
    {
        gun = GameObject.FindWithTag("Weapon");
        if (player.dead)
        {
            result.SetActive(true);
            result.GetComponentInChildren<Text>().text = "생존하는 동안 " + player.zombieKill + "마리의 좀비를 죽였습니다.";
            replay.SetActive(true);
            gototitle.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            gun.SetActive(false);
            player.enabled = false;
            friend.GetComponent<Friend>().enabled = false;
            temp.GetComponent<MouseLookScript>().enabled = false;
        }
    }

    public void ReplayOnClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToTitleOnClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
}