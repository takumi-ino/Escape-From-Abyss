using UnityEngine;
using System.Collections;

public class CharacterVoiceManager : MonoBehaviour
{
    //　ダメージ
    [SerializeField] private AudioSource damageAudioSource;
    [SerializeField] private AudioClip[] damageClips;

    //　息
    [SerializeField] private AudioSource breathAudioSource;
    [SerializeField] private AudioClip breathClip;

    //　ゲームクリア・ゲームオーバー
    [SerializeField] private AudioSource gameStateAudioSource;  // AudioSource
    [SerializeField] private AudioClip[] gameClearClips;        // ゲームクリアClip
    [SerializeField] private AudioClip[] gameOverClips;         // ゲームオーバーClip

    [SerializeField] private AudioClip deadClip;                // 死亡Clip

    [SerializeField] private AudioSource readyToStartSource;    // AudioSource
    [SerializeField] private AudioClip[] readyToStartClips;     // ゲームクリアClip


    public enum Select
    {
        ReadyToStart,
        Damage,
        Breath,
        GameClear,
        GameOver,
        Dead,
        ToTitle,            // 死亡後戻る時
        ToCharacterSelect,  // 死亡後戻る時
        Retry
    };


    public static CharacterVoiceManager instance;

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
        damageAudioSource = GetComponent<AudioSource>();
        breathAudioSource = GetComponent<AudioSource>();
    }

    public void Play(Select se)
    {
        switch (se)
        {
            case Select.ReadyToStart:

                int readyIdx = Random.Range(0, readyToStartClips.Length - 1);

                readyToStartSource.clip = readyToStartClips[readyIdx];
                readyToStartSource.Play();

                break;
            case Select.Damage:

                int damageIdx = Random.Range(0, damageClips.Length - 1);

                damageAudioSource.clip = damageClips[damageIdx];
                damageAudioSource.Play();
                break;
            case Select.Breath:

                breathAudioSource.clip = breathClip;
                breathAudioSource.Play();
                break;
            case Select.GameClear:

                AudioClip clip = gameClearClips[Random.Range(0, gameClearClips.Length)];
                gameStateAudioSource.clip = clip;
                gameStateAudioSource.Play();
                break;
            case Select.GameOver:

                StartCoroutine(PlayDelayGameOverSE());
                break;

            case Select.Dead:

                damageAudioSource.clip = deadClip;
                damageAudioSource.Play();
                break;
            case Select.ToTitle:

                break;

            case Select.ToCharacterSelect:

                break;
            case Select.Retry:

                break;
        }
    }

    // SE -----------------------------------------------------------------------------------------
    IEnumerator PlayDelayGameOverSE()
    {
        yield return new WaitForSeconds(1.4f);

        AudioClip clip = gameOverClips[Random.Range(0, gameOverClips.Length)];
        gameStateAudioSource.clip = clip;
        gameStateAudioSource.Play();
    }

    public void Stop(Select se)
    {
        switch (se)
        {
            case Select.Damage:

                damageAudioSource.Stop();
                break;
            case Select.Breath:

                breathAudioSource.Stop();
                break;
        }
    }
}