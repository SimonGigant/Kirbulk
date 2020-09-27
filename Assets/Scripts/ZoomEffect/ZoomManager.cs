using System.Collections;
using UnityEngine;

public class ZoomManager : MonoBehaviour {
    public Transform target;
    public Transform initTransformCam;

    public bool withReverse = true;

    public float ReverseStartTimer = 1f;

    public float timerZoom = 2f;

    public float speedCoeff = 5f;

    public AnimationCurve timerSpeedCurve;

    private Camera m_MainCamera;
    private Vector2 initPosCam;
    private bool isZoom = true;

    private void Awake() {
        m_MainCamera = Camera.main;
        initPosCam = m_MainCamera.transform.position;
        if (target == null) {
            target = transform;
        }

        if (timerSpeedCurve.length == 0) {
            timerSpeedCurve = AnimationCurve.Linear(0, 1, 1, 0);
        }

        if (initTransformCam == null) {
            initTransformCam = m_MainCamera.transform;
        }
    }

    private void Start() {
        initPosCam = m_MainCamera.transform.position;
        //StartZoom();
    }

    public void StartZoom() {
        StartCoroutine(CinematicZoom());
    }
    
    public void ReverseZoom() {
        StartCoroutine(CinematicReverseZoom());
    }

    IEnumerator CinematicZoom() {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / timerZoom) {
            Vector3 newPosition = Vector3.Lerp(m_MainCamera.transform.position,
                target.position - Vector3.forward*target.position.z, (1/timerZoom) * 3f * Time.deltaTime);
            newPosition.z = -100f;
            m_MainCamera.transform.position = newPosition;
            m_MainCamera.orthographicSize -= evaluateCurve(t) * speedCoeff * Time.deltaTime;
            yield return null;
        }

        if (withReverse) {
            yield return new WaitForSeconds(ReverseStartTimer);
            ReverseZoom();
        }
    }

    IEnumerator CinematicReverseZoom() {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / timerZoom) {
            if (t > 0.5) {
                m_MainCamera.transform.position =
                    Vector3.Lerp(m_MainCamera.transform.position, initPosCam, 2f * Time.deltaTime);
            }

            m_MainCamera.orthographicSize += evaluateReverseCurve(t) * speedCoeff * Time.deltaTime;
            yield return null;
        }
    }

    private float evaluateCurve(float i) {
        if (i >= 0) {
            return timerSpeedCurve.Evaluate(timerSpeedCurve.length * i);
        }

        return 0;
    }

    private float evaluateReverseCurve(float i) {
        if (i >= 0) {
            return timerSpeedCurve.Evaluate(timerSpeedCurve.length * (timerSpeedCurve.length - i));
        }

        return 0;
    }
}