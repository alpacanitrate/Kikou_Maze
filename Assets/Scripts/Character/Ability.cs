using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

  public string name = "New Ability";

  public int cost;
  public int coolDown;
  public bool silencable;
  public int castType;
  public int range;
  public bool combat;
  public bool penetrate;
  public float combatModifier;
  public PlayerControl owner;

  public abstract void Initialize();
  public abstract bool cast( Tile targetTile );
}
