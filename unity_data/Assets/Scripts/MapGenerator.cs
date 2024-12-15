using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] TextAsset mapText;
    [SerializeField] GameObject[] prefabs;
    float mapSize;
    enum MAP_TYPE
    {
        GROUND, //0
        WALL,   //1
        PLAYER  //2
    }
    MAP_TYPE[,] mapTable;
    void Start()
    {
        _loadMapData();
        _createMap();
    }

    void _loadMapData()
    {
        string[] mapLines = mapText.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        int row = mapLines.Length; //行の数
        int col = mapLines[0].Split(new char[] { ',' }).Length; //列の数
        mapTable = new MAP_TYPE[col,row]; //初期化
        
        //追加　行の数だけループ
        for (int y = 0; y < row; y++)
        {
            //1行をカンマ区切りで分割
            string[] mapValues = mapLines[y].Split(new char[] { ',' });
            //列の数だけループ
            for (int x = 0; x < col; x++)
            {
                //mapValuesのx番目をMAP_TYPEにキャストしてmapTable[x,y]番目に代入
                mapTable[x, y] = (MAP_TYPE)int.Parse(mapValues[x]);
            }
        }
    }


    void _createMap()
    {
         //追加サイズを取得する
        mapSize = prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;

        //mapTableの行のループ
        for(int y = 0;y < mapTable.GetLength(1); y++)
        {
            //MapTableの行のループ
            for(int x = 0;x < mapTable.GetLength(0); x++)
            {
                //現在地
                Vector2Int pos = new Vector2Int(x,y);

                //wayを敷き詰める
                GameObject _ground = Instantiate(prefabs[(int)MAP_TYPE.GROUND]);
                //prefabsの中のmapTable[x,y]に当たるものを生成
                GameObject _map = Instantiate(prefabs[(int)mapTable[x,y]]);

                _ground.transform.position = _screenPos(pos);
                //生成したゲームオブジェクトの位置を設定
                _map.transform.position = _screenPos(pos);
            }
        }
    }


    //現在地取得関数
    Vector2 _screenPos(Vector2Int _pos)
    {
        return new Vector2(_pos.x * mapSize,_pos.y * mapSize);
    }
}

