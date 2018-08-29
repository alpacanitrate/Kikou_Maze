using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/test/SimpleVitality")]
public class SimpleVitality : Ability {


    public override void Initialize () {
      cost = 1;
      coolDown = 2;
      silencable = true;
      castType = 0;
      range = 2;
      combat = false;
      penetrate = true;
      combatModifier = 0;
    }
    public override bool cast( Tile targetTile ) {
        if( cCoolDown > 0 ) {
	  TextMesh cdtm = Object.Instantiate( owner.DmgText, owner.transform.position - new Vector3 (0.8f, -1.2f, 0), new Quaternion(0,0,0,0) );
	  cdtm.color = new Color(1, 1, 1);
	  cdtm.text = "冷却中! 还有" + cCoolDown + "回合!";
	  return false;
	}

	owner.HP.regen(500);
	TextMesh tm = Object.Instantiate( owner.DmgText, owner.transform.position - new Vector3 (0.45f + Random.Range(-0.2f, 0.2f), -0.3f, 0), new Quaternion(0,0,0,0) );
	tm.text = "500";
	tm.color = new Color( 0.7f, 1, 0.7f );

	cCoolDown = coolDown;
	return true;
    }
}
