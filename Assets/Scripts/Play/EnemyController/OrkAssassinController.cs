using UnityEngine;

public class OrkAssassinController : EnemyBaseController
{

    [SerializeField] AudioClip stabAttackSE;

    public OrkAssassinController() : base("オークアサシン", 8, 5f, 7f, 8f, 5f) { }

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void PlayStabAttackSE()
    {
        weaponAudio.clip = stabAttackSE;
        weaponAudio.Play();
    }
}
