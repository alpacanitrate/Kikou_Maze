using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Feats/ExorcismSpecialist")]
public class ExorcismSpecialist : Feat {

  public override void Initialize() {
    breakable = true;
    enable();
  }

  public override void enable() {
    owner.magicResist.addStatModifier(new StatModifier(30, 5, this));
    enabled = true;
  }

  public override void disable() {
    owner.evasion.RemoveFromSource(this);
    enabled = false;
  }
}
