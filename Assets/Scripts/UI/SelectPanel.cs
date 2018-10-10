﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPanel: MonoBehaviour {

    public UnityEngine.UI.Button[] buttons;
    public UnityEngine.UI.Button nextButton;
    public UnityEngine.UI.Button prevButton;
    public UnityEngine.UI.Image[] unitImages;
    public UnityEngine.UI.Text pageText;
    public Sprite noneSprite;

    public int clickedButton;

    //public bool ready;
    public GameManager manager;
    public int page;

    void Start () {
	  
      reset();	
      gameObject.SetActive(false);
    }

    public void reset() {
      page  = 0;
      setPage();
      clickedButton  = -1;
      //ready = true;
      gameObject.SetActive(true);
    }

    void setPage() {
    }

    public void click0() {
      //if( ready ) {
	clickedButton  = 0 + page*8;
	//ready = false;
      //}	
    }
    public void click1() {
      //if( ready ) {
	clickedButton  = 1 + page*8;
	//ready = false;
      //}	
    }
    public void click2() {
      //if( ready ) {
	clickedButton  = 2 + page*8;
	//ready = false;
      //}	
    }
    public void click3() {
      //if( ready ) {
	clickedButton  = 3 + page*8;
	//ready = false;
      //}	
    }
    public void click4() {
      //if( ready ) {
	clickedButton  = 4 + page*8;
	//ready = false;
      //}	
    }
    public void click5() {
      //if( ready ) {
	clickedButton  = 5 + page*8;
	//ready = false;
      //}	
    }
    public void click6() {
      //if( ready ) {
	clickedButton  = 6 + page*8;
	//ready = false;
      //}	
    }
    public void click7() {
      //if( ready ) {
	clickedButton  = 7 + page*8;
	//ready = false;
      //}	
    }
    
    public void nextPage() {
      page++;
      prevButton.interactable = true;
      setPage();
    }

    public void prevPage() {
      page--;
      if( page == 0 ) {
	prevButton.interactable = false;
      }
      setPage();
    }
}
