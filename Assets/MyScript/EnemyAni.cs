using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAni : MonoBehaviour
{
    public const string IDLE = "Z_Idle";
    public const string WALK = "Z_Walk_InPlace";
    public const string ATTACK = "Z_Attack";
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
