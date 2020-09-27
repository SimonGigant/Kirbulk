using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watercan : MonoBehaviour
{
    public MaraveController marave;

    public void Pick()
    {
        marave.UnlockWatercan();
        Destroy(gameObject);
    }
}
