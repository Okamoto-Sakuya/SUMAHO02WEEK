using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Collider attackCollider;

    private EnemyHealth hp;

    void Start()
    {
        hp = GetComponent<EnemyHealth>();
        attackCollider.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        hp.TakeDamage(damage);
    }

    // Player‚©‚çŒÄ‚Î‚ê‚é
    public void StartAttack()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        // š‚±‚±‚ªu1•b‘Ò‚Âv
        yield return new WaitForSeconds(1f);

        // UŒ‚ON
        attackCollider.enabled = true;

        // UŒ‚ŠÔ
        yield return new WaitForSeconds(0.3f);

        // OFF
        attackCollider.enabled = false;
    }
}