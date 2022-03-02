using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace PolyPerfect
{
    namespace City
    {
        [Serializable]
        public class Tile : MonoBehaviour
        {
            //Path types that are on this tile. Road includes pathwalks.
            public enum TileType
            {
                RoadAndRail,
                Road,
                Rail,
                OnlyPathwalk
            }
            //Determines how steep tile is and how verticaly is placed.
            public enum VerticalType
            {
                Plane,
                Ramp,
                Tunnel,
                BridgeRamp,
                Bridge
            };
            //Shape of road / rail
            public enum TileShape
            {
                T,
                Cross,
                Straight,
                Turn,
                End,
                Exit,
                ExitOneWay,
                BothSideExit,
                BothSideExitOneWay
            };

            //List of all road/train paths on tile
            [HideInInspector]
            public List<Path> paths = new List<Path>();
            //List of all sidewalk paths on tile
            [HideInInspector]
            public List<Path> sidewalkPaths = new List<Path>();
            public TileShape tileShape;
            public VerticalType verticalType;
            public TileType tileType = TileType.Road;
            
            public Guid Id;
            //List of all walkable/driveable tiles in sceen
            public static List<Tile> tiles = new List<Tile>();
            //Neighbor tiles of this tile
            //[HideInInspector]
            public List<Tile> NeighborTiles = new List<Tile>(4);
            private void Awake()
            {
                //Sets up unique id
                Id = Guid.NewGuid();
                //Gets all paths from childrens of this gameobject
                foreach (Path navPath in gameObject.GetComponentsInChildren<Path>())
                {
                    navPath.TileId = Id;
                    if (navPath.pathType == PathType.Sidewalk)
                    {
                        sidewalkPaths.Add(navPath);
                    }
                    else
                    {
                        paths.Add(navPath);
                    }
                }
                GetNeighborTiles();
                //Fixes paths if tile is mirrored
                if(transform.localScale.z < 0)
                {
                    foreach(Path path in paths)
                    {
                        path.pathPositions.Reverse();
                    }
                    foreach (Path path in sidewalkPaths)
                    {
                        path.pathPositions.Reverse();
                    }
                }
                tiles.Add(this);
            }

            private void Start()
            {
                //Sets up paths that are connected from neighbor tiles for all paths
                foreach (Path path in paths)
                {
                    path.nextPaths = GetNextPaths(path.pathPositions[path.pathPositions.Count-1].position,path.pathType);
                }
                foreach (Path path in sidewalkPaths)
                {
                    path.nextPaths = GetNextPaths(path.pathPositions[path.pathPositions.Count - 1].position, path.pathType);
                }
            }


            //Gets neighbor tiles from sides of current tile 
            public void GetNeighborTiles()
            {
                
                Vector3 topOffset = Vector3.zero;
                Vector3 botttomOffset = Vector3.zero;
                Vector3 rightOffset = Vector3.zero; 
                Vector3 leftOffset = Vector3.zero;
                switch (verticalType)
                {
                    case VerticalType.Plane:
                        break;
                    case VerticalType.Ramp:
                        botttomOffset = new Vector3(0, 6 * transform.lossyScale.y, 0);
                        break;
                    case VerticalType.Tunnel:
                        break;
                    case VerticalType.BridgeRamp:
                        topOffset = new Vector3(0, 6 * transform.lossyScale.y, 0);
                        botttomOffset = new Vector3(0, 12 * transform.lossyScale.y, 0);
                        if (tileShape == TileShape.Turn)
                        {
                            leftOffset = new Vector3(0, 12 * transform.lossyScale.y, 0);
                            botttomOffset = new Vector3(0, 6 * transform.lossyScale.y, 0);
                        }
                        break;
                    case VerticalType.Bridge:

                        topOffset = new Vector3(0, 12 * transform.lossyScale.y, 0);
                        botttomOffset = new Vector3(0, 12 * transform.lossyScale.y, 0);
                        rightOffset = new Vector3(0, 12 * transform.lossyScale.y, 0);
                        leftOffset = new Vector3(0, 12 * transform.lossyScale.y, 0);

                        break;
                }
                Vector3 size = new Vector3(Mathf.Abs(2 * transform.lossyScale.x), Mathf.Abs(2 * transform.lossyScale.y), Mathf.Abs(2 * transform.lossyScale.z));


                Collider[] hitsTop = Physics.OverlapBox(gameObject.transform.position + (transform.forward * 18 * Mathf.Abs(transform.lossyScale.z)) + topOffset, size);
                
                Collider[] hitsBottom = Physics.OverlapBox(gameObject.transform.position + (-transform.forward * 18 * Mathf.Abs(transform.lossyScale.z)) + botttomOffset, size);
                Collider[] hitsRight = Physics.OverlapBox(gameObject.transform.position + (transform.right * 18 * Mathf.Abs(transform.lossyScale.x)) + rightOffset, size);
                Collider[] hitsLeft = Physics.OverlapBox(gameObject.transform.position + (-transform.right * 18 * Mathf.Abs(transform.lossyScale.x)) + leftOffset, size);

                NeighborTiles[Direction.Top] = GetTile(hitsTop);
                if (tileShape == TileShape.T || tileShape == TileShape.Exit || tileShape == TileShape.ExitOneWay)
                {
                    if (transform.localScale.z < 0)
                    {
                        NeighborTiles[Direction.Top] = GetTile(hitsBottom);
                        NeighborTiles[Direction.Left] = GetTile(hitsRight);
                        NeighborTiles[Direction.Right] = GetTile(hitsLeft);
                    }
                    else
                    {
                        NeighborTiles[Direction.Right] = GetTile(hitsRight);
                        NeighborTiles[Direction.Left] = GetTile(hitsLeft);
                    }
                }
                else if (tileShape == TileShape.Turn)
                {
                    if (transform.localScale.z < 0)
                    {
                        NeighborTiles[Direction.Top] = GetTile(hitsBottom);
                    }
                    NeighborTiles[Direction.Left] = GetTile(hitsLeft);
                }
                else if (tileShape == TileShape.Straight)
                {
                    NeighborTiles[Direction.Bottom] = GetTile(hitsBottom);
                }
                else if (tileShape == TileShape.Cross || tileShape == TileShape.BothSideExit || tileShape == TileShape.BothSideExitOneWay)
                {
                    NeighborTiles[Direction.Bottom] = GetTile(hitsBottom);
                    NeighborTiles[Direction.Right] = GetTile(hitsRight);
                    NeighborTiles[Direction.Left] = GetTile(hitsLeft);
                }

            }

            private Tile GetTile(Collider[] colliders)
            {
                Tile tile;
                foreach (Collider collider in colliders)
                {
                    if(collider.gameObject.TryGetComponent<Tile>(out tile))
                    {
                        return tile;
                    }
                }
                return null;
            }
            //Gets all paths from neighbor tiles that starts up to 0.5m from point
            private List<Path> GetNextPaths(Vector3 point, PathType pathType)
            {
                List<Path> paths = new List<Path>();
                foreach(Tile tile in NeighborTiles)
                {
                    if (tile != null)
                    {
                        if (pathType == PathType.Sidewalk)
                        {
                            foreach (Path path in tile.sidewalkPaths)
                            {
                                if (Vector3.Distance(path.pathPositions[0].position, point) < 0.5f * Mathf.Abs(transform.lossyScale.z))
                                {
                                    paths.Add(path);
                                }
                            }
                        }
                        else
                        {
                            foreach (Path path in tile.paths)
                            {
                                if (Vector3.Distance(path.pathPositions[0].position, point) < 0.5f * Mathf.Abs(transform.lossyScale.z))
                                {
                                    paths.Add(path);
                                }
                            }
                        }
                    }
                }
                return paths;
            }
        }
    }
}
