using System.Collections;
using UnityChan;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TextTimingManager : MonoBehaviour
{

    [SerializeField] private GameObject searchBlackBoxKeyText;    // �ŏ��ɕ\������e�L�X�g
    [SerializeField] private GameObject aimForThePlaceText;       // �ړI�n�ֈړ�����I�̃e�L�X�g

    public GameObject gameClearText; // �N���A�p�A���S�p�e�L�X�g
    public GameObject gameOverText;
    [SerializeField] private Text boxNumText_denominator;         // �����{�b�N�X�̕ꐔ 
    [SerializeField] private Text boxNumText_numerator;           // �����{�b�N�X�̕��q

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
        // �ŏ��ɕ\������e�L�X�g
        searchBlackBoxKeyText.SetActive(true);
    }

    private void FixedUpdate()
    {
        // �Q�[���J�n��1�x�����\�������
        StartCoroutine(ShowSearchBlackBoxKeyText());

        if (SceneManager.GetActiveScene().name != "TitleScene" &&
            SceneManager.GetActiveScene().name != "CharaSelectScene" &&
            SceneManager.GetActiveScene().name != "Result")
        {
            // ��ɕ\��
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
        // �{�b�N�X�������e�L�X�g
        boxNumText_numerator.text = BlackBoxManager.BoxNum.ToString();

        string currentScene = SceneManager.GetActiveScene().name;

        // �{�b�N�X�������e�L�X�g
        boxNumText_denominator.text = BlackBoxManager.BoxNumReference[currentScene].ToString();
    }


    IEnumerator ShowSearchBlackBoxKeyText()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(searchBlackBoxKeyText);
    }


    // ����K�v����������烏�[�v�|�C���g�ɍs���A�Ƃ����e�L�X�g��\��
    public IEnumerator ShowAimForThePlaceText()
    {
        aimForThePlaceText.SetActive(true);

        yield return new WaitForSeconds(2);

        aimForThePlaceText.SetActive(false);
    }
}