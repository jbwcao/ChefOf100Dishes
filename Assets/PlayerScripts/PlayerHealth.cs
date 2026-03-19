using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHP = 3;
    private int currentHP;
    public float iframeLength = 1.5f;
    private float ifameTimer;


    public Rigidbody2D PlayerRB;

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

    public void takeDamage(int damage)
    {
        if (ifameTimer <= 0)
        { 
        
            currentHP -= damage;

            if(currentHP <= 0)
            {
                death();
            }

            ifameTimer = iframeLength;
        }
    }

    void death()
    {
        //TODO: replace with death animation then transition over to game over
        Destroy(this.gameObject);
    }
}
