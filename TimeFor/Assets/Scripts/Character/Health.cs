using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;

    public int mana;
    public int maxMana = 100;

    [SerializeField] Slider healthBar;
    [SerializeField] Slider manaBar;

    private void Update()
    {
        healthBar.value = health;
        manaBar.value = mana;
    }

    public void TakeHit(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void SetHealth(int bonushealth)
    {
        health += bonushealth;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void TakeMana(int amount)
    {
        mana -= amount;

        
    }

    public void SetMana(int bonushealth)
    {
        mana += bonushealth;

        if (mana > maxMana)
        {
            mana = maxMana;
        }
    }
}
