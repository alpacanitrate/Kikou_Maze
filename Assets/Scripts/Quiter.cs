using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiter : MonoBehaviour {
  public void quitToStartMenu() {
    Application.LoadLevel("StartMenu");
  }

  public void quit() {
    Application.Quit();
  }
}
