using System.Collections;
using System.Threading.Tasks;
using UnityChan;
using UnityEngine;
using UnityEngine.SceneManagement;

// TitleSceneからゲームを実行しないとシングルトンの関係によりエラーが発生するので注意


public class SceneChange : MonoBehaviour
{
    CharacterVoiceManager voiceManager;
    ButtonManager buttonManager;

    public GameObject fadeCanvas;

    static string currentScene;

    public enum Select
    {
        TITLE,
        CHARASELECT,
        FirstStage,
        SecondStage,
        FinalStage,
        Retry
    };

    public static SceneChange instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!SceneFadeManader.isFadeInstance)
        {
            Instantiate(fadeCanvas);
        }

        Invoke("findFadeObject", 0.02f);

        currentScene = SceneManager.GetActiveScene().name;
    }


    private void Update()
    {
        UpdateCurrentSceneName();

        LoadCharaSelectSceneFromTitle();

        GetManagerInstance();       
    }


    private void UpdateCurrentSceneName()
    {
        // シーンが変わったら currentSceneを更新
        if (currentScene != SceneManager.GetActiveScene().name)
        {
            string newScene = SceneManager.GetActiveScene().name;
            currentScene = newScene;
        }
    }

    private void GetManagerInstance()
    {
        if (SceneManager.GetActiveScene().name == "FirstStage")
        {
            if (voiceManager == null && buttonManager == null)
            {
                voiceManager = GameObject.Find("CharacterVoiceManager").GetComponent<CharacterVoiceManager>();
                buttonManager = GameObject.Find("ButtonManager").GetComponent<ButtonManager>();
            }
        }
    }

    void FindFadeObject()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("Fade");
        fadeCanvas.GetComponent<SceneFadeManader>().fadeIn();
    }


    public async void ChangeScene(string sceneName)
    {
        // リザルトシーンでなければ
        if (sceneName != "Result")
        {
            UnityChanController.instance.HpSliderUpdate();
        }

        fadeCanvas.GetComponent<SceneFadeManader>().fadeOut();
        await Task.Delay(800);
        SceneManager.LoadScene(sceneName);
    }


    public async void LoadCharaSelectSceneFromTitle()
    {
        //　タイトルからキャラ選択シーンへ
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                fadeCanvas.GetComponent<SceneFadeManader>().fadeOut();//フェードアウトフラグを立てる
                await Task.Delay(200);
                SceneManager.LoadScene("CharaSelectScene");
            }
        }
    }


    public void LoadFirstStageFromMissionDescription()
    {
        BgmManager.instance.StopBGM();

        fadeCanvas.GetComponent<SceneFadeManader>().fadeOut();//フェードアウトフラグを立てる

        SceneManager.LoadScene("FirstStage");
    }


    public void LoadTitleSceneAfterGame()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadCharSelectSceneAfterGame()
    {
        SceneManager.LoadScene("CharaSelectScene");
    }

    public void LoadCurrentSceneAfterGame()
    {
        UnityChanController.instance.ResetCurrentStatus();
    }

    IEnumerator DelayLoadTitleSceneAfterGame()
    {
        bool isClicked = buttonManager.DetectBackToTitleButtonClick();

        yield return new WaitUntil(() => isClicked);

        yield return new WaitForSeconds(0.1f);
        voiceManager.Play(CharacterVoiceManager.Select.ToTitle);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator DelayLoadCharSelectScene()
    {
        bool isClicked = buttonManager.DetectBackToCharaSelectButtonClick();

        yield return new WaitUntil(() => isClicked);

        yield return new WaitForSeconds(0.1f);
        voiceManager.Play(CharacterVoiceManager.Select.ToCharacterSelect);

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("CharaSelectScene");
    }

    public IEnumerator DelayLoadMissionDescriptionScene()
    {

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("MissionDescription");
    }

    IEnumerator DelayLoadRetryCurrentScene()
    {
        bool isClicked = buttonManager.DetectRetryGameButtonClick();

        yield return new WaitUntil(() => isClicked);

        yield return new WaitForSeconds(0.1f);
        voiceManager.Play(CharacterVoiceManager.Select.Retry);

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator DelayLoadResult()
    {

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Result");
    }
}