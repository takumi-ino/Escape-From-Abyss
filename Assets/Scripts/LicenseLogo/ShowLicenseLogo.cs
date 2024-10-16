using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShowLicenseLogo : MonoBehaviour
{
    Image unityLogoImage, acquireLogoImage;

    const float DURATION = 1f;

    void Start()
    {
        unityLogoImage = GameObject.Find("UnityLogo").GetComponent<Image>();
        acquireLogoImage = GameObject.Find("AcquireLogo").GetComponent<Image>();

        unityLogoImage.color = new Color(1, 1, 1, 0);
        acquireLogoImage.color = new Color(1, 1, 1, 0);

        StartCoroutine(ShowLogos());
    }

    IEnumerator ShowLogos()
    {
        // Unityロゴ
        yield return StartCoroutine(FadeInLogo(unityLogoImage));
        yield return StartCoroutine(FadeOutLogo(unityLogoImage));

        // Acquireロゴ
        yield return StartCoroutine(FadeInLogo(acquireLogoImage));
        yield return StartCoroutine(FadeOutLogo(acquireLogoImage));

        // タイトルシーンへ遷移
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator FadeInLogo(Image logo)
    {
        for (float t = 0.0f; t < DURATION; t += Time.deltaTime)
        {
            logo.color = new Color(1, 1, 1, t);

            // 現在のフレームの終了まで処理を一時停止し、次のフレームの開始までコルーチンを待機
            yield return null;
        }

        logo.color = new Color(1, 1, 1, 1);
    }

    IEnumerator FadeOutLogo(Image logo)
    {
        for (float t = 0.0f; t < DURATION; t += Time.deltaTime)
        {
            logo.color = new Color(1, 1, 1, 1 - t);
            yield return null;
        }

        logo.color = new Color(1, 1, 1, 0);
    }
}