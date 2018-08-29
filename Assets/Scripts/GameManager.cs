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
  public List<List<PlayerControl>> alignUnits;
  [HideInInspector] public PlayerControl activeUnit;
  private int tileMask;
  public UnityEngine.UI.Text alignText;
  public UnityEngine.UI.Text APText;
  public UnityEngine.UI.Text actionText;
  public UnityEngine.UI.Text winText;
  public UnityEngine.UI.Button summonButton;
  public ButtonListen button;
  List<PlayerControl> moved;
  

  public List<GameObject> buildTable;
  public List<GameObject> masterTable;
  List<PlayerControl> masters;
  int mastersAlive;

  [HideInInspector] int turns;
  [HideInInspector] List<int> turnActions;
  [HideInInspector] List<int> cTurnActions;

  List<int> AP;
  private Object[] tilePres;


  // Use this for initialization
  void Awake () {
    turns = 1;
    moved = new List<PlayerControl>();
    tileMask = 1<<1;
    mapGen();
    aligns.Add( PlayerControl.Alignment.OverJustice);
    aligns.Add( PlayerControl.Alignment.Neutral);
    alignUnits = new List<List<PlayerControl>>();
    turnActions = new List<int>();
    cTurnActions = new List<int>();
    masters = new List<PlayerControl>();
    AP = new List<int>();
    for( int i = 0; i < aligns.Count; i++ ) {
      alignUnits.Add(new List<PlayerControl>());
      masters.Add( Instantiate( masterTable[i], new Vector3(0, 0, -1), new Quaternion(0, 0, 0, 0) ).GetComponent<PlayerControl>() );
      alignUnits[i].Add( masters[i] );
      masters[i].leading = OnMap(-2+i*4, 0);
      masters[i].GetComponent<Rigidbody2D>().MovePosition( masters[i].leading.transform.position );
      masters[i].leading.unit = masters[i];
      masters[i].align = aligns[i];
      masters[i].isMaster = true;
      masters[i].canSummon = true;
      //PlayerControl[] allUnits = (PlayerControl[])GameObject.FindObjectsOfType(typeof(PlayerControl));
      turnActions.Add(1);
      cTurnActions.Add(0);
      AP.Add(0);
      //for( int j = 0; j < allUnits.Length; j++ ) {
	//if( allUnits[j].align == aligns[i] ) {
	  //Debug.Log(allUnits[j]);
	  //alignUnits[i].Add( allUnits[j] );
	//}
      //}
    }
    mastersAlive = masters.Count;


    resetAP();
    activeUnit = null;
    alignText.text = "Turn" + turns + "-" + aligns[actionAlign] + " " + cTurnActions[actionAlign] + "/" + turnActions[actionAlign];
    button = GameObject.Find("ButtonListener").GetComponent<ButtonListen>();
  }
	
  // Update is called once per frame
  void Update () {
    if( Input.GetMouseButtonDown(0) ) {
      if( CastRayToUnit(Input.mousePosition) && CastRayToUnit(Input.mousePosition).align == aligns[actionAlign]
	  && CastRayToUnit(Input.mousePosition).alive && (activeUnit == null || activeUnit.stts == PlayerControl.Status.standBy) ) {

	if( activeUnit != null ) {
	  activeUnit.selected = false;
	  activeUnit.leading.markSelection(false);
	  if( activeUnit.actions[0] == true && activeUnit.actions[1] == true && activeUnit.actions[2] == true ) {
	    useAP(-activeUnit.moved);
	    activeUnit.returnActions();
	  }
	}

	activeUnit = CastRayToUnit(Input.mousePosition);
	if( activeUnit.moved != 0 || AP[actionAlign] > 0 ) {
	  activeUnit.selected = true;
	  activeUnit.leading.markSelection(true);
	  activeUnit.button.reset();
	  moved.Add(activeUnit);
	  if( activeUnit.moved == 0 ) {
	    moved.Add(activeUnit);alignText.text = "Turn" + turns + "-" + aligns[actionAlign] + " " + cTurnActions[actionAlign] + "/" + turnActions[actionAlign];
	    activeUnit.refillActions();
	    useAP(activeUnit.moved);
	 }
	}
      }

      if( activeUnit != null ) {
        if( activeUnit.isMaster &&  !summonButton.interactable ) {
	  summonButton.interactable = true;
        } else if( !activeUnit.isMaster &&  summonButton.interactable ) {
	  summonButton.interactable = false;
        }
      }
    }

    if( (Input.GetKey("r") || (button.refillClick && button.isReady))
      && activeUnit != null && activeUnit.moved+1 <= AP[actionAlign] &&
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
    if( cTurnActions[actionAlign] == turnActions[actionAlign] ) {
      turnShift();
    }
    resetAP();

    for( int i = 0; i < moved.Count; i++ ) {
      moved[i].moved = 0;
    }
    moved.Clear();

    alignText.text = "Turn" + turns + "-" + aligns[actionAlign] + " " + cTurnActions[actionAlign] + "/" + turnActions[actionAlign];
    actionText.text = "Actions";
  }

  public void addUnit( Tile tile ) {
    PlayerControl minion = Instantiate( buildTable[actionAlign],  new Vector3(0, 0, -1), new Quaternion(0, 0, 0, 0) ).GetComponent<PlayerControl>();
    minion.leading = tile;
    tile.unit = minion;
    minion.align = aligns[actionAlign];
    minion.GetComponent<Rigidbody2D>().MovePosition( tile.transform.position );
    alignUnits[actionAlign].Add(minion);
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

  void turnShift() {
    turns++;
    PlayerControl[] allUnits = (PlayerControl[])GameObject.FindObjectsOfType(typeof(PlayerControl));
    for( int i = 0; i < allUnits.Length; i++ ) {
      allUnits[i].passTurn();
    }

    int maxTurnActions = 0;
    for( int j = 0; j < aligns.Count; j++ ) {
      if( alignUnits[j].Count > maxTurnActions ) {
	maxTurnActions = alignUnits[j].Count;
      }
      cTurnActions[j] = 0;
    }

    for( int k = 0; k < aligns.Count; k++ ) {
      turnActions[k] = maxTurnActions;
    }
  }

  void resetAP() {
    /*AP += 3;
    if( AP > 4 ) {
      AP = 4;
    }*/

    if( turnActions[actionAlign] - cTurnActions[actionAlign] >= 3 ) {
      AP[actionAlign] = 3;
    } else { 
      AP[actionAlign] = turnActions[actionAlign] - cTurnActions[actionAlign];
    }
    APText.text = "AP left: " + AP[actionAlign];
  }

  void useAP( int use ) {
    AP[actionAlign] -= use;
    cTurnActions[actionAlign] += use;
    APText.text = "AP left: " + AP[actionAlign];
    alignText.text = "Turn" + turns + "-" + aligns[actionAlign] + " " + cTurnActions[actionAlign] + "/" + turnActions[actionAlign];
  }

  public void masterFallen() {
    mastersAlive --;
    if( mastersAlive == 1 ) {
      for( int i = 0; i < masters.Count; i++ ) {
	if( masters[i].alive ) {
	  winText.text = "" + aligns[i] + " is playing to win baby!";
	}
      }
    }
  }
}
