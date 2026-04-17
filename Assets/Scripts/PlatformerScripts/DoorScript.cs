using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public List<string> sceneNamesPool = new List<string>();
    public string sceneName;
    void Start()
    {
        sceneName = sceneNamesPool[Random.Range(0, sceneNamesPool.Count)];
    }

}
