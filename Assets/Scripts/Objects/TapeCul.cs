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

        SoundManager.PlaySound("Play_SFX_Blast_Zone_Smash", gameObject);

        yield return new WaitForSeconds(0.6f);

        SoundManager.PlaySound("Play_SFX_Smash_Game", gameObject);
    }
}
