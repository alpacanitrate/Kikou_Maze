﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
	// stats
	public CharacterStat HP;
	public CharacterStat mind;
	public CharacterStat melee;
	public CharacterStat ATK;
	public CharacterStat armor;
	public CharacterStat magicResist;
	public CharacterStat evasion;
	public CharacterStat wind;
	public CharacterStat forest;
	public CharacterStat fire;
	public CharacterStat mountain;
	public CharacterStat endurance;
	public CharacterStat movement;
	[HideInInspector]public bool freeMove;

	public CharacterStat burningResist;
	public CharacterStat coldResist;
	public CharacterStat corrosionResist;
	public CharacterStat infestionResist;
	public CharacterStat poisonResist;
	public CharacterStat stopResist;
	public CharacterStat crippleResist;
	public CharacterStat stunResist;
	public CharacterStat silenceResist;
	public CharacterStat breakResist;
	public CharacterStat thornResist;
	public CharacterStat blindsightResist;
	public CharacterStat rageResist;
	public CharacterStat dazzleResist;
	public int cost;

	[HideInInspector] public Ability[] abilities = new Ability[7];
	[HideInInspector] public Ability activeAbility;
	[HideInInspector] public Feat[] feats = new Feat[7];
	public bool isMaster;
	[HideInInspector] public bool canSummon;
	
	public GameManager manager;

	public StatTable table;

	public ButtonListen button; 

	[HideInInspector] public List<CharacterStat> Resistances;

	[HideInInspector] public enum Status {
	  idol = 0,
	  standBy = 1,
	  move = 2,
	  ability = 3,
	  summonSelect = 4,
	  summon = 5,
	  special = 6,
	}
	public Vector2 dir;
	public Tile leading;
	public int moved;
	[HideInInspector]public bool[] actions = new bool[3];


	public bool selected;

	public enum Alignment {
	  OverJustice = 0,
	  Neutral = 1,
	  PurpleForce = 2,
	}
	public Alignment align;
	public bool alive;
	public GameObject YouDied;
	public GameObject youWin;
	public TextMesh DmgText;
        public UnityEngine.UI.Text ActionText;

	[HideInInspector] public Status stts;
	private int moveMask = 1<<1;
	private List<Tile> reachable;
	private List<Tile> past;
	private TextMesh HPtm;
	private TextMesh ACtm;
	

	// Use this for initialization
	void Start () {
	  initStats(table);
	  Resistances = new List<CharacterStat>();
	  Resistances.Add(burningResist);
	  Resistances.Add(coldResist);
	  Resistances.Add(corrosionResist);
	  Resistances.Add(infestionResist);
	  Resistances.Add(poisonResist);
	  Resistances.Add(stopResist);
	  Resistances.Add(crippleResist);
	  Resistances.Add(stunResist);
 	  Resistances.Add(silenceResist);
	  Resistances.Add(breakResist); 
 	  Resistances.Add(thornResist); 
 	  Resistances.Add(blindsightResist);
	  Resistances.Add(rageResist);
	  Resistances.Add(dazzleResist);

	  moved = 0;
	  stts = Status.idol;
	  dir = new Vector2(0,0);
	  reachable = new List<Tile>();
	  past = new List<Tile>();
	  manager = GameObject.Find("GameManager").GetComponent<GameManager>();
	  ActionText = GameObject.Find("ActionText").GetComponent<UnityEngine.UI.Text>();
	  button = GameObject.Find("ButtonListener").GetComponent<ButtonListen>();
	  HPtm = transform.GetChild(0).GetComponent<TextMesh>();
	  ACtm = transform.GetChild(1).GetComponent<TextMesh>();
	  HPtm.text = "" + HP.remain + "/" + mind.remain;
	}
	
	// Update is called once per frame
	void Update () {
	  //transform.Rotate( new Vector3( 0, 0, 10f * Time.deltaTime ));
	  if (stts == Status.idol) {
	    if (selected) {
	      TranStts(Status.standBy);
	      setActionText();
	    }
	  }

	  if (stts == Status.standBy) { 
	    if (!selected) {
	      TranStts(Status.idol);
	    }

	    if (Input.GetKey("s") || (button.moveClick && button.isReady)) {
	      if( checkActions(1) ) {
		TranStts(Status.move);
		PathFind(1, (int)movement.finalValue, false, leading);
		button.reset();
	      } else {
		Debug.Log("Not enough action");
	      }
	    }
	    
	    if (Input.GetKey("g") || (button.summonClick && button.isReady)) {
	      if( checkActions(2) ) {
		if( isMaster && canSummon ) {
		  manager.summonPanel.reset();
		  TranStts(Status.summonSelect);
		  button.reset();
	       }
	      }
	    }
	    

	    if (Input.GetKey("a") || (button.attackClick && button.isReady )) {
	      if( checkActions(abilities[0].cost) ) {
		TranStts(Status.ability);
		activeAbility = abilities[0];
		PathFind(activeAbility.castType, activeAbility.range, activeAbility.penetrate, leading);
		button.reset();
	      } else {
	 	Debug.Log("Not enough Actions");
 	      }
	    }

	   if (Input.GetKey("1")) {
	      if( checkActions(abilities[1].cost) ) {
		TranStts(Status.ability);
		activeAbility = abilities[1];
		PathFind(activeAbility.castType, activeAbility.range, activeAbility.penetrate, leading);
		button.reset();
	      } else {
	 	Debug.Log("Not enough Actions");
 	      }
	   }

	   if (Input.GetKey("2")) {
	      if( checkActions(abilities[2].cost) ) {
		TranStts(Status.ability);
		activeAbility = abilities[2];
		PathFind(activeAbility.castType, activeAbility.range, activeAbility.penetrate, leading);
		button.reset();
	      } else {
	 	Debug.Log("Not enough Actions");
 	      }
	   }

	   if (Input.GetKey("3")) {
	      if( checkActions(abilities[3].cost) ) {
		TranStts(Status.ability);
		activeAbility = abilities[3];
		PathFind(activeAbility.castType, activeAbility.range, activeAbility.penetrate, leading);
		button.reset();
	      } else {
	 	Debug.Log("Not enough Actions");
 	      }
	   }
	  }

	  if (stts == Status.move) { 
	    if (!selected) {
	      ClearReachable();
	      button.ready();
	      button.reset();
	      TranStts(Status.idol);
	    }

	   if (Input.GetMouseButtonDown(0)) {
	      if( CastRayToTile(Input.mousePosition) && reachable.Contains(CastRayToTile(Input.mousePosition)) ) {	
		  leading.unit = null;
		  moveTo( CastRayToTile(Input.mousePosition) );
		  TranStts(Status.standBy);
		  ClearReachable();
		  button.ready();

		  costAction(1);
	      }
	   }

	   if(Input.GetMouseButtonDown(1)) {
	     ClearReachable();
	     button.ready();
	     button.reset();
	     TranStts(Status.standBy);
	   }
	  }

	  if (stts == Status.ability) {
	    if (!selected) {
	      activeAbility = null;
	      ClearReachable();
	      button.ready();
	      button.reset();
	      TranStts(Status.idol);
	    }

	   if (Input.GetMouseButtonDown(0)) {
	     if( CastRayToTile(Input.mousePosition) && reachable.Contains(CastRayToTile(Input.mousePosition))) {
		 Tile targetTile = CastRayToTile(Input.mousePosition);
		 if( activeAbility.cast( targetTile) ) {
		   costAction(activeAbility.cost);
		   button.ready();
		   button.reset();
		   
		   if( !activeAbility.spc ) {
		     activeAbility = null;
		     ClearReachable();
		     TranStts(Status.standBy);
		   } else {
		     TranStts(Status.special);
		   }
		 }


		 /*if( enemy.alive) {
		   new Combat( this, enemy, true, true, 0 );
		   ClearReachable();
		   button.ready();
		   TranStts(Status.standBy);

		   actions[0] = false;
		   setActionText();
		 }*/
	     }
	   }

	   if(Input.GetMouseButtonDown(1))  {
	     activeAbility = null;
	     ClearReachable();
	     button.ready();
	     button.reset();
	     TranStts(Status.standBy);
	   }
	}

	if (stts == Status.special) {
	    if (!selected) {
	      activeAbility = null;
	      ClearReachable();
	      button.ready();
	      button.reset();
	      TranStts(Status.idol);
	    }

	   if (Input.GetMouseButtonDown(0)) {
	     if( CastRayToTile(Input.mousePosition) && reachable.Contains(CastRayToTile(Input.mousePosition))) {
		 Tile targetTile = CastRayToTile(Input.mousePosition);
		 if( activeAbility.special( targetTile )) {
		   ClearReachable();
		   button.ready();
		   button.reset();
		   
		   if( !activeAbility.spc ) {
		     activeAbility = null;
		     TranStts(Status.standBy);
		   } else {
		     TranStts(Status.special);
		   }
		 }
	     }
	   }

	   if(Input.GetMouseButtonDown(1))  {
	     activeAbility = null;
	     ClearReachable();
	     button.ready();
	     button.reset();
	     TranStts(Status.standBy);
	   }
	}


	if(stts == Status.summonSelect) {
	  if (!selected) {
	    ClearReachable();
	    button.ready();
	    button.reset();
	    manager.summonPanel.gameObject.SetActive(false);
	    TranStts(Status.idol);
	  }

	  if( manager.summonPanel.clickedButton >= 0 && manager.buildTable.Count >  manager.summonPanel.clickedButton &&
	      manager.buildTable[manager.summonPanel.clickedButton].GetComponent<PlayerControl>().cost <= manager.getFantasies() ) {
	      PathFind( 3, 2, true, leading );
	      manager.summonPanel.gameObject.SetActive(false);
	      TranStts(Status.summon);
	  }

	  if(Input.GetMouseButtonDown(1)) {
	     ClearReachable();
	     button.ready();
	     button.reset();
	     manager.summonPanel.gameObject.SetActive(false);
	     TranStts(Status.standBy);
	  }
	}
	
	if (stts == Status.summon) {
	  if (!selected) {
	    ClearReachable();
	    button.ready();
	    button.reset();
	    TranStts(Status.idol);
	  }

	  if (Input.GetMouseButtonDown(0)) {
	    if( CastRayToTile(Input.mousePosition) && reachable.Contains(CastRayToTile(Input.mousePosition))) {
	      Tile targetTile = CastRayToTile(Input.mousePosition);
	      if( targetTile.unit == null ) {
		manager.addUnit(targetTile, manager.summonPanel.clickedButton);
		manager.costFantasies( manager.buildTable[manager.summonPanel.clickedButton].GetComponent<PlayerControl>().cost );
		ClearReachable();
		canSummon = false;
		costAction(2);
		button.ready();
		button.reset();
		TranStts(Status.standBy);
	      }
	    }
	  }

	  if(Input.GetMouseButtonDown(1)) {
	     ClearReachable();
	     button.ready();
	     button.reset();
	     manager.summonPanel.reset();
	     TranStts(Status.summonSelect);
	   }
	}


	  if (HP.remain <= 0 && alive == true ) {
	    HP.remain = 0;
	    Debug.Log("YOU DIED!!!");
	    Instantiate( YouDied, transform.position, new Quaternion(0,0,0,0) );
	    GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f);
      	    alive = false;
	    if( isMaster ) {
	      manager.masterFallen();
	    }
	  }

	  if(HP.remain > HP.finalValue) {
	    HP.remain = HP.finalValue;
	  }

	  HPtm.text = "" + HP.remain + "/" + mind.remain;
	}

	Tile CastRayToTile( Vector2 pos ) {
	  Ray ray = Camera.main.ScreenPointToRay(pos);
	  RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity, moveMask);


	  if(hit.transform.gameObject.GetComponent<Tile>()) {
	    return hit.transform.gameObject.GetComponent<Tile>();
	  } else {
	    return null;
	  }
	}

      public void PathFind( int type, int range, bool penetrate, Tile center) {
	  if( type == 0 ) {
	    reachable.Add(center);
	    center.Shade(3);
	    return;
	  }

	  Tile cTile = center;
	  center.dist = 0;
	  center.visited = true;
	  Queue<Tile> que = new Queue<Tile>();
	  que.Enqueue(center);
	  while( que.Count != 0 ) {
	    cTile = que.Peek();
	    for( int i = -1; i <= 1; i++ ) {
	      for( int j = -1; j <= 1; j++ ) {
		if( i!=j && GameObject.Find("GameManager").GetComponent<GameManager>().Bounded( (int)cTile.Q, (int)cTile.R, i, j )) {

		  Tile neighbor = GameObject.Find("GameManager").GetComponent<GameManager>().OnMap((int)cTile.Q+i, (int)cTile.R+j);
		  int cost = 0;

		  //move
		  if( type == 1 ) {
		    if( freeMove == true ) {
		      cost = 1;
		    } else {
		      cost = neighbor.cost;
		    }
		  // target ability
		  } else if( type == 2 ) {
		    if((!penetrate && neighbor.blockProjectile)) {
		      cost = 255;
		    } else {
		      cost = 1;
		    }
		  // ground ability
		  } else if( type == 3 ) {
		    cost = 1;
		  }


		  if( !neighbor.visited && cTile.dist + cost <= range ) {
		    if( type == 1 ) {  
		      if( !neighbor.unit ) {
			reachable.Add(neighbor); 
			neighbor.Shade(1);
			neighbor.dist = cTile.dist + cost;
			que.Enqueue(neighbor);
		      } else if(  neighbor.unit.align != this.align && neighbor.unit.alive == true && !freeMove ) {
			past.Add(neighbor);
		      } else {
			past.Add(neighbor);
			neighbor.Shade(2);
			neighbor.dist = cTile.dist + cost;
			que.Enqueue(neighbor);
		      }
		    } else if( type == 2 ) {
		      if( !neighbor.unit || !neighbor.unit.alive ) {
			past.Add(neighbor); 
			neighbor.Shade(4);
			neighbor.dist = cTile.dist + cost;
			que.Enqueue(neighbor);
		      } else {
		        reachable.Add(neighbor); 
			neighbor.Shade(3);
			neighbor.dist = cTile.dist + cost;
			que.Enqueue(neighbor);
		      }
		    } else if( type == 3 ) {
		     if( !neighbor.unit ) {
			reachable.Add(neighbor); 
			neighbor.Shade(3);
			neighbor.dist = cTile.dist + cost;
			que.Enqueue(neighbor);
		      } else {
		        past.Add(neighbor); 
			neighbor.Shade(4);
			neighbor.dist = cTile.dist + cost;
			que.Enqueue(neighbor);
		      }
		    }

			
		    neighbor.visited = true;
		  }
		}
	      }
	    }
	    que.Dequeue();
	  }
	  center.dist = -1;
	  center.visited = false;
	}

	/*
	void MeleeFind() {
	    for( int i = -1; i <= 1; i++ ) {
	      for( int j = -1; j <= 1; j++ ) {
		if( i!=j && GameObject.Find("GameManager").GetComponent<GameManager>().Bounded( (int)leading.Q, (int)leading.R, i, j )) {
		  Tile neighbor = GameObject.Find("GameManager").GetComponent<GameManager>().OnMap((int)leading.Q+i, (int)leading.R+j);
		  reachable.Add(neighbor);
		  neighbor.Shade(2);
		}
	      }
	    }
	}*/



	void TranStts( Status stts ) {
	  this.stts = stts;
	  Debug.Log(stts);
	}

	public void ClearReachable() {
	  for(int i = 0; i < reachable.Count; i++) {
	    reachable[i].Clear();
	  }
	  for(int i = 0; i < past.Count; i++) {
	    past[i].Clear();
	  }
	  reachable.Clear();
	  past.Clear();
	}

	void initStats( StatTable table ) {
	  HP = new CharacterStat(table.HP);
          mind = new CharacterStat(table.mind);
	  melee = new CharacterStat(table.melee);
	  ATK = new CharacterStat(table.ATK);
	  armor = new CharacterStat(table.armor);
	  magicResist = new CharacterStat(table.magicResist);
	  evasion = new CharacterStat(table.evasion);
	  wind = new CharacterStat(table.wind);
          forest = new CharacterStat(table.forest);
	  fire = new CharacterStat(table.fire);
	  mountain = new CharacterStat(table.mountain);
	  endurance = new CharacterStat(table.endurance);
	  movement  = new CharacterStat(table.movement);
	  
	  freeMove = table.freeMove;
	  cost  = table.cost;

	  abilities = table.abilities;
	  for( int i = 0; i <= 6; i++ ) {
	    if( table.abilities[i] != null ) {
	      abilities[i] =(Ability) Object.Instantiate(table.abilities[i]); //ScriptableObject.CreateInstance(table.abilities[i].GetType());
	      abilities[i].Initialize();
	      abilities[i].owner = this;
	    }
	  }

	  feats = table.feats;
	  for( int i = 0; i <= 6; i++ ) {
	    if( table.feats[i] != null ) {
	      feats[i] = (Feat)Object.Instantiate(table.feats[i]);
	      feats[i].owner = this;
	      feats[i].Initialize();
	    }
	  }

	  burningResist = new CharacterStat(table.burningResist);
	  coldResist = new CharacterStat(table.coldResist);
	  corrosionResist = new CharacterStat(table.corrosionResist);
	  infestionResist = new CharacterStat(table.infestionResist);
          poisonResist = new CharacterStat(table.poisonResist);
	  stopResist = new CharacterStat(table.stopResist);
	  crippleResist = new CharacterStat(table.crippleResist);
	  stunResist = new CharacterStat(table.stunResist);
	  silenceResist = new CharacterStat(table.silenceResist);
	  breakResist = new CharacterStat(table.breakResist);
	  thornResist = new CharacterStat(table.thornResist);
	  blindsightResist = new CharacterStat(table.blindsightResist);
	  rageResist = new CharacterStat(table.rageResist);
	  dazzleResist = new CharacterStat(table.dazzleResist);
	}

	public void win() {
	  Object.Instantiate( youWin, transform.position - new Vector3 (0, -0.5f, 0), new Quaternion(0,0,0,0) );

	}

	public void passTurn() {
	  moved = 0;
	  ACtm.text = "";
	  for( int i = 0; i < abilities.Length; i++ ) {
	    if( abilities[i] != null && abilities[i].cCoolDown != 0 ) {
	      abilities[i].cCoolDown --;
	    }
	  }

	  if( isMaster ) {
	    canSummon = true;
	  }
	}

	public void refillActions() {
	  moved++;
	  ACtm.text = "";
	  for( int i = 0; i < moved; i++ ) {
	    ACtm.text += "动 ";
	  }

	  actions = new bool[] {true, true, true};
	  setActionText();
	}

	public void returnActions() {
	  moved--;
	  ACtm.text = "";
	  for( int i = 0; i < moved; i++ ) {
	    ACtm.text += "动 ";
	  }
	  actions = new bool[] {false, false, false};
	  setActionText();
	}

	void setActionText() {
	  string text = "";
	  if( actions[0] ) {
	    text += "P ";
	  } else {
	    text += "  ";
	  }
	  if( actions[1] ) {
	    text += "M ";
	  } else {
	    text += "  ";
	  }
	  if( actions[2] ) {
	    text += "S";
	  } else {
	    text += "";
	  }

	  ActionText.text = text;
	}

	bool checkActions( int action ) {
	  if ( action < 0 || action > 2 ) {
	    return false;
	  }
	  for( int i = action; i >= 0; i-- ) {
	   if(actions[i] == true) {
	     return true;
	   }
	  }

	  return false;
	}

	void costAction( int action ) {
	  if ( action < 0 || action > 2 ) {
	    return;
	  }

	  for( int i = action; i >= 0; i-- ) {
	   if(actions[i] == true) {
	     actions[i] = false;
	     setActionText();
	     return;
	   }
	  }
	}

	public void addAbility( Ability ability ) {
	  for( int i = 0; i <= 6; i++ ) {
	    if( abilities[i] == null ) {
	      abilities[i] = ability;
	      ability.owner = this;
	    }
	  }
	}

	public void moveTo( Tile targetTile ) {
	  leading.markSelection( false );
	  leading.unit = null;
	  GetComponent<Rigidbody2D>().MovePosition( targetTile.transform.position );
	  leading = targetTile;
	  leading.unit = this;
	  if( selected ) {
	    leading.markSelection( true );
	  }
	}

	public int GetReachableCount() {
	  return reachable.Count;
	}

	public bool sanCheck( float mindMod ) {
	  bool passed = false;
	  mindMod = mind.finalValue * mindMod / HP.finalValue;
	  mindMod += 10;
	  if( Random.Range(0, 100) < endurance.finalValue ) {
	    mindMod /= 2;
	    passed = true;
	  }
	  mindMod = Mathf.Floor( mindMod );
	  mind.lose( mindMod);
	  return passed;
	} 


}
