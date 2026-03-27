using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Explosive : MonoBehaviour
{
    public int damage = 2;
    public float knockBack;
    public float armTime = 1.5f;
    private float countDown;

    public float blinkStartTime = 0.25f; // will fluctuate from white, to normal in 0.25 seconds, then faster the next time
    public float blinkEndTime = 0.01f;
    public Color blinkColor = Color.white;
    private float currentBlinkTime;
    private bool currentlyBlinking = false;
    
    public float squezeAndStreach;

    public SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countDown = armTime;
        currentBlinkTime = blinkStartTime;
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
        //TODO fill this with explosion logic
        //check for player in explosion radius, then apply knockback and damage
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
}
