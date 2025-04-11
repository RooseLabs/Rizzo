using UnityEngine;
using UnityEngine.Tilemaps;

namespace RooseLabs.Template
{
    public class CheckerboardTilemap : MonoBehaviour
    {
        [SerializeField] private TileBase tileA;
        [SerializeField] private TileBase tileB;
        [SerializeField] private BoundsInt area;

        private void Start()
        {
            //if (!Application.isEditor) return;
            TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
            for (int index = 0; index < tileArray.Length; ++index)
            {
                tileArray[index] = index % 2 == 0 ? tileA : tileB;
            }
            Tilemap tilemap = GetComponent<Tilemap>();
            tilemap.SetTilesBlock(area, tileArray);
        }
    }
}
