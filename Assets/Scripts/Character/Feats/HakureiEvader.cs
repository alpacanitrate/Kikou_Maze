using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Feats/HakureiEvader")]
public class HakureiEvader : Feat {


  public override void Initialize() {
    breakable = true;
    enable();
  }

  public override void enable() {
    owner.evasion.addStatModifier(new StatModifier(20, 0, this));
    enabled = true;
  }

  public override void disable() {
    owner.evasion.RemoveFromSource(this);
    enabled = false;
  }

}
