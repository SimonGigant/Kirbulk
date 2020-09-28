using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeWalk : MonoBehaviour
{
    public GameObject stomp;

    public void Step()
    {
        if (GameManager.instance.isCredits)
            return;
        Gamefeel.Instance.InitScreenshake(0.15f, 0.5f);
        float sign = GetComponentInParent<SpriteRenderer>().flipX ? -1 : 1f;
        Instantiate(stomp, transform.position + new Vector3(1f * sign, 1.5f, 0.01f), transform.rotation);

        SoundManager.PlaySound("Play_Footsteps_Gravel", gameObject);
    }

    public void BigShake()
    {
        Gamefeel.Instance.InitScreenshake(1f, 4f);
        Gamefeel.Instance.InitFreezeFrame(0.1f, 0.01f);
    }
}
