using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static Canvas PauseMenuCanvas { get; set; }

    void Start()
    {
        // ゲームクリアまたはゲームオーバー時に表示するキャンバス
        PauseMenuCanvas = GetComponent<Canvas>();
        PauseMenuCanvas = GameObject.Find("PauseMenuCanvas").GetComponent<Canvas>();
        PauseMenuCanvas.enabled = false;
    }


    // Title, CharacterSelect, Retry の３つのメニュー
    public static IEnumerator ShowPauseMenuCanvas()
    {
        yield return new WaitForSeconds(1.75f);
        PauseMenuCanvas.enabled = true;
    }
}