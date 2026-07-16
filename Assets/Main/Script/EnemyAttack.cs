using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 10f;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit : " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Hit");

            PlayerHealth hp = other.GetComponent<PlayerHealth>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
            else
            {
                Debug.Log("PlayerHealth‚ªŒ©‚Â‚©‚ç‚È‚¢");
            }
        }
    }
}