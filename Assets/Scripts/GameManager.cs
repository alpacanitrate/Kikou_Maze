using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {
  
  public GameObject TilePre;
  public int mapRad;
  public List<List<Tile>> map;
  public int actionAlign;
  public float tileSize;
  public List<PlayerControl.Alignment> aligns;
  public List<List<PlayerControl>> alignUnits;
  public List<int> alignFantasies;
  [HideInInspector] public PlayerControl activeUnit;
  private int tileMask;
  public UnityEngine.UI.Text alignText;
  public UnityEngine.UI.Text APText;
  public UnityEngine.UI.Text actionText;
  public UnityEngine.UI.Text winText;
  public UnityEngine.UI.Text fantasyText;
  public SummonPanel summonPanel;
  public UnityEngine.UI.Button summonButton;
  public ButtonListen button;
  

  public List<GameObject> buildTable;
  public List<GameObject> masterTable;
  List<PlayerControl> masters;
  int mastersAlive;

  [HideInInspector] int turns;


  List<int> AP;
  private UnityEngine.Object[] tilePres;


  // Use this for initialization
  void Awake () {
    turns = 1;
    tileMask = 1<<1;
    mapGen();
    aligns.Add( PlayerControl.Alignment.OverJustice);
    aligns.Add( PlayerControl.Alignment.Neutral);
    alignUnits = new List<List<PlayerControl>>();
    alignFantasies = new List<int>();

    masters = new List<PlayerControl>();
    AP = new List<int>();
    for( int i = 0; i < aligns.Count; i++ ) {
      alignUnits.Add(new List<PlayerControl>());
      masters.Add( Instantiate( masterTable[i], new Vector3(0, 0, -1), new Quaternion(0, 0, 0, 0) ).GetComponent<PlayerControl>() );
      alignUnits[i].Add( masters[i] );
      alignFantasies.Add(70);
      masters[i].leading = OnMap(-2+i*4, -1+i*2);
      masters[i].GetComponent<Rigidbody2D>().MovePosition( masters[i].leading.transform.position );
      masters[i].leading.unit = masters[i];
      masters[i].align = aligns[i];
      masters[i].isMaster = true;
      masters[i].canSummon = true;
      //PlayerControl[] allUnits = (PlayerControl[])GameObject.FindObjectsOfType(typeof(PlayerControl));
      AP.Add(0);
      //for( int j = 0; j < allUnits.Length; j++ ) {
	//if( allUnits[j].align == aligns[i] ) {
	  //Debug.Log(allUnits[j]);
	  //alignUnits[i].Add( allUnits[j] );
	//}
      //}
    }
    mastersAlive = masters.Count;

    summonPanel.setSprites( buildTable );
    
    resetAP();
    activeUnit = null;
    fantasyText.text = "" + alignFantasies[actionAlign];
    alignText.text = "Turn" + turns + "-" + aligns[actionAlign];
    button = GameObject.Find("ButtonListener").GetComponent<ButtonListen>();
  }
	
  // Update is called once per frame
  void Update () {
    if( Input.GetMouseButtonDown(0) ) {
      if( CastRayToUnit(Input.mousePosition) && CastRayToUnit(Input.mousePosition).align == aligns[actionAlign]
	  && CastRayToUnit(Input.mousePosition).alive && (activeUnit == null || activeUnit.stts == PlayerControl.Status.standBy) ) {

	button.reset();
	if( activeUnit != null ) {
	  activeUnit.selected = false;
	  activeUnit.leading.markSelection(false);
	  if( activeUnit.actions[0] == true && activeUnit.actions[1] == true && activeUnit.actions[2] == true ) {
	    useAP(-activeUnit.moved);
	    activeUnit.returnActions();
	  }
	}

	activeUnit = CastRayToUnit(Input.mousePosition);
	//if(  activeUnit.actions[0] == true || activeUnit.actions[1] == true || activeUnit.actions[2] == true || AP[actionAlign] > activeUnit.moved ) {
	  activeUnit.selected = true;
	  activeUnit.leading.markSelection(true);
	  activeUnit.button.reset();
	  if( activeUnit.moved == 0 && AP[actionAlign] > 0 ) {
	    alignText.text = "Turn" + turns + "-" + aligns[actionAlign];
	    activeUnit.refillActions();
	    useAP(activeUnit.moved);
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
    StreamReader sr = new StreamReader(MapManager.mapPath);
    string radString = sr.ReadLine();
    
    int loadRad = -1;
    if( radString == "" ) {
      return;
    } else {
      Int32.TryParse(radString, out loadRad);
    }
    
    if( loadRad == -1 ) {
      return;
    }

    map = new List<List<Tile>>();
    for( int i = -loadRad; i <= loadRad; i++ ) {
      string[] rowStrs = sr.ReadLine().Split(' ');
      List<Tile> row = new List<Tile>();
      for( int j = -loadRad; j <= loadRad; j++ ) {
	int pre = 0;
	Int32.TryParse(rowStrs[j + loadRad], out pre);
	row.Add(TileGen(i, j, pre));
      }
      map.Add(row);
    }
    mapRad = loadRad;
    sr.Close();
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
      activeUnit.leading.markSelection( false );
      activeUnit = null;
    }
    if( actionAlign >= aligns.Count - 1 ) {
      actionAlign = 0;
    } else {
      actionAlign++;
    }

    turns++;
    PlayerControl[] allUnits = (PlayerControl[])GameObject.FindObjectsOfType(typeof(PlayerControl));
    for( int i = 0; i < allUnits.Length; i++ ) {
      allUnits[i].passTurn();
    }

    for( int j = 0; j < aligns.Count; j++ ) {
      alignFantasies[j] += 20;
    }


    resetAP();

    alignText.text = "Turn" + turns + "-" + aligns[actionAlign];
    actionText.text = "Actions";
    fantasyText.text = "" + alignFantasies[actionAlign];
  }

  public void addUnit( Tile tile, int unitNumber ) {
    PlayerControl minion = Instantiate( buildTable[unitNumber],  new Vector3(0, 0, -1), new Quaternion(0, 0, 0, 0) ).GetComponent<PlayerControl>();
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
/**
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
      alignFantasies[j] += 30;
    }

    for( int k = 0; k < aligns.Count; k++ ) {
      turnActions[k] = maxTurnActions;
    }
  }
  **/

  void resetAP() {
    AP[actionAlign] += 3;
    if( AP[actionAlign] > 4 ) {
      AP[actionAlign] = 4;
    }

    //if( turnActions[actionAlign] - cTurnActions[actionAlign] >= 3 ) {
      //AP[actionAlign] = 3;
    //} else { 
      //AP[actionAlign] = turnActions[actionAlign] - cTurnActions[actionAlign];
    //}
    APText.text = "AP left: " + AP[actionAlign];
  }

  void useAP( int use ) {
    AP[actionAlign] -= use;
    APText.text = "AP left: " + AP[actionAlign];
    alignText.text = "Turn" + turns + "-" + aligns[actionAlign];
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

  public int getFantasies() {
    return alignFantasies[actionAlign];
  }

  public void costFantasies( int cost ) {
    alignFantasies[actionAlign] -= cost;
    fantasyText.text = "" +alignFantasies[actionAlign];
  }
}
