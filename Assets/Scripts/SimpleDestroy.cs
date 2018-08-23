﻿using UnityEngine;
using System.Collections;

public class SimpleDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	  Destroy( gameObject, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
	  transform.Translate( new Vector3(0, 0.5f, -1) * Time.deltaTime);
	
	}
}