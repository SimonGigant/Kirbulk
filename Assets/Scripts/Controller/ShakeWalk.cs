using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeWalk : MonoBehaviour
{
    public void Step()
    {
        Gamefeel.Instance.InitScreenshake(0.15f, 0.5f);
    }
}
