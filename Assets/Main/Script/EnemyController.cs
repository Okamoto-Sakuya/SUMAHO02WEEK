using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Collider attackCollider;

    public GameObject attackEffect;   // エフェクトPrefab
    public Transform attackObject;    // 動かしたいオブジェクト

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

    public void StartAttack()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        // 攻撃前の待機
        yield return new WaitForSeconds(1f);


        // 攻撃モーション開始
        if (attackObject != null)
        {
            StartCoroutine(AttackMotion());
        }


        // コライダーON
        attackCollider.enabled = true;


        // コライダー位置からエフェクト発生
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
    }


    IEnumerator AttackMotion()
    {
        Vector3 original = attackObject.position;

        float distance = 1.0f;
        float speed = 0.05f;


        // 左へ振る
        attackObject.position = original + Vector3.left * distance;
        yield return new WaitForSeconds(speed);


        // 右へ戻す
        attackObject.position = original + Vector3.right * distance;
        yield return new WaitForSeconds(speed);


        // 元の位置
        attackObject.position = original;
    }
}