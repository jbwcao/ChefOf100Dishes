using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class EnemyCounterScript : MonoBehaviour
{
    public static EnemyCounterScript Instance;
    private int? _enemiesLeft;

    public int? EnemiesLeft 
    {
        get 
        {   if (_enemiesLeft == null)
            {
                return 0;
            }
            return _enemiesLeft; 
        } 
        set 
        {
            _enemiesLeft = value; 
            if (_enemiesLeft == 0)
            {
                SceneManager.LoadScene("RPG Phase");
            }
        } 
    }

    void Awake()
    {
        Instance = this;
    }
}
