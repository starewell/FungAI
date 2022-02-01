
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public GameObject blackImage;
    public float fadeDuration;

    public delegate void OnSceneChange();
    public event OnSceneChange FadeOutCallback;

    private static SceneLoader instance;
    public static SceneLoader GetInstance() {
        return instance;
    }

    void Awake() { 
        if (instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void OnEnabled() {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void Start() {
        StartCoroutine(FadeInScene());

    }

    public void SceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("SceneLoaded");
        StartCoroutine(FadeInScene());
    }

    public void ChangeScene(string name) {

        StartCoroutine(FadeToScene(name));
        Debug.Log("Fading");
    }

    public IEnumerator FadeToScene(string name) {
        Color alpha = new Color(0, 0, 0, 0);
        blackImage.SetActive(true);
        blackImage.GetComponent<Image>().color = alpha;
        while (alpha.a < 1) {

            alpha = new Color(alpha.r, alpha.g, alpha.b, alpha.a += 0.1f);
            blackImage.GetComponent<Image>().color = alpha;
            yield return new WaitForSeconds(fadeDuration/60);
        }
        SceneManager.LoadScene(name);
        yield return new WaitForSeconds(1);
        FadeOutCallback?.Invoke();
    }

    public void TEst() { 
    

    }
    public IEnumerator FadeInScene() {
        Color alpha = new Color(0, 0, 0, 1);
        blackImage.SetActive(true);
        blackImage.GetComponent<Image>().color = alpha;
        while (alpha.a > 0) {
            alpha = new Color(alpha.r, alpha.g, alpha.b, alpha.a -= 0.1f);
            blackImage.GetComponent<Image>().color = alpha;
            yield return new WaitForSeconds(fadeDuration/60);
        }
        blackImage.SetActive(false);
    }
}
