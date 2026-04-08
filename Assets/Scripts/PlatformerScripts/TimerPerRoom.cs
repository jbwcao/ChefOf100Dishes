using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerPerRoom : MonoBehaviour
{

    public float startTime = 15f;
    private float timeLeft;
    public String nextScene;

    public Slider slider;   
    public Gradient gradient;
    public Image fill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = startTime;
        fill.color = gradient.Evaluate(1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            slider.value = timeLeft/startTime;
            fill.color = gradient.Evaluate(timeLeft/startTime);
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }


}
