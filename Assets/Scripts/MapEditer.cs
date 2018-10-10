using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class MapEditer : MonoBehaviour {
  public int mapRad;
  private int cRad;
  public List<List<Tile>> map;
  public UnityEngine.UI.Slider radSlider;
  public UnityEngine.UI.Slider tileSlider;
  public UnityEngine.UI.Image tileImage;
  public UnityEngine.UI.InputField nameInput;
  public float tileSize;
  public int cPre;

  private int tileMask;
  private UnityEngine.Object[] tilePres;
  
  void Start() {
    cRad = -1;
    tilePres = Resources.LoadAll("Tiles");
    tileImage.sprite = ((GameObject)tilePres[0]).GetComponent<SpriteRenderer>().sprite;
    tileSlider.maxValue = tilePres.Length-1;
    tileMask = 1<<1;
  }
  
  void Update() {
    if( Input.GetMouseButton(0) ) {
      if( CastRayToTile(Input.mousePosition) ) {
	Tile tile = CastRayToTile(Input.mousePosition);
	changeTile( tile.Q, tile.R, cPre );
      }
    }
  }


  public void updateRad() {
    mapRad = (int)radSlider.value;
  }

  public void updateTile() {
    cPre = (int)tileSlider.value;
    tileImage.sprite = ((GameObject)tilePres[cPre]).GetComponent<SpriteRenderer>().sprite;
  }

  void clearMap() {
    for( int i = 0; i < map.Count; i++ ) {
      for( int j = 0; j < map[i].Count; j++ ) {
	Destroy( map[i][j].gameObject );
      }
    }
  }

  public void emptyMapGen( ) {
    cRad = mapRad;
    if( map != null ) {
      clearMap();
    }

    map = new List<List<Tile>>();
    for( int i = -mapRad; i <= mapRad; i++ ) {
      List<Tile> row = new List<Tile>();
      for( int j = -mapRad; j <= mapRad; j++ ) {
	row.Add(TileGen(i, j, 0));
      }
      map.Add(row);
    }
  }

  public void saveMap() {
    string path =  Application.dataPath + "/Maps/CustomMaps/";
    if( nameInput.text != "" ){
      path += nameInput.text + ".txt";
    } else {
      path += "新建地图1.txt";
    }

    if( !Directory.Exists(Application.dataPath + "/Maps/CustomMaps/")) {
      Directory.CreateDirectory(Path.GetDirectoryName(Application.dataPath + "/Maps/CustomMaps/"));
    }
    StreamWriter sw = new StreamWriter(path, false);
    sw.WriteLine( "" + cRad );

    if( map != null ) {
      for( int i = 0; i < map.Count; i++ ) {
	for( int j = 0; j < map[i].Count; j++ ) {
	  sw.Write("" +  map[i][j].tilePre + " "); 
	}
	sw.WriteLine("");
      }
    }    

    sw.Close();
  }

  public void loadMap() {
    string path =  Application.dataPath + "/Maps/CustomMaps/";
    if( nameInput.text != "" ){
      path += nameInput.text + ".txt";
    } else {
      path += "新建地图1.txt";
    }

    StreamReader sr = new StreamReader(path);
    string radString = sr.ReadLine();
    
    int loadRad = -1;
    if( radString == "" ) {
      return;
    } else {
      Int32.TryParse(radString, out loadRad);
    }
    
    if( loadRad == -1 ) {
      return;
    }

    if( map != null ) {
      clearMap();
    }

    map = new List<List<Tile>>();
    for( int i = -loadRad; i <= loadRad; i++ ) {
      string[] rowStrs = sr.ReadLine().Split(' ');
      List<Tile> row = new List<Tile>();
      for( int j = -loadRad; j <= loadRad; j++ ) {
	int pre = 0;
	Int32.TryParse(rowStrs[j + loadRad], out pre);
	row.Add(TileGen(i, j, pre));
      }
      map.Add(row);
    }
    mapRad = loadRad;
    radSlider.value = loadRad;
    sr.Close();
  }    





  // Generate Tile
  Tile TileGen (float q, float r, int tilePre) {
    Tile tile = ((GameObject) Instantiate( tilePres[tilePre], new Vector3((q+r/2)*Mathf.Sqrt(3)*tileSize, (float)3/(float)2*r*tileSize, 0),
      new Quaternion(0,0,0,0))).GetComponent<Tile>();
    tile.Q = q;
    tile.R = r;
    tile.tilePre = tilePre;
    return tile;
  }

	Tile CastRayToTile( Vector2 pos ) {
	  Ray ray = Camera.main.ScreenPointToRay(pos);
	  RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity, tileMask);


	  if(hit.transform.gameObject.GetComponent<Tile>()) {
	    return hit.transform.gameObject.GetComponent<Tile>();
	  } else {
	    return null;
	  }
	}

  void changeTile( float q, float r, int tilePre ) {
    Tile tile = ((GameObject) Instantiate( tilePres[tilePre], new Vector3((q+r/2)*Mathf.Sqrt(3)*tileSize, (float)3/(float)2*r*tileSize, 0),
    new Quaternion(0,0,0,0))).GetComponent<Tile>();
    tile.Q = q;
    tile.R = r;
    tile.tilePre = tilePre;
    Destroy(map[(int)q + mapRad ][(int)r + mapRad].gameObject);
    map[(int)q + mapRad][(int)r + mapRad] = tile;
  }
	  
}
