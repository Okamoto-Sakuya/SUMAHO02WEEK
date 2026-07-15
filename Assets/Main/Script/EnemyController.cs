using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("攻撃設定")]
    public Collider attackCollider;
    public GameObject attackEffect;
    public Transform attackObject;

    [Header("攻撃間隔")]
    public float attackInterval = 2f;

    [Range(0f, 1f)]
    public float attackChance = 0.5f; // 50%

    private EnemyHealth hp;
    private bool isAttacking = false;

    void Start()
    {
        hp = GetComponent<EnemyHealth>();

        attackCollider.enabled = false;

        // 攻撃ループ開始
        StartCoroutine(AttackLoop());
    }

    public void TakeDamage(int damage)
    {
        hp.TakeDamage(damage);
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            // 攻撃中でなければランダム判定
            if (!isAttacking && Random.value <= attackChance)
            {
                yield return StartCoroutine(AttackRoutine());
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 攻撃前の待機
        yield return new WaitForSeconds(1f);

        // 攻撃モーション
        if (attackObject != null)
        {
            StartCoroutine(AttackMotion());
        }

        // コライダーON
        attackCollider.enabled = true;

        // エフェクト生成
        if (attackEffect != null)
        {
            GameObject effect = Instantiate(
                attackEffect,
                attackCollider.bounds.center,
                attackCollider.transform.rotation
            );

            ParticleSystem ps = effect.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                ps.Play();
                Destroy(effect, ps.main.duration + 0.5f);
            }
            else
            {
                Destroy(effect, 2f);
            }
        }

        // 攻撃判定時間
        yield return new WaitForSeconds(0.3f);

        // コライダーOFF
        attackCollider.enabled = false;

        isAttacking = false;
    }

    IEnumerator AttackMotion()
    {
        Vector3 original = attackObject.position;

        float distance = 1.0f;
        float speed = 0.05f;

        // 左へ
        attackObject.position = original + Vector3.left * distance;
        yield return new WaitForSeconds(speed);

        // 右へ
        attackObject.position = original + Vector3.right * distance;
        yield return new WaitForSeconds(speed);

        // 元の位置
        attackObject.position = original;
    }
}