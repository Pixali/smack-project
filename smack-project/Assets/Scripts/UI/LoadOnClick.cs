using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

    bool loading = false;
    AsyncOperation async;

    void OnGUI() {
        if (loading) {
            GUI.Label(new Rect(10, 10, 150, 150), "Loading...");
        }
    }

    public void loadScene(int scene) {
        StartCoroutine(load(scene));
    }

    private IEnumerator load(int scene) {
        async = SceneManager.LoadSceneAsync(scene);
        loading = true;
        while (!async.isDone) {
            yield return null;
        }
    }
}

