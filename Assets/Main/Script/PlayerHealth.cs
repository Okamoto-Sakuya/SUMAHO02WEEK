using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 100f;
    float currentHP;

    public Slider hpSlider;   // ←追加

    void Start()
    {
        currentHP = maxHP;

        // スライダー初期化
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
            currentHP = 0;

        // ★ここが重要（UI更新）
        hpSlider.value = currentHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");

        SceneManager.LoadScene("GameOverScene");
    }
}