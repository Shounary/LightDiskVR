using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public Image blackImg;
    public AnimationCurve f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    // Fade to menu
    public void FadeTo(string sceneName) {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn() {
        float t = 1f;
        while (t > 0f) {
            t -= Time.deltaTime;
            blackImg.color = new Color(0f, 0f, 0f, f.Evaluate(t));
            yield return 0;
        }
    }

    IEnumerator FadeOut(string sceneName) {
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime;
            blackImg.color = new Color(0f, 0f, 0f, f.Evaluate(t));
            yield return 0;
        }
        SceneManager.LoadScene(sceneName);
    }
}
