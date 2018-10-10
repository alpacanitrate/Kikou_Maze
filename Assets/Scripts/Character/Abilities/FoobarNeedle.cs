using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/FoobarNeedle")]
public class FoobarNeedle : Ability {


    public override void Initialize () {
      /*cost = 0;
      coolDown = 0;
      silencable = false;
      castType = 2;
      range = 2;
      combat = true;
      penetrate = false;
      combatModifier = 0;*/
    }
    public override bool cast( Tile targetTile ) {
      if( targetTile.unit == null ) {
	return false;
      }

      
      PlayerControl enemy = targetTile.unit;
      if( enemy.alive) {
        new Combat( owner, enemy, true, false, 0 );
	return true;
      } else {
	return false;
      }
    }
    public override bool special( Tile targetTile ) {
      return false;
    }
}
