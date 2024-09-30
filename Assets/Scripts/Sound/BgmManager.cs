using UnityEngine;
using UnityEngine.SceneManagement;


public class BgmManager : MonoBehaviour
{

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] clips;

    string currentScene;

    public enum Select
    {
        TITLE,
        CHARASELECT,
    };

    public static BgmManager instance;

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
        source = GetComponent<AudioSource>();
        currentScene = SceneManager.GetActiveScene().name;
        PlayEachAssignedBGM(currentScene);
    }


    private void Update()
    {
        string newScene = SceneManager.GetActiveScene().name;

        if (currentScene != newScene)
        {
            currentScene = newScene;

            if (newScene == "CharaSelectScene")
                PlayEachAssignedBGM("CharaSelectScene");
            else if (newScene == "TitleScene")
                PlayEachAssignedBGM("TitleScene");
        }
    }

    void PlayEachAssignedBGM(string scene)
    {
        switch (scene)
        {
            case "TitleScene":
                source.clip = clips[(int)Select.TITLE];
                source.Play();
                break;
            case "CharaSelectScene":
                source.clip = clips[(int)Select.CHARASELECT];
                source.Play();
                break;
        }
    }

    public void StopBGM()
    {
        source.Stop();
    }
}