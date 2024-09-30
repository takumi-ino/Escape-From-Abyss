using System.Collections;
using UnityChan;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TextTimingManager : MonoBehaviour
{

    [SerializeField] private GameObject searchBlackBoxKeyText;    // 最初に表示するテキスト
    [SerializeField] private GameObject aimForThePlaceText;       // 目的地へ移動しろ！のテキスト

    public GameObject gameClearText; // クリア用、死亡用テキスト
    public GameObject gameOverText;
    [SerializeField] private Text boxNumText_denominator;         // 所持ボックスの母数 
    [SerializeField] private Text boxNumText_numerator;           // 所持ボックスの分子

    public static TextTimingManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 最初に表示するテキスト
        searchBlackBoxKeyText.SetActive(true);
    }

    private void FixedUpdate()
    {
        // ゲーム開始時1度だけ表示される
        StartCoroutine(ShowSearchBlackBoxKeyText());

        if (SceneManager.GetActiveScene().name != "TitleScene" &&
            SceneManager.GetActiveScene().name != "CharaSelectScene" &&
            SceneManager.GetActiveScene().name != "Result")
        {
            // 常に表示
            ShowBoxNumText();
        }

        if (GameState.gameClear)
        {
            gameClearText.SetActive(true);
        }
        else if (GameState.gameOver)
        {
            gameOverText.SetActive(true);
        }
    }

    private void ShowBoxNumText()
    {
        // ボックス所持数テキスト
        boxNumText_numerator.text = BlackBoxManager.BoxNum.ToString();

        string currentScene = SceneManager.GetActiveScene().name;

        // ボックス所持数テキスト
        boxNumText_denominator.text = BlackBoxManager.BoxNumReference[currentScene].ToString();
    }


    IEnumerator ShowSearchBlackBoxKeyText()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(searchBlackBoxKeyText);
    }


    // 箱を必要数回収したらワープポイントに行け、というテキストを表示
    public IEnumerator ShowAimForThePlaceText()
    {
        aimForThePlaceText.SetActive(true);

        yield return new WaitForSeconds(2);

        aimForThePlaceText.SetActive(false);
    }
}