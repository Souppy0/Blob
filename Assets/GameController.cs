using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using NUnit.Framework;
public class GameController2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

 int progressAmount;
    public Slider progressSlider;

    void Start()
    {
        progressAmount = 0; // starts @ 0 
        progressSlider.value = 0;
        Gem.OnGemCollect += IncreaseProgressAmount;
    }
    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;
        if (progressAmount >= 100)
        {
            Debug.Log("level complete");
            //level done (transition to end screen / nextt level )
        }
    }
    void Update()
    {
        
    }
}
