using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{

    [SerializeField] private UnityEvent whenInteracted;

    [SerializeField] private bool singleTimeInteraction;
    private bool interacted = false;
    public bool pet = false;

    /*[SerializeField] private Material highlight;
    private Material basicMaterial;*/

    private void Start()
    {
        //basicMaterial = GetComponentInChildren<Renderer>().material;
    }

    private void ChangeMaterial(Material newMaterial)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in renderers)
        {
            r.material = newMaterial;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<MaraveController>(out MaraveController marave))
        {
            if (marave.ActionPress() && (!singleTimeInteraction || !interacted))
                Use(marave);
        }
    }

    private void Use(MaraveController marave)
    {
        interacted = true;
        whenInteracted.Invoke();
        if (pet)
            marave.Pet();
    }
}
