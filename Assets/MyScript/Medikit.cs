using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medikit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovementScript>().health + 20 > 100)
        {
            other.GetComponent<PlayerMovementScript>().health = 100;
        }
        else
        {
            other.GetComponent<PlayerMovementScript>().health += 20;
        }
        Destroy(gameObject);
    }
}
