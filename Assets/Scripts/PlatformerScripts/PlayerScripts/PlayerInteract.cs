using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float searchRadius = 2f;
    public LayerMask searchMask; //currently ingredients are on the same mask as bombs, sue me
    public InputActionReference interactAction;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interactAction != null && interactAction.action.WasPressedThisFrame())
        {
            AttemptInteraction();
        }
    }

    //turn on/off ability to interact with items
     private void OnEnable()
    {
        if (interactAction != null)
            interactAction.action.Enable();
    }

    private void OnDisable()
    {
        if (interactAction != null)
            interactAction.action.Disable();
    }


    // look for nearby interacatbles(x.e ingedients)
    // for overlapping cases get the closest interactable
    private void AttemptInteraction()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, searchRadius, searchMask);

        HashSet<IInteractable> found = new HashSet<IInteractable>();
        IInteractable closestInteractable = null;
        float closestDistanceSqr = float.MaxValue;
        
        foreach (Collider2D interacts in nearby)
        {
            if (interacts.CompareTag("Ingredient"))
            {
                IInteractable interactable = interacts.GetComponentInParent<IInteractable>();

                if (interactable == null)
                {
                    continue;
                }
                
                //calculate distance from player
                float distanceSqr = ((Vector2)interactable.GetTransform().position - (Vector2)transform.position).sqrMagnitude;

                //sort to mark the shortest distance
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestInteractable = interactable;
                }
            }
        }

        //once the closest is found run that interactable's interact method(i.e adding ingredient to inventory)
        if (closestInteractable != null)
        {
            closestInteractable.Interact(gameObject);
        }
            
        
    }



    
   
}
