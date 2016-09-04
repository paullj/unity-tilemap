using UnityEngine;
using System.Collections;
using toinfiniityandbeyond.Tilemapping;

public class RandomWalk : MonoBehaviour
{
    [SerializeField]
    private int maxSize = 60;
    [SerializeField]
    private int steps = 20;
    [SerializeField]
    private ScriptableTile floor, wall;

    private TileMap tileMap;

    private void Start()
    {
        tileMap = GetComponent<TileMap>();
        StartCoroutine("Generate");
    }
    private IEnumerator Generate()
    {
        int width = 50;//andom.Range(10, maxSize);
        int height = 50;//Random.Range(10, maxSize);
        tileMap.Resize(width, height);
        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                tileMap.SetTileAt(x, y, wall);
            }
        }
        Point midPoint = new Point(width / 2, height / 2);
        Point currentPoint = midPoint;
        for (int i = 0; i < steps; i++)
        {
			Point lastPoint = currentPoint;
            tileMap.SetTileAt(currentPoint, floor);
            while (lastPoint == currentPoint || !tileMap.IsInBounds(currentPoint))
            {
            	int randomIndex = Random.Range(0, 4);
                switch (randomIndex)
                {
                    case 0:
                        currentPoint = currentPoint.Left;
                        break;
                    case 1:
                        currentPoint = currentPoint.Up;
                        break;
                    case 2:
                        currentPoint = currentPoint.Right;
                        break;
                    case 3:
                        currentPoint = currentPoint.Down;
                        break;
                    default:
                        Debug.Log("uh oh!");
                        break;
                }
            }
            yield return null;
        }
    }
}
