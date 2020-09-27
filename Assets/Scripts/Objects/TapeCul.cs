using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeCul : MonoBehaviour
{
    public GameObject version1;
    public GameObject version2;

    public void TapeMoiLeCul()
    {
        StartCoroutine(TapeMoi());
    } 

    private IEnumerator TapeMoi()
    {
        yield return new WaitForSeconds(0.5f);
        version1.SetActive(false);
        version2.SetActive(true);
    }
}
