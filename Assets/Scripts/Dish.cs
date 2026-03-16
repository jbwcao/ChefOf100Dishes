using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/NewScriptableObjectScript")]
public class NewScriptableObjectScript : ScriptableObject
{
    public string name; 
    public Sprite sprite;


    public override string ToString()
    {
        return $"{name}";
    }
}
