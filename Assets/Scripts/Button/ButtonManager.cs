using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // �L�����N�^�[�Z���N�g�i�Q�[���J�n�O�j
    static Button yesGameStartButton;
    Button notGameStartButton;
    public static Button YesGameStartButton { get { return yesGameStartButton; } }

    // �Q�[���v���C���i�Q�[���N���A�E�Q�[���I�[�o�[���j
    Button backToTitleButton;
    Button backToCharaSelectButton;
    Button retryGameButton;

    private void Update()
    {
        // �L�����N�^�[�Z���N�g�V�[���݂̂ŋ@�\
        GetYesAndNoButtonInstance();
        //�@�v���C�V�[���ł̂ݎ��s�@first, second, final �X�e�[�W
        GetGameFinishedButtonInstance();
    }

    private void GetYesAndNoButtonInstance()
    {
        //�@�L�����N�^�[�I���V�[���ł̂ݎ��s
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