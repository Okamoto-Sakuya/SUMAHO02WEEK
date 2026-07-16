using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Vector2 swipeStartPos;
    private bool isSwiping = false;

    [SerializeField]
    private float swipeDistance = 80f; // 必要なスワイプ距離

    [Header("移動設定")]
    public float moveTime = 0.1f; // 左右移動時間
    public float stayTime = 1f;   // 滞在時間

    public Transform startPoint;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform attackObject;

    public GameObject attackEffect;   // エフェクトPrefab
    public Transform effectPoint;     // エフェクトを出す位置

    public EnemyController enemy;

    public Transform cameraTransform; // 揺らすカメラ

    public AudioClip attackSound;      // 攻撃音
    private AudioSource audioSource;

    private bool canAction = true;
    private bool canAttack = true;

    void Start()
    {
        transform.position = startPoint.position;

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!canAction)
            return;

#if UNITY_EDITOR || UNITY_STANDALONE
        // ===== マウス =====

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            swipeStartPos = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 endPos = Input.mousePosition;
            Vector2 delta = endPos - swipeStartPos;

            // 横スワイプ判定
            if (Mathf.Abs(delta.x) > swipeDistance &&
                Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                Attack();
            }

            isSwiping = false;
        }

#else
    // ===== スマホ =====

    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                swipeStartPos = touch.position;
                break;

            case TouchPhase.Ended:
                Vector2 delta = touch.position - swipeStartPos;

                if (Mathf.Abs(delta.x) > swipeDistance &&
                    Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    Attack();
                }
                break;
        }
    }
#endif
    }

    public void MoveLeft()
    {
        if (!canAction) return;

        StartCoroutine(MoveRoutine(leftPoint));
    }

    public void MoveRight()
    {
        if (!canAction) return;

        StartCoroutine(MoveRoutine(rightPoint));
    }

    IEnumerator MoveRoutine(Transform target)
    {
        canAction = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = target.position;

       // float moveTime = 0.25f; // 移動時間
        float t = 0f;

        // 左右へ移動
        while (t < moveTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / moveTime);
            yield return null;
        }

        transform.position = endPos;

        // 左右にいる時間
        yield return new WaitForSeconds(stayTime);

        // 元の位置へ戻る
        startPos = transform.position;
        endPos = startPoint.position;

        t = 0f;

        while (t < moveTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / moveTime);
            yield return null;
        }

        transform.position = endPos;

        canAction = true;
    }

    void Attack()
    {
        if (!canAttack) return;
        StartCoroutine(AttackRoutine());
        // 攻撃音
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        StartCoroutine(AttackMotion());

        // カメラシェイク
        if (cameraTransform != null)
        {
            StartCoroutine(CameraShake());
        }

        // エフェクト生成
        if (attackEffect != null && effectPoint != null)
        {
            GameObject effect = Instantiate(
                attackEffect,
                effectPoint.position,
                effectPoint.rotation
            );

            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(effect, 2f);
            }
        }

        if (enemy != null)
        {
            enemy.TakeDamage(10);
            //enemy.StartAttack();
        }

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackMotion()
    {
        Vector3 original = attackObject.position;

        float distance = 1.0f;
        float speed = 0.05f;

        attackObject.position = original + Vector3.right * distance;
        yield return new WaitForSeconds(speed);

        attackObject.position = original + Vector3.left * distance;
        yield return new WaitForSeconds(speed);

        attackObject.position = original;
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(1.5f);

        canAttack = true;
    }
    IEnumerator CameraShake()
    {
        Vector3 originalPos = cameraTransform.localPosition;

        float duration = 0.15f; // 揺れる時間
        float power = 0.2f;      // 揺れる強さ

        float timer = 0;

        while (timer < duration)
        {
            float x = Random.Range(-power, power);
            float y = Random.Range(-power, power);

            cameraTransform.localPosition = originalPos + new Vector3(x, y, 0);

            timer += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }

    IEnumerator AttackRoutine()
    {
        canAttack = false;

        // 攻撃前の構え
        Vector3 original = attackObject.position;
        attackObject.position = original + Vector3.left * 0.3f;

        // 少し溜める
        yield return new WaitForSeconds(0.15f);

        // 攻撃音
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // 攻撃モーション
        StartCoroutine(AttackMotion());

        // カメラシェイク
        if (cameraTransform != null)
        {
            StartCoroutine(CameraShake());
        }

        // エフェクト
        if (attackEffect != null && effectPoint != null)
        {
            GameObject effect = Instantiate(
                attackEffect,
                effectPoint.position,
                effectPoint.rotation
            );

            ParticleSystem ps = effect.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                ps.Play();
                Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(effect, 2f);
            }
        }

        // ダメージ
        if (enemy != null)
        {
            enemy.TakeDamage(10);
        }

        // クールタイム
        yield return new WaitForSeconds(1.5f);
        canAttack = true;
    }
}