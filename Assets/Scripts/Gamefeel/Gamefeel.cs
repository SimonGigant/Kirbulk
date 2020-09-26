using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamefeel : MonoBehaviour
{
	
	
	public static Gamefeel _instance;
	public static Gamefeel Instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<Gamefeel>();
				if (_instance == null)
				{
					GameObject container = new GameObject("Gamefeel");
					_instance = container.AddComponent<Gamefeel>();
				}
			}
			return _instance;
		}
	}

	private Camera cam;
	private GameObject target;
	private float startDistance;
	private float endDistance;
	private float vertigoDuration;
	private float vertigoCount;
	private float startingFOV;
	private AnimationCurve vertigoCurve;
	private Vector3 positionChangeVertigo;
	private bool inVertigo = false;
	
	private float shakeMagnitude;
	private float shakeDuration;
	private float shakeCount;
	private Vector3 positionChangeShake;
	private AnimationCurve shakeCurve;
	private bool inShake = false;

    public AnimationCurve thisShakeCurve;
    public AnimationCurve thisFreezeCurve;

    private float freezeMagnitude;
    private float freezeDuration;
    private float freezeCount;
    private AnimationCurve freezeCurve;
    private bool inFreeze = false;
	
	public void InitVertigo(Camera thisCam, GameObject thisTarget, float duration, float force, AnimationCurve curve){
		if(!inVertigo){
			cam = thisCam;
			target = thisTarget;
			vertigoDuration = duration;
			vertigoCurve = curve;
			startDistance = Vector3.Distance(cam.transform.position, target.transform.position);
			endDistance = startDistance - force;
			startingFOV = cam.fieldOfView;
			vertigoCount = 0f;
			StartCoroutine("Vertigo");
		}
	}
	
	
    private IEnumerator Vertigo(){
		inVertigo = true;
		for(;;){
			vertigoCount+=Time.deltaTime;
			Vector3 direction = (target.transform.position - cam.transform.position).normalized;
			float distance = startDistance + vertigoCurve.Evaluate(vertigoCount/vertigoDuration)*(endDistance-startDistance);
			cam.transform.position = target.transform.position - distance*direction;
			positionChangeVertigo += cam.transform.position;
			//Compute FOV angle and convert from radians to degrees
			float angle = (180 / Mathf.PI) * Mathf.Atan(1 / distance);
			float startAngle = (180/Mathf.PI) * Mathf.Atan(1 / startDistance);
			cam.fieldOfView = angle * startingFOV / startAngle;
			if(vertigoCount < vertigoDuration){
				yield return null;
			}else{
				cam.fieldOfView = startingFOV;
				cam.transform.position = target.transform.position - cam.transform.forward * startDistance;
				break;
			}
		}
		inVertigo = false;
	}

    public void InitScreenshake(float duration, float force)
    {
        InitScreenshake(Camera.main, duration, force, thisShakeCurve);
    }
	
	public void InitScreenshake(Camera thisCam, float duration, float force, AnimationCurve curve){
		if(!inShake){
			cam = thisCam;
			shakeDuration = duration;
			shakeMagnitude = force;
			shakeCount = 0f;
			shakeCurve = curve;
			positionChangeShake = new Vector3(0,0,0);
			StartCoroutine("Screenshake");
		}
	}
	
    //Modifié pour adoucir le retour du shake
	private IEnumerator Screenshake(){
		inShake = true;
		for(;;){
            while (inFreeze)
            {
                yield return null;
            }
			shakeCount+=Time.deltaTime;
			Vector3 change = Random.insideUnitSphere * shakeCurve.Evaluate(shakeCount/shakeDuration) * shakeMagnitude;
			cam.transform.localPosition += change - positionChangeShake;
			positionChangeShake = change;
			if(shakeCount < shakeDuration){
				yield return null;
			}else{
                cam.transform.localPosition -= positionChangeShake;
				break;
			}
		}
		inShake = false;
	}

    public bool IsInFreeze()
    {
        return inFreeze;
    }

    public void InitFreezeFrame(float duration, float force)
    {
        InitFreezeFrame(duration, force, thisFreezeCurve);
    }

    public void InitFreezeFrame(float duration, float force, AnimationCurve curve)
    {
        if (!inFreeze)
        {
            freezeDuration = duration;
            freezeMagnitude = force;
            freezeCurve = curve;
            StartCoroutine("FreezeFrame");
        }
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    private IEnumerator FreezeFrame()
    {
        inFreeze = true;
        float original = Time.timeScale;
        freezeCount = 0f;
        for(;;)
        {
            freezeCount += Time.unscaledDeltaTime;
            float freezeValue = freezeCurve.Evaluate(freezeCount / freezeDuration) * freezeMagnitude;
            Time.timeScale = 0f;
            if (freezeCount < freezeDuration) {
                yield return new WaitForSecondsRealtime(freezeValue);
                Time.timeScale = original;
            }
            else
            {
                Time.timeScale = original;
                break;
            }
        }
        inFreeze = false;
    }
}
