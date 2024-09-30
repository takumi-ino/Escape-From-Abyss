using UnityEngine;

public class ShowFinalCheckCanvas : MonoBehaviour
{
    // inspector
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clip;
    [SerializeField] private Canvas checkStartGameCanvas;
    [SerializeField] Canvas chooseCharaTextCanvas;

    // singleton
    public static ShowFinalCheckCanvas instance;

    CharacterVoiceManager characterVoiceManager;

    private void Start()
    {
        chooseCharaTextCanvas = GameObject.Find("ChooseCharaTextCanvas").GetComponent<Canvas>();

        characterVoiceManager = GameObject.Find("CharacterVoiceManager").GetComponent<CharacterVoiceManager>();

        checkStartGameCanvas.enabled = false;
    }

    // YesButton�ɓK�p
    public void CallMissionDescriptionScene()
    {
        characterVoiceManager.Play(CharacterVoiceManager.Select.ReadyToStart);
        BgmManager.instance.StopBGM();
        StartCoroutine(SceneChange.instance.DelayLoadMissionDescriptionScene());
    }


    // Choose Character! �̃e�L�X�g��\��
    public void ShowChooseCharacterText()
    {
        chooseCharaTextCanvas.enabled = true;
    }

    // Choose Character! �̃e�L�X�g���\��
    public void HideChooseCharaText()
    {
        chooseCharaTextCanvas.enabled = false;
    }


    // Start Game? �e�L�X�g��\�����A�Q�[���J�n�̒��O��Ԃ܂Ői��
    public void ShowFinalCheckMenuCanvas()
    {
        HideChooseCharaText();

        PlaySelectCanvasSE();
        checkStartGameCanvas.enabled = true;
    }

    public void HideFinalCheckMenuCanvas()
    {
        PlayNoButtonSE();
        ShowChooseCharacterText();
        checkStartGameCanvas.enabled = false;
    }

    public void PlaySelectCanvasSE()
    {
        audioSource.clip = clip[0];
        audioSource.Play();
    }

    public void PlayNoButtonSE()
    {
        audioSource.clip = clip[1];
        audioSource.Play();
    }
}