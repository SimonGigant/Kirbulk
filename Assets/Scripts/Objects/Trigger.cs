using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private bool singleTimeTrigger;
    private bool wasTriggeredIn;
    private bool wasTriggeredOut;

    [SerializeField] private UnityEvent triggerIn;
    [SerializeField] private UnityEvent triggerOut;

    [HideInInspector] public bool lepidIsInTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<MaraveController>(out MaraveController marave)){
            if(!(singleTimeTrigger && wasTriggeredIn))
            {
                triggerIn.Invoke();
                wasTriggeredIn = true;
            }
            lepidIsInTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<MaraveController>(out MaraveController marave))
        {
            if (!(singleTimeTrigger && wasTriggeredIn))
            {
                triggerIn.Invoke();
                wasTriggeredIn = true;
            }
            lepidIsInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<MaraveController>(out MaraveController marave)){
            if (!(singleTimeTrigger && wasTriggeredOut))
            {
                triggerOut.Invoke();
                wasTriggeredOut = true;
            }
            lepidIsInTrigger = false;
        }
    }
}
