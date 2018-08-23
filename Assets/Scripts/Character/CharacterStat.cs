﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStat{
  public float initValue;
  private List<StatModifier> statModifiers;
  [HideInInspector]public float baseValue;
  [HideInInspector]public float addValue;
  [HideInInspector]public float finalValue;
  [HideInInspector]public float remain;

  public CharacterStat() {
    statModifiers = new List<StatModifier>();
  }
  public CharacterStat (float initValue) : this() {
    this.initValue=initValue;
    finalValue = initValue;
    remain = finalValue;
  }

  public void lose(float lost) {
    remain -= lost;
  }

  public void regen(float got) {
    remain += got;
  }

  public void addStatModifier( StatModifier mod) {
    statModifiers.Add(mod);
    calcValue();
    statModifiers.Sort();
  }

  private int ComparaModifier( StatModifier a, StatModifier b ) {
    if( a.type < b.type ) {
      return -1;
    } else if ( a.type > b.type ) {
      return 1;
    }
    return 0;
  }

  public bool removeModifier( StatModifier mod) {
    bool removal = ( statModifiers.Remove(mod) );
    if( removal ) {
      calcValue();
    }
    return removal;
  }

  public bool RemoveFromeSource( object source ) {
    bool removal = false;
    for( int i = statModifiers.Count - 1; i >= 0; i-- ) {
      if( statModifiers[i].source == source ) {
	statModifiers.RemoveAt(i);
	removal = true;
      }
    }
    if( removal ) {
      calcValue();
    }
    return removal;
  }

  public void calcValue() {
    baseValue = initValue;
    addValue = baseValue;
    float addPercent = 1;
    float oldFinal = finalValue;

    for( int i = 0; i < statModifiers.Count; i++ ) {
      StatModifier mod = statModifiers[i];
      if( mod.type == 0 ) {
	baseValue += mod.statValue;
      } else if ( mod.type == 1 ) {
	baseValue *= 1 + mod.statValue;
      } else if ( mod.type == 2 ) {
	addPercent += mod.statValue;
	addValue = baseValue * addPercent;
      } else if ( mod.type == 3 ) {
	addValue += mod.statValue;
      }
    }

    baseValue = (float)Math.Round(baseValue, 4);
    addValue = (float)Math.Round(addValue, 4);
    finalValue = baseValue + addValue;
    remain =(float)Math.Round( remain * finalValue / oldFinal );
  }  
}