using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPanel: MonoBehaviour {

    public UnityEngine.UI.Button[] buttons;
    public UnityEngine.UI.Button nextButton;
    public UnityEngine.UI.Button prevButton;
    public UnityEngine.UI.Image[] unitImages;
    public UnityEngine.UI.Text pageText;
    public Sprite noneSprite;
    public List<Sprite> sprites;
    public List<string> names;

    public int clickedButton;

    //public bool ready;
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

    public void setSprites( List<GameObject> objects) {
      for( int i = 0; i < objects.Count; i++ ) {
	sprites.Add( objects[i].GetComponent<SpriteRenderer>().sprite );
	names.Add( objects[i].name );
      }
    }



    void setPage() {
      pageText.text = "" + page;
      for( int i = 0; i < 8; i++ ) {
	if(  i + page*8 < sprites.Count ) {
	  unitImages[i].sprite = sprites[i+page*8];
	  buttons[i].GetComponentInChildren<UnityEngine.UI.Text>().text = names[i+page*8];
	  //unitImages[i].sprite =  manager.buildTable[i+page*8].GetComponent<SpriteRenderer>().sprite;
	  //buttons[i].GetComponentInChildren<UnityEngine.UI.Text>().text =  manager.buildTable[i+page*8].name;
	} else {
	  unitImages[i].sprite = noneSprite;
	  buttons[i].GetComponentInChildren<UnityEngine.UI.Text>().text = "无";
	}

      }
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
