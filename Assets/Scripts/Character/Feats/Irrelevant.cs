using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Feats/Irrelevant")]
public class Irrelevant : Feat {
  public override void Initialize() {
    breakable = false;
    enable();
  }

  public override void enable() {
    owner.mind.locked = true;
    enabled = true;
  }

  public override void disable() {
    owner.mind.locked = false;
    enabled = false;
  }
}
