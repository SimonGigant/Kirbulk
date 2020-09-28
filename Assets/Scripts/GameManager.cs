using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager
        instance = null;

    public Image imagetofade;
    public MaraveController marave;
    public bool isCredits = false;

    void Awake() {
//        Debug.Log("init GameManager");
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        StartCoroutine(DelayBeforeEnd());
    }

    IEnumerator DelayBeforeEnd()
    {
        yield return new WaitForSeconds(30f);
        if (isCredits)
            LoadLevel("JojoPostCredit");
    }

    private void Update() {
        float tValue = Keyboard.current.tKey.isPressed ? 1f : 0f;
        if (tValue == 1) {
//            LoadLevel("StoneTestScene");
            QuitGame();
        }
    }

    public void LoadLevel(string sceneName) {
        StartCoroutine(CinematicLoadLevel(sceneName));
    }

    public void QuitGame() {
        StartCoroutine(CinematicQuitGame());
    }
    
    IEnumerator CinematicLoadLevel(string sceneName) {
        yield return FadeIn();
        SceneManager.LoadSceneAsync(sceneName);
        yield return FadeOut();
    }
    
    IEnumerator CinematicQuitGame() {
        yield return FadeIn();
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private IEnumerator FadeIn() {
        float start = Time.time;
        float duration = 1;
        float elapsed = 0;
        while (elapsed < duration) {
            // calculate how far through we are
            elapsed = Time.time - start;
            float normalisedTime = Mathf.Clamp(elapsed / duration, 0, 1);
            imagetofade.color = Color.Lerp(Color.clear, Color.black, normalisedTime);
            // wait for the next frame
            yield return null;
        }
    }
    
    private IEnumerator FadeOut() {
        float start = Time.time;
        float duration = 1;
        float elapsed = 0;
        while (elapsed < duration) {
            // calculate how far through we are
            elapsed = Time.time - start;
            float normalisedTime = Mathf.Clamp(elapsed / duration, 0, 1);
            imagetofade.color = Color.Lerp(Color.black, Color.clear, normalisedTime);
            // wait for the next frame
            yield return null;
        }
    }
    
}