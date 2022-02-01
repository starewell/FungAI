using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    SceneLoader sceneLoader;
    TileGenerator tileGen;

    private static GameManager instance;
    public static GameManager GetInstance() {
        return instance;
    }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this);
        }
        else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start() {
        sceneLoader = SceneLoader.GetInstance();
        sceneLoader.FadeOutCallback += NewScene;
    }

    public void NewScene() {
        StartCoroutine(WaitForTileGenerator());

    }

    IEnumerator WaitForTileGenerator() { 
        while(TileGenerator.instance == null) { 
            yield return null;
        }
    }
}
