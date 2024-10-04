using UnityChan;
using UnityEngine;

public class ScavengerController : EnemyBaseController
{
    [SerializeField] AudioClip slashAttackSE;

    public ScavengerController() : base("スカベンジャー", 4, 4f, 8f, 5f) { }

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void PlaySlapAttackSE()
    {
        weaponAudio.clip = slashAttackSE;
        weaponAudio.Play();
    }
}
