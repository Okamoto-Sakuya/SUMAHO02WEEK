using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public float maxHP = 100f;
    float currentHP;

    public Slider hpSlider;

    void Start()
    {
        currentHP = maxHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
            currentHP = 0;

        hpSlider.value = currentHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Dead");

        // クリアシーンへ移動
        SceneManager.LoadScene("ClearScene");
    }
}