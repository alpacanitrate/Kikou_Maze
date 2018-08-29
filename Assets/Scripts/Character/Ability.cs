﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {


  public int cost;
  public bool silencable;
  public int castType;
  public int range;
  public bool combat;
  public bool penetrate;
  public float combatModifier;
  public PlayerControl owner;
  public int coolDown;
  public TextMesh CDText;
  [HideInInspector]public int cCoolDown;

  public abstract void Initialize();
  public abstract bool cast( Tile targetTile );
}
