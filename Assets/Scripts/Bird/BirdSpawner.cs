using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> birdPrefabs;

    [SerializeField] float timeBetweenSpawns = 3;

    void Start()
    {
        StartCoroutine(SpawnABird());
    }

    IEnumerator SpawnABird()
    {
        while (true)
        {
            GameObject birdToInstantiate = birdPrefabs[Random.Range(0, birdPrefabs.Count)];

            float isLeftOrRight = Random.Range(-1f, 1f);

            Vector3 positionToInstantiate =
                new Vector3((isLeftOrRight < 0f ? -12f : 12f), Random.Range(-4f, 4f), 0f);

            Instantiate(birdToInstantiate, positionToInstantiate, Quaternion.identity);

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
