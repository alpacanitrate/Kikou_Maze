﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feat : ScriptableObject {

  public string name = "New Feat";
  public PlayerControl owner;

  public bool breakable;
  public bool enabled;


  public abstract void Initialize();
  public abstract void enable();
  public abstract void disable();
}