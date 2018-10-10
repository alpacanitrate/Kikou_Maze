﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/SubspaceHole")]
public class SubspaceHole : Ability {
    public override void Initialize () {
      /*cost = 2;
      coolDown = 4;
      silencable = true;
      castType = 1;
      range = 2;
      combat = false;
      penetrate = true;
      combatModifier = 0;
      spc = false;*/
    }

    public override bool cast( Tile targetTile ) {
      owner.moveTo( targetTile );
      cCoolDown = coolDown;
      return true;
    }
    
    public override bool special( Tile targetTile ) {
      return false;
    }
}