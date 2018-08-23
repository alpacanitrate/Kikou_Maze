using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier{
  public float statValue;
  public int type;
  public object source;

  public StatModifier( float Value, int Type, object Source ) {
    statValue = Value;
    type = Type;
    source = Source;
  }
}
