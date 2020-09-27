using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager
        instance = null;

    private Camera m_MainCamera;


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

        m_MainCamera = Camera.main;
    }

    public void LoadLevel(string name) {
        SceneManager.LoadSceneAsync(name);
    }

    public void QuitGame() {
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}