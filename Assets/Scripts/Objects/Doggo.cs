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
        GameObject.Instantiate(explosion, transform.position + Vector3.up * 3.9f, Quaternion.identity);
        Gamefeel.Instance.InitScreenshake(2f, 2f);
        StartCoroutine(WaitExplosion());
    }

    private IEnumerator WaitExplosion()
    {
        yield return new WaitForSeconds(0.1f);
        Gamefeel.Instance.InitFreezeFrame(0.3f, 0.01f);
    }

    public void Waf()
    {
        anim.SetTrigger("Waf");
    }
}
