using UnityEngine;
using UnityEngine.UI;
public class RoundSliderUI : MonoBehaviour
{
    Slider RoundSlider; 
    private float maxRound;
    float currRound;
    void Start()
    {
        maxRound = 10f;
        RoundSlider = GetComponent<Slider>();
        currRound = GameManager.Instance.roundRemaining;
        RoundSlider.value = currRound/maxRound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
