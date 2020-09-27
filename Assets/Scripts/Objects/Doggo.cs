using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doggo : MonoBehaviour
{
    private Animator anim;
    public GameObject explosion;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Pet()
    {
        anim.SetTrigger("Pet");
    }

    public void Explosion()
    {
        GameObject.Instantiate(explosion, transform.position + Vector3.up * 4.1f, Quaternion.identity);
    }

    public void Waf()
    {
        anim.SetTrigger("Waf");
    }
}
