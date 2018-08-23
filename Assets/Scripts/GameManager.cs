using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  
  public GameObject TilePre;
  public int mapRad;
  public List<List<Tile>> map;
  public int actionAlign;
  public float tileSize;
  public List<PlayerControl.Alignment> aligns;
  [HideInInspector] public PlayerControl activeUnit;
  private int tileMask;
  public UnityEngine.UI.Text alignText;
  public UnityEngine.UI.Text APText;
  public ButtonListen button;
  List<PlayerControl> moved;
  
  int AP;
  private Object[] tilePres;

  // Use this for initialization
  void Awake () {
    resetAP();
    moved = new List<PlayerControl>();
    tileMask = 1<<1;
    mapGen();
    aligns.Add( PlayerControl.Alignment.overJustice);
    aligns.Add( PlayerControl.Alignment.neutral);
    activeUnit = null;
    alignText.text = "" + aligns[actionAlign];
    button = GameObject.Find("ButtonListener").GetComponent<ButtonListen>();
  }
	
  // Update is called once per frame
  void Update () {
    if( Input.GetMouseButtonDown(0) ) {
      if( CastRayToUnit(Input.mousePosition) && CastRayToUnit(Input.mousePosition).align == aligns[actionAlign]
	  && CastRayToUnit(Input.mousePosition).alive && (activeUnit == null || activeUnit.stts == PlayerControl.Status.standBy) ) {

	if( activeUnit != null ) {
	  activeUnit.selected = false;
	  if( activeUnit.actions[0] == true && activeUnit.actions[1] == true && activeUnit.actions[2] == true ) {
	    useAP(-activeUnit.moved);
	    activeUnit.returnActions();
	  }
	}

	activeUnit = CastRayToUnit(Input.mousePosition);
	if( activeUnit.moved != 0 || AP > 0 ) {
	  activeUnit.selected = true;
	  activeUnit.button.reset();
	  moved.Add(activeUnit);
	  if( activeUnit.moved == 0 ) {
	    moved.Add(activeUnit);
	    activeUnit.refillActions();
	    useAP(activeUnit.moved);
	 }
	}
      }
    }

    if( (Input.GetKey("r") || (button.refillClick && button.isReady))
      && activeUnit != null && activeUnit.moved+1 <= AP &&
      (activeUnit.actions[0] == false || activeUnit.actions[1] == false || activeUnit.actions[2] == false)) {
	activeUnit.refillActions();
	useAP( activeUnit.moved );
	button.reset();
	button.ready();
    }

    if( Input.GetKey("e") ) {
	PhaseShift();
    }
  }

  // Generate map
  void mapGen () {  
    tilePres = Resources.LoadAll("Tiles");
    map = new List<List<Tile>>();
    for( int i = -mapRad; i <= mapRad; i++ ) {
      List<Tile> row = new List<Tile>();
      for( int j = -mapRad; j <= mapRad; j++ ) {
	row.Add(TileGen(i, j, Random.Range((int)0, tilePres.Length)));
      }
      map.Add(row);
    }
  }

  // Generate Tile
  Tile TileGen (float q, float r, int tilePre) {
    Tile tile = ((GameObject) Instantiate( tilePres[tilePre], new Vector3((q+r/2)*Mathf.Sqrt(3)*tileSize, (float)3/(float)2*r*tileSize, 0),
      new Quaternion(0,0,0,0))).GetComponent<Tile>();
    tile.Q = q;
    tile.R = r;
    return tile;
  }

  public Tile OnMap( int q, int r ) {
    return map[q+mapRad][r+mapRad];
  }

  public bool Bounded( int q, int r, int i, int j ) {
    if( q+i >= -mapRad && q+i <= mapRad && r+j >= -mapRad && r+j <= mapRad) {
      return true;
    } else {
      return false;
    }
  }

  public void PhaseShift() {
    if( activeUnit != null ) {
      activeUnit.selected = false;
      activeUnit = null;
    }
    if( actionAlign >= aligns.Count - 1 ) {
      actionAlign = 0;
    } else {
      actionAlign++;
    }
    alignText.text = "" + aligns[actionAlign];
    resetAP();

    for( int i = 0; i < moved.Count; i++ ) {
      moved[i].moved = 0;
    }
    moved.Clear();
  }

  PlayerControl CastRayToUnit( Vector2 pos ) {
	  Ray ray = Camera.main.ScreenPointToRay(pos);
	  RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity, tileMask);


	  if(hit.transform.gameObject.GetComponent<Tile>()) {
	    Tile tile =  hit.transform.gameObject.GetComponent<Tile>();
	    if( tile.unit != null ) {
	      return tile.unit;
	    } else {
	      return null;
	    }
	  } else {
	    return null;
	  }
  }

  void resetAP() {
    /*AP += 3;
    if( AP > 4 ) {
      AP = 4;
    }*/

    AP = 3;
    APText.text = "AP left: " + AP; 
  }	

  void useAP( int use ) {
    AP -= use;
    APText.text = "AP left: " + AP;
  }
}
