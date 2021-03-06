﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnfantProjectile : MonoBehaviour
{
    public void Project()
    {
        StartCoroutine(Projection());
    }

    private IEnumerator Projection()
    {
        yield return new WaitForSeconds(0.5f);

        SoundManager.PlaySound("Play_Footsteps_Grass_Soil_Hard", gameObject);
        
        for (; ; )
        {
            transform.position += Vector3.up * Time.deltaTime * 70f;
            yield return null;
        }
    }
}
