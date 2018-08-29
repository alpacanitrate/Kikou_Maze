﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StatTable {
    	public float HP;
	public float mind;
	public float melee;
	public float ATK;
	public float armor;
	public float evasion;
	public float wind;
	public float forest;
	public float fire;
	public float mountain;
	public float endurance;
	public float movement;

	public bool freeMove;

	public Ability[] abilities = new Ability[7];
	public Feat[] feats = new Feat[7];

	public float burningResist;
	public float coldResist;
	public float corrosionResist;
	public float infestionResist;
	public float poisonResist;
	public float stopResist;
	public float crippleResist;
	public float stunResist;
	public float silenceResist;
	public float breakResist;
	public float thornResist;
	public float blindsightResist;
	public float rageResist;
	public float dazzleResist;
}
