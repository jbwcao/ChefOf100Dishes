using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TofuShoot : TomatoLob
{
    private bool isAttacking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }


    //funny unintended effects happen without isAtacking
    protected override IEnumerator Attack(Vector3 attackLocation)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            yield return Shake(0.6f, 0.2f);
            yield return base.Attack(attackLocation);
            isAttacking = false;
        }
        
    }


    //shakes only on the x axis
    IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration) {
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPos.y;
            transform.position = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
}

    
}

