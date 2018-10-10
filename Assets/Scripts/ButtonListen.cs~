using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListen : MonoBehaviour {
	public bool attackClick;
	public bool moveClick;
	public bool refillClick;
	public bool summonClick;

	public bool isReady;

	// Use this for initialization
	void Start () {
	  
	  reset();	
	}
	
	// Update is called once per frame
	void Update () {
	  isReady = true;
	}

	public void reset() {
	  attackClick = false;
	  moveClick = false;
	  refillClick = false;
	}

	public void attackClicked() {
	  attackClick = true;
	  isReady = false;
	}
	public void moveClicked() {
	  moveClick = true;
	  isReady = false;
	}

	public void refillClicked() {
	  refillClick = true;
	  isReady = false;
	}

	public void summonClicked() {
	  summonClick = true;
	  isReady = false;
	}

	public void ready() {
	  isReady = true;
	}
}
