﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat {

  PlayerControl attacker;
  PlayerControl defender;
  bool atkHit;
  bool defHit;

  // 0 for melee, 1 for ranged
  int type;

  public Combat( PlayerControl Attacker, PlayerControl Defender, bool AtkHit,
      bool DefHit, int Type ) {
    attacker = Attacker;
    defender = Defender;
    atkHit = AtkHit;
    defHit = DefHit;
    type = Type;

    combatCalc();
  }

  void combatCalc() {
    float atkBonus = attacker.melee.finalValue;
    float defBonus = defender.melee.finalValue;
    float atkOdd = Random.Range(0, 100 );
    float defOdd = Random.Range(0, 100 );

    if( attacker.align == defender.align ) {
      attacker.win();
      hit( defender, attacker.ATK.finalValue * (100/(100+defender.armor.finalValue) ));
    } else {

      if( atkBonus + atkOdd  < defBonus + defOdd) {
	defender.win();
      } else if (atkBonus + atkOdd > defBonus + defOdd) {
	attacker.win();
      }

       if( defHit && 
  	 atkBonus + atkOdd + attacker.evasion.finalValue < defBonus + defOdd) {
         hit( attacker, defender.ATK.finalValue * (100/(100+attacker.armor.finalValue) ));
       }

      if( atkHit && 
  	 atkBonus + atkOdd > defBonus + defOdd) {
         hit( defender, attacker.ATK.finalValue * (100/(100+defender.armor.finalValue) ));
       }
    }
  }

  void hit( PlayerControl target, float rawDamage ) {
     float damage = Mathf.Floor(rawDamage);
     target.HP.lose(damage);
     //Instantiate( Minu2, enemy.transform.position, new Quaternion(0,0,0,0) );
     TextMesh tm = Object.Instantiate( target.DmgText, target.transform.position - new Vector3 (0.45f + Random.Range(-0.2f, 0.2f), -0.4f, 0), new Quaternion(0,0,0,0) );
     tm.text = "" + damage;
  }
}
