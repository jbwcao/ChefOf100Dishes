using System.Collections.Generic;
using UnityEngine;

public class DoorWithSignScript : MonoBehaviour
{
    public List<string> sceneNamesPool = new List<string>();
    public List<Sprite> ingredientSprites = new List<Sprite>(); // Corresponding sprite to the scene
    public GameObject signIngredient;

    public string sceneName;
    public Sprite ingredient;
    void Start()
    {
        int randomIndex = Random.Range(0, sceneNamesPool.Count);
        sceneName = sceneNamesPool[randomIndex];
        ingredient = ingredientSprites[randomIndex];

        signIngredient.GetComponent<SpriteRenderer>().sprite = ingredient;
    }

}
