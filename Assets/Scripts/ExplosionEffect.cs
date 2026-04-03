using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private float scaleMultiplier = 1.1f;

    public SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        StartCoroutine(fadeAway());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private IEnumerator fadeAway()
    {

      
        Color startColor = sr.color;
        Vector3 startScale = transform.localScale;

        Color endColor = startColor;
        endColor.a = 0f;

        Vector3 endScale = startScale * scaleMultiplier;

        float elapsed = 0f;



        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            //Ease out using the quartic equation
            float t = elapsed / duration;
            t = 1f - Mathf.Pow(1f - t, 4f);

            sr.color = Color.Lerp(startColor, endColor, t);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        sr.color = endColor;
        transform.localScale = endScale;
        Destroy(gameObject);
        
    

    }

}
