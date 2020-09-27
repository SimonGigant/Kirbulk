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

        if (!SoundManager.IsEventPlayingOnGameObject("Play_Dog_Caresse_Happy", gameObject))
            SoundManager.PlaySound("Play_Dog_Caresse_Happy", gameObject);

        Gamefeel.Instance.InitFreezeFrame(0.3f, 0.01f);

        if (!SoundManager.IsEventPlayingOnGameObject("Play_SFX_Explosion", gameObject))
            SoundManager.PlaySound("Play_SFX_Explosion", gameObject);

        isDead = true;

    }

    public void Waf()
    {
        if (!isDead)
        {
            anim.SetTrigger("Waf");

            if (!SoundManager.IsEventPlayingOnGameObject("Play_Dog_Happy", gameObject))
                SoundManager.PlaySound("Play_Dog_Happy", gameObject);

            if (!SoundManager.IsEventPlayingOnGameObject("Play_Dog_Leash", gameObject))
                SoundManager.PlaySound("Play_Dog_Leash", gameObject);
        }
    }
}
