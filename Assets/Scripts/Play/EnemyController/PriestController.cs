using UnityEngine;

public class PriestController : EnemyBaseController
{
    [SerializeField] AudioClip stampAttackSE;


    public PriestController() : base("ê_ïÉ", 13, 3f, 4.5f, 3.0f) { }

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void PlayStampAttackSE()
    {
        weaponAudio.clip = stampAttackSE;
        weaponAudio.Play();
    }
}
