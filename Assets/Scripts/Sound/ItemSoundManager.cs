using UnityEngine;

public class ItemSoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource source;

    //　プレイヤー死亡時に鳴らす衝撃音
    [SerializeField] private AudioClip deadImpactClip;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip getBlackBoxClip;
    [SerializeField] private AudioClip flipPageClip;


    public enum Select
    {
        ImpactSeWhenPlayerDead,
        HealPlayer,
        GetBlackBox,
        FlipPage
    };


    public static ItemSoundManager instance;

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
        }
    }
}