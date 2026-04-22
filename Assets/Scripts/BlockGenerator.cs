using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public GameObject normalBlockPrefab;
    public GameObject hardBlockPrefab;
    public int stage;

    // ★追加：高さ調整用
    public float heightOffset = 3f;

    int[,] map;

    void Start()
    {
        map = GetMap(stage);

        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        float blockSizeX = normalBlockPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float blockSizeY = normalBlockPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        float offsetX = (cols - 1) * blockSizeX / 2f;
        float offsetY = (rows - 1) * blockSizeY / 2f;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector2 pos = new Vector2(
                    x * blockSizeX - offsetX,
                    offsetY - y * blockSizeY + heightOffset // ★ここ変更
                );

                if (map[y, x] == 1)
                {
                    Instantiate(normalBlockPrefab, pos, Quaternion.identity);
                }
                else if (map[y, x] == 2)
                {
                    Instantiate(hardBlockPrefab, pos, Quaternion.identity);
                }
            }
        }
    }

    int[,] GetMap(int stage)
    {
        if (stage == 1)
        {
            return new int[,]
            {
                {1,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,1},
                {1,1,1,1,1,1,1,1},
                {0,1,0,0,0,0,1,0},
                {1,1,1,1,1,1,1,1}
            };
        }
        else if (stage == 2)
        {
            return new int[,]
            {
                {2,1,2,1,2,1,2,1},
                {1,2,1,2,1,2,1,2},
                {2,2,2,2,2,2,2,2},
                {1,0,1,0,1,0,1,0},
                {2,1,2,1,2,1,2,1}
            };
        }
        else if (stage == 3)
        {
            return new int[,]
            {
                {2,2,2,2,2,2,2,2},
                {2,0,0,0,0,0,0,2},
                {2,0,0,0,0,0,0,2},
                {2,0,0,0,0,0,0,2},
                {2,0,0,0,0,0,0,2},
                {2,2,2,2,2,2,2,2}
            };
        }
        else if (stage == 4)
        {
            return new int[,]
            {
                {0,0,0,2,2,0,0,0},
                {0,0,0,2,2,0,0,0},
                {0,0,0,1,1,0,0,0},
                {0,0,0,1,1,0,0,0},
                {0,0,0,2,2,0,0,0},
                {0,0,0,2,2,0,0,0}
            };
        }

    else if (stage == 5)
        {
            return new int[,]
            {
                {1,1,1,0,0,1,1,1},
                {1,2,1,0,0,1,2,1},
                {1,1,1,0,0,1,1,1},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {1,1,1,0,0,1,1,1},
                {1,2,1,0,0,1,2,1},
                {1,1,1,0,0,1,1,1}
            };
        }
        else
        {
            return new int[,]
            {
                {1,1,1},
                {1,1,1}
            };
        }
    }
}