using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{

    [SerializeField] private AudioSource source;

    //　プレイヤー死亡時に鳴らす衝撃音
    [SerializeField] private AudioClip deadImpactClip;

    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip getBlackBoxClip;
    [SerializeField] private AudioClip flipPageClip;

    // 敵用
    [SerializeField] private AudioClip noticePlayerClip;


    public enum Select
    {
        ImpactSeWhenPlayerDead,
        HealPlayer,
        GetBlackBox,
        FlipPage,
        NoticePlayer
    };


    public static SoundEffectManager instance;

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
    }


    public void Play(Select se)
    {
        switch (se)
        {
            case Select.ImpactSeWhenPlayerDead:

                source.clip = deadImpactClip;
                source.Play();
                break;

            case Select.HealPlayer:

                source.clip = healClip;
                source.Play();
                break;

            case Select.GetBlackBox:

                source.clip = getBlackBoxClip;
                source.Play();
                break;

            case Select.FlipPage:
                source.clip = flipPageClip;
                source.Play();
                break;

            case Select.NoticePlayer:
                source.clip = noticePlayerClip;
                source.Play();
                break;
        }
    }
}