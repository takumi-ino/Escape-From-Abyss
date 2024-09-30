using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public string[] sentences; // ���͂��i�[����
    [SerializeField] Text uiText;   // uiText�ւ̎Q��

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharDisplay = 0.05f;   // 1�����̕\���ɂ����鎞��

    private int currentSentenceNum = 0; //���ݕ\�����Ă��镶�͔ԍ�
    private int shownSentenceCount = 0;
    private string currentSentence = string.Empty;  // ���݂̕�����
    private float timeUntilDisplay = 0;     // �\���ɂ����鎞��
    private float timeBeganDisplay = 1;         // ������̕\�����J�n��������
    private int lastUpdateCharCount = -1;       // �\�����̕�����

    private bool isTextPush = false;

    void Start()
    {
        SetNextSentence();

        var text = GameObject.Find("Enter").GetComponent<Text>();
        text.enabled = false;
    }

    public void TextUpdate()
    {
        // ���͂̕\������ / ������
        if (IsDisplayComplete())
        {
            // �Ō�̕��͂ł͂Ȃ� & �G���^�[�܂��̓}�E�X��
            if (currentSentenceNum < sentences.Length && Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Return))
            {
                SetNextSentence();
                ItemSoundManager.instance.Play(ItemSoundManager.Select.FlipPage);
            }
            // �Ō�̕��ɓ��B
            else if (currentSentenceNum >= sentences.Length)
            {
                var text = GameObject.Find("Enter").GetComponent<Text>();
                text.enabled = true;

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
                {
                    SceneChange.instance.LoadFirstStageFromMissionDescription();
                }
            }
        }
        else
        {
            //�\������镶�������v�Z
            int displayCharCount = (int)(Mathf.Clamp01((Time.time - timeBeganDisplay) / timeUntilDisplay) * currentSentence.Length);
            //�\������镶�������\�����Ă��镶�����ƈႤ
            if (displayCharCount != lastUpdateCharCount)
            {
                uiText.text = currentSentence.Substring(0, displayCharCount);
                //�\�����Ă��镶�����̍X�V
                lastUpdateCharCount = displayCharCount;
            }

        }

    }

    // ���̕��͂��Z�b�g����
    void SetNextSentence()
    {
        currentSentence = sentences[currentSentenceNum];
        timeUntilDisplay = currentSentence.Length * intervalForCharDisplay;
        timeBeganDisplay = Time.time;
        currentSentenceNum++;
        lastUpdateCharCount = 0;
    }

    bool IsDisplayComplete()
    {
        return Time.time > timeBeganDisplay + timeUntilDisplay; //��2
    }

    private void Update()
    {
        TextUpdate();
    }
}