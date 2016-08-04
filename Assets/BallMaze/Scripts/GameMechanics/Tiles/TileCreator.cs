using BallMaze.Exceptions;
using UnityEngine;

namespace BallMaze.GameMechanics.Tiles
{
    class TileCreator
    {
        private static GameObject _TilePrefab;
        private static GameObject TilePrefab
        {
            get
            {
                if (_TilePrefab == null)
                {
                    _TilePrefab = Resources.Load<GameObject>(Paths.TILE);
                }
                return _TilePrefab;
            }
        }

        private static GameObject _Objective1TilePrefab;
        private static GameObject Objective1TilePrefab
        {
            get
            {
                if (_Objective1TilePrefab == null)
                {
                    _Objective1TilePrefab = Resources.Load<GameObject>(Paths.OBJECTIVE1_TILE);
                }
                return _Objective1TilePrefab;
            }
        }

        private static GameObject _Objective2TilePrefab;
        private static GameObject Objective2TilePrefab
        {
            get
            {
                if (_Objective2TilePrefab == null)
                {
                    _Objective2TilePrefab = Resources.Load<GameObject>(Paths.OBJECTIVE2_TILE);
                }
                return _Objective2TilePrefab;
            }
        }

        internal static TileController CreateTile(TileData tileData, Vector3 position, float sizeRatio)
        {
            GameObject mesh;
            GameObject tile = new GameObject();
            tile.transform.localPosition = position;
            TileController tileController = null;
            switch (tileData.ObjectiveType)
            {
                case ObjectiveType.NONE:
                    mesh = (GameObject)Object.Instantiate(TilePrefab);
                    break;
                case ObjectiveType.OBJECTIVE1:
                    mesh = (GameObject)Object.Instantiate(Objective1TilePrefab);
                    break;
                case ObjectiveType.OBJECTIVE2:
                    mesh = (GameObject)Object.Instantiate(Objective2TilePrefab);
                    break;
                default:
                    throw new UnhandledSwitchCaseException(tileData.ObjectiveType);
            }
            switch (tileData.TileType)
            {
                case TileType.NORMAL:
                    tileController = tile.AddComponent<TileController>();
                    break;
                case TileType.SYNCED:
                    tileController = tile.AddComponent<SyncedTileModel>();
                    tile.tag = Tags.SyncedTile;
                    break;
                default:
                    throw new UnhandledSwitchCaseException(tileData.TileType);
            }
            tileController.Init(tileData);
            mesh.transform.localScale *= sizeRatio;
            tileController.SetMesh(mesh);
            return tileController;
        }

    }
}

