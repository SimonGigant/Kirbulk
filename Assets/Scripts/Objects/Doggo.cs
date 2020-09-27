using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doggo : MonoBehaviour
{
    private Animator anim;
    public GameObject explosion;

    bool isDead = false;

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

        SoundManager.PlaySound("Play_Dog_Caresse_Happy", gameObject);

        StartCoroutine(WaitExplosion());
    }

    private IEnumerator WaitExplosion()
    {
        yield return new WaitForSeconds(0.1f);
        Gamefeel.Instance.InitFreezeFrame(0.3f, 0.01f);

        SoundManager.PlaySound("Play_SFX_Explosion", gameObject);
    }

    public void Waf()
    {
        if (!isDead)
        {
            anim.SetTrigger("Waf");

            SoundManager.PlaySound("Play_Dog_Happy", gameObject);
            SoundManager.PlaySound("Play_Dog_Leash", gameObject);
        }
    }
}