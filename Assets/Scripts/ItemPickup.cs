using System.Collections;
using Mono.Cecil;
using UnityEditor.VersionControl;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    //Issues:
    //need reference to ingridents for sprite and data
    //need to remove game object when player interacts with object
    //adding items to player inventory(using game manager)
    //solve how to handle overlaping cases
    //Player can interact with dropped items(layer issue)


    Vector2 itemLobDirection;
    public float minPower = 10;
    public float maxPower = 15;
    
    private bool playerInRange = false;


    public float glowStartupTime = 0.2f;
    private Color originalColor;

    public Ingredient assignedItem;
    public Rigidbody2D itemRB;
    public Collider2D interactableArea;
    public SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //on start pick a random direction
        itemLobDirection.x = Random.Range(-0.5f, 0.5f);
        itemLobDirection.y = Random.Range(0f, 1f);
        
        //launch item at random velocity in a random direction that isn't down
        lob();
        GetComponent<SpriteRenderer>().sprite = assignedItem.sprite;
        //resizeSprite(new Vector2(0.5f, 0.5f));
        //sr.sprite = assignedItem.sprite;
        originalColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //stuff needed for I-Interactable
    public void Interact(GameObject interactor)
    {
        Debug.Log(assignedItem.name + " interacted by " + interactor.name);
        GameManager.Instance.addInventory(assignedItem);

        Destroy(gameObject);
    }

    //used to calculate closest interactable to player
    public Transform GetTransform()
    {
        return transform;
    }



    void lob()
    {
        itemRB.linearVelocity = itemLobDirection * Random.Range(minPower, maxPower);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        //only glow if the item has landed
        if (collision.transform.CompareTag("Player") && itemRB.linearVelocity.magnitude < 0.01f && playerInRange == false)
        {
            StartCoroutine(Glow());
            playerInRange = true;//do somthing with playerInRange
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && itemRB.linearVelocity.magnitude < 0.01f && playerInRange == true)
        {
            StartCoroutine(GoBack());
            playerInRange = false;
        }
        
    }


    public void resizeSprite(Vector2 size)
    {
        if (sr == null || sr.sprite == null)
            return;

        Vector2 spriteSize = sr.sprite.bounds.size;
        Vector2 targetSize = GetComponent<BoxCollider2D>().size;

        transform.localScale = new Vector3(
            targetSize.x / spriteSize.x,
            targetSize.y / spriteSize.y,
            1f
        );
    }


    IEnumerator Glow()
    {
        
        float t = 0f;

        while (t < glowStartupTime)
        {
            t += Time.deltaTime;
            sr.color = Color.Lerp(originalColor, Color.white, t / glowStartupTime);
            yield return null;
        }

        sr.color = Color.white;
    }

    IEnumerator GoBack()
    {

        float t = 0f;

        while (t < glowStartupTime)
        {
            t += Time.deltaTime;
            sr.color = Color.Lerp(Color.white, originalColor, t / glowStartupTime);
            yield return null;
        }
        sr.color = originalColor;

    }



}
