﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : MonoBehaviour {

    public FishStatus[] listaPeixes;

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(listaPeixes[1].chanceAppear);
	}

}
