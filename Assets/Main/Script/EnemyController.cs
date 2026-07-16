using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("攻撃設定")]
    public Collider attackCollider;
    public GameObject attackEffect;
    public Transform attackObject;

    [Header("攻撃オブジェクト設定")]
    public float attackDistance = 1f;   // 移動距離
    public float attackSpeed = 0.08f;   // 攻撃速度

    public Vector3 attackRotation = new Vector3(0, 0, -90f); // 攻撃時の回転
    public float rotationSpeed = 0.08f; // 回転速度

    [Header("攻撃間隔")]
    public float attackInterval = 2f;

    [Header("攻撃前移動")]
    [SerializeField] private Transform moveObject;   // ← Inspectorでアタッチ
    public float moveDistance = 0.5f;
    public float moveDuration = 0.15f;

    [Header("サウンド")]
    public AudioClip attackSound;
    private AudioSource audioSource;

    //[Range(0f, 1f)]
    //public float attackChance = 0.5f; // 50%

    private EnemyHealth hp;
    private bool isAttacking = false;

    void Start()
    {
        hp = GetComponent<EnemyHealth>();

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

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
            // 2秒間の中でランダムな待機時間
            float randomTime = Random.Range(0f, attackInterval);

            yield return new WaitForSeconds(randomTime);

            if (!isAttacking)
            {
                yield return StartCoroutine(AttackRoutine());
            }

            // 次の攻撃まで待つ
            yield return new WaitForSeconds(attackInterval - randomTime);
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 敵本体が前に出て戻る
        yield return StartCoroutine(PrepareAttackMove());

        // 少し溜める
        yield return new WaitForSeconds(0.2f);

        // attackObjectを動かす（終わるまで待つ）
        yield return StartCoroutine(AttackMotion());

        // 攻撃音
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // attackObjectを動かす
        yield return StartCoroutine(AttackMotion());
        // コライダーON
        //attackCollider.enabled = true;

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

        yield return new WaitForSeconds(0.3f);

       // attackCollider.enabled = false;

        isAttacking = false;
    }

    IEnumerator AttackMotion()
    {
        if (attackObject == null)
            yield break;

        Vector3 startPos = attackObject.localPosition;
        Quaternion startRot = attackObject.localRotation;

        Vector3 attackPos = startPos + Vector3.left * attackDistance;
        Quaternion targetRot = Quaternion.Euler(attackRotation);

        float t = 0f;

        // 攻撃（移動＋回転）
        while (t < attackSpeed)
        {
            t += Time.deltaTime;

            float progress = t / attackSpeed;

            attackObject.localPosition = Vector3.Lerp(
                startPos,
                attackPos,
                progress
            );

            attackObject.localRotation = Quaternion.Lerp(
                startRot,
                targetRot,
                progress
            );

            yield return null;
        }

        attackObject.localPosition = attackPos;
        attackObject.localRotation = targetRot;


        // 元に戻す
        t = 0f;

        while (t < rotationSpeed)
        {
            t += Time.deltaTime;

            float progress = t / rotationSpeed;

            attackObject.localPosition = Vector3.Lerp(
                attackPos,
                startPos,
                progress
            );

            attackObject.localRotation = Quaternion.Lerp(
                targetRot,
                startRot,
                progress
            );

            yield return null;
        }

        attackObject.localPosition = startPos;
        attackObject.localRotation = startRot;
    }
    IEnumerator PrepareAttackMove()
    {
        if (moveObject == null)
            yield break;

        Vector3 startPos = moveObject.localPosition;
        Vector3 targetPos = startPos + Vector3.left * moveDistance;

        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            moveObject.localPosition = Vector3.Lerp(startPos, targetPos, t / moveDuration);
            yield return null;
        }

        moveObject.localPosition = targetPos;

        yield return new WaitForSeconds(0.05f);

        t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            moveObject.localPosition = Vector3.Lerp(targetPos, startPos, t / moveDuration);
            yield return null;
        }

        moveObject.localPosition = startPos;
    }
}