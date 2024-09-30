using UnityChan;
using UnityEngine;

public class DestinationPosSceneTransition : MonoBehaviour
{

    public static DestinationPosSceneTransition instance;

    public static Vector3 transitionPoint_stage1 { get; private set; }
    public static Vector3 transitionPoint_stage2 { get; private set; }
    public static Vector3 transitionPoint_stage3 { get; private set; }

    private void Awake()
    {
        if (instance == null)
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
        // �X�e�[�W�P �f�o�b�O�p�X�|�[���ʒu�@Vector3(25,8.36,38)
        // �X�e�[�W�P ��ʒu                  Vector3((24f, 8.36f, 37.75f)
        transitionPoint_stage1 = new Vector3(25, 8.36f, 38);

        // �X�e�[�W�Q �f�o�b�O�p�X�|�[���ʒu�@Vector3(101, 4.5f, 92f)
        // �X�e�[�W�Q ��ʒu                  Vector3(83, 7.7f, -34)
        transitionPoint_stage2 = new Vector3(101, 4.5f, 92f);

        // �X�e�[�W�R �f�o�b�O�p�X�|�[���ʒu�@Vector3(-374, 10f, 11f)
        // �X�e�[�W�R �]�܂�����ʒu�i��q�œo��Ȃ����ߕs�jVector3(Vector3(70.1800003,2.6099999,26.75)
        // �X�e�[�W�R �T�u�X�|�[���ʒu(�f�t�H���g�j�@Vector3(55.0730019, 7.8130002, 9.125)

        transitionPoint_stage3 = new Vector3(-374f, 10f, 11f);
    }

    public void SceneTransition(string nextSceneName)
    {
        if (!BlackBoxManager.HasAllRequiredBox()) return;

        // new Vector3�� y����Ⴍ�ݒ肵������ƁA�J�ڂƓ����ɃQ�[���I�[�o�[�ɂȂ��Ă��܂��̂Œ���

        if (nextSceneName == "SecondStage")
        {
            SceneChange.instance.ChangeScene(nextSceneName);
            UnityChanController.instance.transform.position = transitionPoint_stage2;
            BlackBoxManager.boxNum = 0;

            return;

            //player.transform.position = new Vector3(60, 0, -16);
        }
        else if (nextSceneName == "FinalStage")
        {
            SceneChange.instance.ChangeScene(nextSceneName);
            UnityChanController.instance.transform.position = transitionPoint_stage3;
            BlackBoxManager.boxNum = 0;

            //player.transform.position = new Vector3(55f, 7.7f, 7.7f);
            return;
        }
        else if (nextSceneName == "Result")
        {
            GameState.gameClear = true;

            SceneChange.instance.ChangeScene(nextSceneName);
            BlackBoxManager.boxNum = 0;

            return;
        }
    }
}