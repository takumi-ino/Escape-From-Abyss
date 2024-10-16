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
        // Unity���S
        yield return StartCoroutine(FadeInLogo(unityLogoImage));
        yield return StartCoroutine(FadeOutLogo(unityLogoImage));

        // Acquire���S
        yield return StartCoroutine(FadeInLogo(acquireLogoImage));
        yield return StartCoroutine(FadeOutLogo(acquireLogoImage));

        // �^�C�g���V�[���֑J��
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator FadeInLogo(Image logo)
    {
        for (float t = 0.0f; t < DURATION; t += Time.deltaTime)
        {
            logo.color = new Color(1, 1, 1, t);

            // ���݂̃t���[���̏I���܂ŏ������ꎞ��~���A���̃t���[���̊J�n�܂ŃR���[�`����ҋ@
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