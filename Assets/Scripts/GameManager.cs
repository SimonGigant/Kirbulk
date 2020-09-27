using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager
        instance = null;

    public Image imagetofade;

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
    }

    public void LoadLevel(string sceneName) {
        StartCoroutine(CinematicLoadLevel(sceneName));
    }

    public void QuitGame() {
        StartCoroutine(CinematicQuitGame());
    }
    
    IEnumerator CinematicLoadLevel(string sceneName) {
        float lerp = 0;
        while (lerp < 1) {
            imagetofade.color *= 1-lerp;
            lerp += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadSceneAsync(sceneName);
        lerp = 0;
        while (lerp < 1) {
            imagetofade.color *= 1-lerp;
            lerp += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    IEnumerator CinematicQuitGame() {
        float lerp = 0;
        while (lerp < 1) {
            imagetofade.color *= 1-lerp;
            lerp += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
}