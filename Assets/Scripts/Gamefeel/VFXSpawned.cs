using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSpawned : MonoBehaviour
{
    public float duration = 5f;
    void Start()
    {
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
