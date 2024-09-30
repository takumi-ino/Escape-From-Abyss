using UnityEngine;

public class SkeletonController : EnemyBaseController
{
    [SerializeField] AudioClip slashAttackSE;

    public SkeletonController() : base("ƒXƒPƒ‹ƒgƒ“", 4, 3f, 5f, 5f, 4f) { }

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void PlaySlashAttackSE()
    {
        weaponAudio.clip = slashAttackSE;
        weaponAudio.Play();
    }
}
