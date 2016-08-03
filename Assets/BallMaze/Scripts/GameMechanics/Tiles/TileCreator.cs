
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

        internal static TileModel CreateTile(TileData tileData, Vector3 position)
        {
            GameObject tile;
            TileModel tileModel = null;
            switch (tileData.ObjectiveType)
            {
                case ObjectiveType.NONE:
                    tile = (GameObject)Object.Instantiate(TilePrefab, position, TilePrefab.transform.localRotation);
                    break;
                case ObjectiveType.OBJECTIVE1:
                    tile = (GameObject)Object.Instantiate(Objective1TilePrefab, position, Objective1TilePrefab.transform.localRotation);
                    break;
                case ObjectiveType.OBJECTIVE2:
                    tile = (GameObject)Object.Instantiate(Objective2TilePrefab, position, Objective2TilePrefab.transform.localRotation);
                    break;
                default:
                    throw new UnhandledSwitchCaseException(tileData.ObjectiveType);
            }
            switch (tileData.TileType)
            {
                case TileType.NORMAL:
                    tileModel = tile.AddComponent<TileModel>();
                    break;
                case TileType.SYNCED:
                    tileModel = tile.AddComponent<SyncedTileModel>();
                    tile.tag = Tags.ObjectiveTile;
                    break;
                default:
                    throw new UnhandledSwitchCaseException(tileData.TileType);
            }
            tileModel.Init(tileData);

            return tile.GetComponent<TileModel>();
        }

    }
}
