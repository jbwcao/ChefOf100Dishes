using UnityEngine;

[CreateAssetMenu(fileName = "Dish", menuName = "Scriptable Objects/Dish")]
public class Dish : ScriptableObject
{
    public string name; 
    public Sprite sprite;


    public override string ToString() {
        return $"{name}";
    } 
}
