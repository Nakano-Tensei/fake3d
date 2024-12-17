using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] TextAsset mapText;
    [SerializeField] GameObject[] prefabs;
    float mapSize;
    Vector2 centerPos;
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

        if(mapTable.GetLength(0) % 2 == 0)
        {
            centerPos.x = mapTable.GetLength(0) / 2 * mapSize - (mapSize / 2);
        }
        else
        {
            centerPos.x = mapTable.GetLength(0) / 2 * mapSize;
        }

        if(mapTable.GetLength(1) % 2 == 0)
        {
            centerPos.y = mapTable.GetLength(1) / 2 * mapSize - ( mapSize / 2);
        }
        else
        {
            centerPos.y = mapTable.GetLength(1) / 2 * mapSize;
        }
        //mapTableの行のループ
        for(int y = 0;y < mapTable.GetLength(1); y++)
        {
            //MapTableの行のループ
            for(int x = 0;x < mapTable.GetLength(0); x++)
            {
                //現在地
                Vector2Int pos = new Vector2Int(x,y);

                //wayを敷き詰める
                GameObject _ground = Instantiate(prefabs[(int)MAP_TYPE.GROUND],transform);
                //prefabsの中のmapTable[x,y]に当たるものを生成
                GameObject _map = Instantiate(prefabs[(int)mapTable[x,y]],transform);

                _ground.transform.position = ScreenPos(pos);
                //生成したゲームオブジェクトの位置を設定
                _map.transform.position = ScreenPos(pos);
                if(mapTable[x,y] == MAP_TYPE.PLAYER)
                {
                    _map.GetComponent<Player>().currentPos = pos;

                }
            }
        }
    }


    //現在地取得関数
    public Vector2 ScreenPos(Vector2Int _pos)
    {
        return new Vector2( _pos.x * mapSize - centerPos.x,
                            -(_pos.y * mapSize - centerPos.y));
    }
}

