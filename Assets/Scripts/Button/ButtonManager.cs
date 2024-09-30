using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // キャラクターセレクト（ゲーム開始前）
    static Button yesGameStartButton;
    Button notGameStartButton;
    public static Button YesGameStartButton { get { return yesGameStartButton; } }

    // ゲームプレイ中（ゲームクリア・ゲームオーバー時）
    Button backToTitleButton;
    Button backToCharaSelectButton;
    Button retryGameButton;

    private void Update()
    {
        // キャラクターセレクトシーンのみで機能
        GetYesAndNoButtonInstance();
        //　プレイシーンでのみ実行　first, second, final ステージ
        GetGameFinishedButtonInstance();
    }

    private void GetYesAndNoButtonInstance()
    {
        //　キャラクター選択シーンでのみ実行
        if (SceneManager.GetActiveScene().name == "CharaSelectScene")
        {
            if (yesGameStartButton == null)
            {
                yesGameStartButton = GameObject.Find("YesButton").GetComponent<Button>();
                notGameStartButton = GameObject.Find("NoButton").GetComponent<Button>();
            }
        }
    }

    private void GetGameFinishedButtonInstance()
    {        
        if (SceneManager.GetActiveScene().name != "TitleScene" &&
            SceneManager.GetActiveScene().name != "CharaSelectScene" &&
            SceneManager.GetActiveScene().name != "Result")
        {
            backToTitleButton = GameObject.Find("TitleButton").GetComponent<Button>();
            backToCharaSelectButton = GameObject.Find("CharaSelectButton").GetComponent<Button>();
            retryGameButton = GameObject.Find("RetryButton").GetComponent<Button>();
        }
    }

    public bool DetectYesGameStartButtonClick()
    {
        bool isClicked = false;

        yesGameStartButton.onClick.AddListener(() => isClicked = true);

        return isClicked;
    }

    public bool DetectNotGameStartButtonClick()
    {
        bool isClicked = false;

        notGameStartButton.onClick.AddListener(() => isClicked = true);

        return isClicked;
    }
    public bool DetectBackToTitleButtonClick()
    {
        bool isClicked = false;

        backToTitleButton.onClick.AddListener(() => isClicked = true);

        return isClicked;
    }

    public bool DetectBackToCharaSelectButtonClick()
    {
        bool isClicked = false;

        backToCharaSelectButton.onClick.AddListener(() => isClicked = true);

        return isClicked;
    }

    public bool DetectRetryGameButtonClick()
    {
        bool isClicked = false;

        retryGameButton.onClick.AddListener(() => isClicked = true);

        return isClicked;
    }
}