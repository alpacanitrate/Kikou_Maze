using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/AttackAbility")]
public class AttackAbility : Ability {


    public override void Initialize () {
      cost = 0;
      coolDown = 0;
      silencable = false;
      castType = 2;
      range = 1;
      combat = true;
      penetrate = true;
      combatModifier = 0;
    }
    public override bool cast( Tile targetTile ) {
      if( targetTile.unit == null ) {
	return false;
      }

      
      PlayerControl enemy = targetTile.unit;
      if( enemy.alive) {
        new Combat( owner, enemy, true, true, 0 );
	return true;
      } else {
	return false;
      }
    }
}
