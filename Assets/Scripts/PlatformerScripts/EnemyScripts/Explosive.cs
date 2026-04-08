using System.Collections;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Explosive : MonoBehaviour
{
    public int damage = 2;
    public float explosionRadius = 2f;
    public float knockBack;
    public float armTime = 1.5f;
    private float countDown;


    public bool randomFuseTime = false;
    public float minRandomFuse = 1f;
    public float maxRandomFuse = 1.5f;

    [Header("Bomb Visuals")]
    public float blinkStartTime = 0.25f; // will fluctuate from white, to normal in 0.25 seconds, then faster the next time
    public float blinkEndTime = 0.01f;
    public Color blinkColor = Color.white;
    private float currentBlinkTime;
    private bool currentlyBlinking = false;
    public GameObject explosionCirclePrefab;
    

    public LayerMask hurtMask;    // player/enemy layers
    public LayerMask blockingMask; // terrain/platforms layer

    public SpriteRenderer sr;
    // Should bombs explode on contact to the player
    void Start()
    {
        countDown = armTime;
        currentBlinkTime = blinkStartTime;

        if (randomFuseTime)
        {
            countDown = Random.Range(minRandomFuse, maxRandomFuse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if (!currentlyBlinking)
        {
            StartCoroutine(blinkOnce());
        }

        if(countDown <= 0)
        {
            KaBoom();
        }
    }

    private void KaBoom()
    {
        //explosion visulaized
        ShowExplosionCircle();

        //Check for whats in the blast radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, hurtMask);

        foreach (Collider2D target in hits)
        {
            Vector2 targetPoint = target.bounds.center;

            //checks whether terrain blocks the blast
            RaycastHit2D blockHit = Physics2D.Linecast(transform.position, targetPoint, blockingMask);

            if (blockHit.collider != null)
            {
                // something in terrain blocked the explosion
                continue;
            }

            if (target.CompareTag("Player"))
            {
                target.GetComponent<PlayerHealth>().takeDamage(damage);
                //TODO - replace basic explosion with variables depending how close to the center of the blast
                target.GetComponent<PlayerMovement>().applyKnockback(transform.position);
            }
            
        }

        Destroy(gameObject);
    }

    //flash a color, then shorten time it takes to flash
    //Please note this only changes tint, a shader has to be used for the proper blinking for the proper sprite
    private IEnumerator blinkOnce()
    {
        currentlyBlinking = true;
        Color originalColor = sr.color;
        //halfTime = total time it takes to reach the peak
        float halfTime = currentBlinkTime / 2f;

        float t = 0f;
        while (t < halfTime)
        {
            t += Time.deltaTime;
            sr.color = Color.Lerp(originalColor, blinkColor, t / halfTime);
            yield return null;
        }

        sr.color = blinkColor;

        t = 0f;
        while (t < halfTime)
        {
            t += Time.deltaTime;
            sr.color = Color.Lerp(blinkColor, originalColor, t / halfTime);
            yield return null;
        }

        // The "blinkStartTime/countDown" is abritrary, there's probably somthing visualy cleaner
        currentBlinkTime =  Mathf.Lerp(currentBlinkTime, blinkEndTime, blinkStartTime/countDown);
        sr.color = originalColor;
        currentlyBlinking = false;
    }

    private void ShowExplosionCircle()
    {
        GameObject circle = Instantiate(
            explosionCirclePrefab,
            transform.position,
            Quaternion.identity
        );

        // Assumes the sprite's default diameter is 1 world unit
        float diameter = explosionRadius * 2f;
        circle.transform.localScale = new Vector3(diameter, diameter, 1f);

        
    }
}
