using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/AttackAbility")]
public class AttackAbility : Ability {

    bool won;
    PlayerControl enemy;
    public override void Initialize () {
      /*cost = 0;
      coolDown = 0;
      silencable = false;
      castType = 2;
      range = 1;
      combat = true;
      penetrate = true;
      combatModifier = 0;*/
    }
    public override bool cast( Tile targetTile ) {
      spc = false;
      if( targetTile.unit == null ) {
	return false;
      }

      
      enemy = targetTile.unit;
      if( enemy.alive) {
        Combat cbt = new Combat( owner, enemy, true, true, 0 );
	owner.ClearReachable();
	if( cbt.winner == owner && enemy.alive && !enemy.leading.standGround ) {
	  owner.PathFind( 1, 1, false, enemy.leading );
	  if( owner.GetReachableCount() == 0 ) {
	    owner.ClearReachable();
	    new Damage( owner, enemy, owner.ATK.finalValue * 0.25f, 1, "" );
	  } else {
	    spc = true;
	  }

	}

	return true;
      } else {
	return false;
      }
    }

    public override bool special( Tile targetTile ) {
      enemy.moveTo( targetTile );
      spc = false;
      return true;
    }
}
