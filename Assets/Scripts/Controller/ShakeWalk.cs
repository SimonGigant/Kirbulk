using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeWalk : MonoBehaviour
{
    public GameObject stomp;

    public void Step()
    {
        Gamefeel.Instance.InitScreenshake(0.15f, 0.5f);
        float sign = GetComponentInParent<SpriteRenderer>().flipX ? -1 : 1f;
        Instantiate(stomp, transform.position + new Vector3(1f * sign, 1.5f, 0.01f), transform.rotation);
    }
}
