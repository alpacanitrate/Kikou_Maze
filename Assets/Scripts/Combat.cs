using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat {

  PlayerControl attacker;
  PlayerControl defender;
  private bool atkHit;
  private bool defHit;
  private float atkBonus;
  private float defBonus;
  private float atkOdd;
  private float defOdd;
  public PlayerControl winner;

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
    atkBonus = attacker.melee.finalValue;
    defBonus = defender.melee.finalValue;
    atkOdd = Random.Range(0, 100 );
    defOdd = Random.Range(0, 100 );

    if( attacker.align == defender.align ) {
      attacker.win();
      winner = attacker;
      hit( attacker, defender );
    } else {

      if( atkBonus + atkOdd  < defBonus + defOdd) {
	defender.win();
	winner = defender;
	if( hitCheck(false)) {
	  //hit( attacker, defender.ATK.finalValue * (100/(100+attacker.armor.finalValue) ));
	  hit( defender, attacker );
	}

      } else if (atkBonus + atkOdd > defBonus + defOdd) {
	attacker.win();
	winner = attacker;
	if( hitCheck(true)) {
	  //hit( defender, attacker.ATK.finalValue * (100/(100+defender.armor.finalValue) ));
	  hit( attacker, defender );
	}
      }
    }
  }

  bool hitCheck( bool attackerIsHitter ) {
    if( !attackerIsHitter ) {
      if( defHit && 
	 atkBonus + atkOdd + attacker.evasion.finalValue < defBonus + defOdd && attacker.leading.cover <= Random.Range(0,100) ) {
	return true;
      } else {
	if( defHit ) {
	  TextMesh tm = Object.Instantiate( attacker.DmgText, attacker.transform.position - new Vector3 (0.45f + Random.Range(-0.2f, 0.2f), -0.4f, 0), new Quaternion(0,0,0,0) );
	  tm.text = "Miss!!";
	  tm.color = new Color( 0.7f, 0.7f, 0.7f );
	}
	return false;
      }
    } else {
      if( atkHit && 
	 defBonus + defOdd + defender.evasion.finalValue < atkBonus + atkOdd && defender.leading.cover <= Random.Range(0,100) ) {
	return true;
      } else {
	if( atkHit ) {
	  TextMesh tm = Object.Instantiate( defender.DmgText, defender.transform.position - new Vector3 (0.45f + Random.Range(-0.2f, 0.2f), -0.4f, 0), new Quaternion(0,0,0,0) );
	  tm.text = "Miss";
	  tm.color = new Color( 0.7f, 0.7f, 0.7f );
	}
	return false;
      } 
    }     
  }
  void hit( PlayerControl hitter, PlayerControl hittee ) {
     Damage dmg = new Damage( hitter, hittee, hitter.ATK.finalValue, 1, "" );
     hittee.sanCheck( dmg.GetDmg() );
     //Instantiate( Minu2, enemy.transform.position, new Quaternion(0,0,0,0) );
     TextMesh tm = Object.Instantiate( hittee.DmgText, hittee.transform.position - new Vector3 (0.45f + Random.Range(-0.2f, 0.2f), -0.4f, 0), new Quaternion(0,0,0,0) );
     tm.text = "" + dmg.GetDmg();
  }
}
