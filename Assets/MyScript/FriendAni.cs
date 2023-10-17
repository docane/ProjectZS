using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendAni : MonoBehaviour
{
    public const string IDLE = "idle";
    public const string WALK = "run";
    public const string ATTACK = "shoot";
    public const string DIE = "Z_FallingBack";

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeAni(string aniName)
    {
        anim.CrossFade(aniName, 0.2f);
    }
}
