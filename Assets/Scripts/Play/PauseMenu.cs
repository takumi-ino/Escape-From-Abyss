using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static Canvas PauseMenuCanvas { get; set; }

    void Start()
    {
        // �Q�[���N���A�܂��̓Q�[���I�[�o�[���ɕ\������L�����o�X
        PauseMenuCanvas = GetComponent<Canvas>();
        PauseMenuCanvas = GameObject.Find("PauseMenuCanvas").GetComponent<Canvas>();
        PauseMenuCanvas.enabled = false;
    }


    // Title, CharacterSelect, Retry �̂R�̃��j���[
    public static IEnumerator ShowPauseMenuCanvas()
    {
        yield return new WaitForSeconds(1.75f);
        PauseMenuCanvas.enabled = true;
    }
}