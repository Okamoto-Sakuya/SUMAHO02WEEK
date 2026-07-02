using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Transform startPoint;
    public Transform leftPoint;
    public Transform rightPoint;

    public EnemyController enemy;

    private bool canAction = true;
    private bool canAttack = true;

    void Start()
    {
        transform.position = startPoint.position;
    }

    void Update()
    {
        if (!canAction)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            Attack();
        }
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

        transform.position = target.position;

        yield return new WaitForSeconds(2f);

        transform.position = startPoint.position;

        canAction = true;
    }

    void Attack()
    {
        if (!canAttack) return;

        enemy.TakeDamage(10);
        enemy.StartAttack();

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(1.5f); // ←ここがクールダウン

        canAttack = true;
    }
}