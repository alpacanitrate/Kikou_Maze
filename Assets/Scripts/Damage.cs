﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage {
  PlayerControl dealer;
  PlayerControl taker;
  int type;
  string source;
  float rawDmg;
  float dmg;

  public Damage( PlayerControl dealer, PlayerControl taker, float rawDmg, int type, string source ) {
    this.taker = taker;
    this.dealer = dealer;
    this.rawDmg = rawDmg;
    this.type = type;
    this.source = source;

    dmg = rawDmg;
    if( type == 1 ) {
      dmg *= 100 / (100 + taker.armor.finalValue);
      dmg = Mathf.Floor(dmg);
    } else if ( type == 2 ) {
      dmg *= (100 - taker.magicResist.finalValue) / 100;
      dmg = Mathf.Floor(dmg);
      if( dmg < 0 ) {
	dmg = 0;
      }
    }
    taker.HP.lose(dmg);
  }

  public float GetDmg() {
    return dmg;
  }
}
