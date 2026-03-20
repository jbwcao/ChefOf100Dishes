using UnityEngine;

public class EnemyRegistration : MonoBehaviour
{
    void Start()
    {
        EnemyCounterScript.Instance.EnemiesLeft += 1;
    }

    private void OnDestroy()
    {
        EnemyCounterScript.Instance.EnemiesLeft -= 1;
    }
}
