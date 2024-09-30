using UnityEngine;

// �ŏI�X�e�[�W�Ńv���C���[��������֔�΂�

public class AddUpperForceToPlayer : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    float force = 14f;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}