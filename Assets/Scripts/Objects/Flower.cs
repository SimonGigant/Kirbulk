using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public void Smash()
    {
        GetComponent<Animator>().SetTrigger("Explode");
        Gamefeel.Instance.InitScreenshake(1f, 0.7f);
    }
}
