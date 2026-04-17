using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : MonoBehaviour
{
    public bool resetOnDeath = false;
    public string RPGSceneName = "RPG Scene";

    public int maxHP = 3;
    private int currentHP;
    public float iframeLength = 1.5f;
    private float ifameTimer;

    public float damageBlinkTime = 0.1f;
    public SpriteRenderer sr;
    public Rigidbody2D PlayerRB;
    public HealthUI heartUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(ifameTimer > 0)
        {
            ifameTimer -= Time.deltaTime;
            //TODO while iframe timer is going, add a color fluxuation to show iframe length
        }
    }

    public bool takeDamage(int damage)
    {
        if (ifameTimer <= 0)
        { 
            
            currentHP -= damage;
            float healthLeft = (float)currentHP/maxHP;
            Debug.Log("Health % left: " + healthLeft);
            heartUI.spriteUpdate(healthLeft);

            if (currentHP <= 0)
            {
                currentHP = 0;
                Death();
            }
            
            ifameTimer = iframeLength;
            StartCoroutine(BlinkRed());
            return true;
        }
        return false;
    }

    void Death()
    {
        //TODO: replace with death animation then transition over to game over
        if (resetOnDeath)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else
        {
            SceneManager.LoadScene(RPGSceneName);
        }
    }


    private IEnumerator BlinkRed()
    {
        Color original = sr.color;

        while (ifameTimer > 0)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(damageBlinkTime);

            sr.color = original;
            yield return new WaitForSeconds(damageBlinkTime);
        }

        sr.color = original;
    }

}
