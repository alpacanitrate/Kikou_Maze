﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderValueUpdate : MonoBehaviour {
	public UnityEngine.UI.Slider slider;

	
	public void UpdateValue () {
	  GetComponent<UnityEngine.UI.Text>().text = "" + slider.value;
	}
}
