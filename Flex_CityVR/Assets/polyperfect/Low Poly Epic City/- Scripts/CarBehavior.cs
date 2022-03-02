using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyPerfect.City
{
    [RequireComponent(typeof(PathFinding)),RequireComponent(typeof(Rigidbody))]
    public class CarBehavior : MonoBehaviour
    {
        //[HideInInspector]
        public List<Path> trajectory = new List<Path>();
        public bool randomDestination = true;
        public bool closedCircuit = false;
        private PathFinding pathFinding;
        public float minDistance = 90f;
        public float maxspeed = 5.0f;
        public float acceleration = 0.5f;
        private const float KMHTOMS = 0.27777f;
        private float currentMaxSpeed;
        [HideInInspector]
        public float speed;
        int activepoint = 0;
        bool isMoving = false;
        bool drivingBihindCar = false;
        bool drivingTrafficLights = false;
        int activePath = 0;
        int randomPathTries = 10;
        private Vector3 targetDrivePoint;
        private Vector3 destination;
        public List<Vector3> checkpoints = new List<Vector3>();
        private Vector3 start;
        private CarBehavior carInFront;
        private void Awake()
        {
            pathFinding = GetComponent<PathFinding>();
        }

        void Start()
        {
            currentMaxSpeed = maxspeed;
            if(closedCircuit)
                checkpoints.Add(checkpoints[0]);
            StartDriving();
        }
        //Gets next target point of the trajectory
        public void MoveToNextPoint()
        {
            if (activePath == trajectory.Count - 1)
            {
                if (activepoint == trajectory[activePath].pathPositions.Count - 1)
                {
                    isMoving = false;
                    if(randomDestination)
                        StartDriving();
                    else
                    {
                        if(!closedCircuit)
                            checkpoints.Reverse();
                        trajectory = pathFinding.GetPathWithCheckpoints(checkpoints, PathType.Road);
                        if (trajectory != null)
                        {
                            trajectory.RemoveAt(trajectory.Count - 1);
                            activePath = 0;
                            activepoint = 0;
                            speed = 0;
                            isMoving = true;
                        }
                        else
                            return;
                    }
                }
                else
                {
                    activepoint++;
                }
            }
            else
            {
                if (activepoint == trajectory[activePath].pathPositions.Count - 1)
                {
                    activePath++;
                    if (trajectory[activePath].speed < maxspeed)
                    {
                        currentMaxSpeed = trajectory[activePath].speed;
                    }
                    else
                    {
                        currentMaxSpeed = maxspeed;
                    }
                       
                    activepoint = 1;
                }
                else
                {
                    activepoint++;
                }
            }
            if (trajectory != null)
            {
                if (trajectory.Count > activePath)
                    targetDrivePoint = trajectory[activePath].pathPositions[activepoint].transform.position;
            }

        }

        private void StartDriving()
        {
            if (randomDestination)
            {
                //Selects random tile which is at least minDistance away 
                start = transform.position;
                destination = start;
                int tries = 0;
                randomPathTries--;
                while (Vector3.Distance(start, destination) < minDistance && tries < Tile.tiles.Count)
                {
                    tries++;
                    Tile t = Tile.tiles[UnityEngine.Random.Range(0, Tile.tiles.Count - 1)];
                    if (t.tileType == Tile.TileType.Road || t.tileType == Tile.TileType.RoadAndRail)
                    {
                        if (t.verticalType == Tile.VerticalType.Bridge)
                        {
                            destination = t.transform.position + (Vector3.up * 12);
                        }
                        else
                        {
                            destination = t.transform.position;
                        }
                    }
                }
                if(tries == Tile.tiles.Count)
                {
                    Debug.Log(name + ": Path not found");
                    return;
                }
            }
            else
            {
                //Destination of the first checkpoint
                destination = checkpoints[0];
                start = transform.position;
            }
            //Pathfinder finds best path
            trajectory = pathFinding.GetPath(start,destination, PathType.Road);
            if (trajectory != null)
            {
                speed = 0;
                activePath = 0;
                activepoint = 0;
                float closest = float.MaxValue;
                for(int i = 0;i <trajectory[0].pathPositions.Count;i++)
                {
                    float tmp = Vector3.Distance(trajectory[0].pathPositions[i].position, transform.position);
                    if(tmp < closest)
                    {
                        closest = tmp;
                        activepoint = i;
                    }
                }
                targetDrivePoint = trajectory[0].pathPositions[activepoint].transform.position;
                isMoving = true;
                if(!closedCircuit)
                    checkpoints.Reverse();
            }
            else
            {
                Debug.Log(name + ": Path not found");
                if(randomDestination && randomPathTries>0)
                {
                    StartDriving();
                }
            }
        }


        void FixedUpdate()
        {
            if (isMoving && trajectory != null)
            {
                //Calculate remaing distance to current checkpoint and direction to it
                float pointDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetDrivePoint.x, targetDrivePoint.z));
                Vector3 direction = targetDrivePoint - transform.position;

                //If car reaches target it gets the next target
                if (pointDistance < 0.02f*speed)
                {
                    MoveToNextPoint();
                }
                if (!drivingBihindCar)
                {
                    speed = Mathf.Lerp(speed, maxspeed, acceleration * Time.deltaTime);
                }
                else
                {
                    if(carInFront.speed < speed)
                        speed = speed - carInFront.speed;
                }
                if (speed > currentMaxSpeed)
                {
                    speed = Mathf.Lerp(speed, currentMaxSpeed, 10 * Time.deltaTime);
                }

                Vector3 newPosition = transform.position + (direction.normalized * speed * KMHTOMS * Time.deltaTime);
                transform.position = newPosition;

                if(direction != Vector3.zero)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), maxspeed * Time.deltaTime *15);
            }
            else
            {
                speed = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TrafficLight") && !drivingTrafficLights )
            {
                TrafficLight trafic = other.GetComponent<TrafficLight>();
                if (Vector3.Angle(-trafic.transform.forward,transform.forward) < 25)
                {
                    if (!trafic.isGreen)
                    {
                        drivingTrafficLights = true;
                        isMoving = false;
                        trafic.lightChange += StartMoving;
                    }
                }
            }
            if (other.CompareTag("Crosswalk"))
            {
                Crosswalk crosswalk = other.GetComponent<Crosswalk>();
                if (crosswalk.PedestriansAreCrossing)
                {
                    crosswalk.stateChange += CrosswalkChange;
                    isMoving = false;
                }

            }
            else if (other.CompareTag("LevelCrossing"))
            {
                LevelCrossingController levelCrossing = other.GetComponent<LevelCrossingController>();
                if (levelCrossing.trainCrossing)
                {
                    levelCrossing.stateChange += LevelCrossingChange;
                    isMoving = false;
                }

            }
            else if (other.CompareTag("Car") && !other.isTrigger && activePath >1)
            {
                float direction = Vector3.Angle(transform.forward, other.transform.forward);
                float carDirection = Vector3.Angle(transform.right, (other.transform.position - transform.position).normalized);
                if (direction < 50)
                {
                    drivingBihindCar = true;
                    carInFront = other.GetComponentInParent<CarBehavior>();
                    speed = carInFront.speed *0.8f;
                }
               if (direction > 40 && carDirection < 80 && carDirection > 45)
                {
                    isMoving = false;
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Car") && !other.isTrigger)
            {
                StopCoroutine(StartMovingAfterWait(0.2f));
                StartCoroutine(StartMovingAfterWait(0.2f));
                drivingBihindCar = false;
            }
            else if (other.CompareTag("TrafficLight"))
            {
                TrafficLight trafic = other.GetComponent<TrafficLight>();
                trafic.lightChange -= StartMoving;
                drivingTrafficLights = false;
            }
            else if (other.CompareTag("Crosswalk"))
            {
                other.GetComponent<Crosswalk>().stateChange -= CrosswalkChange;
            }
            else if (other.CompareTag("LevelCrossing"))
            {
                other.GetComponent<LevelCrossingController>().stateChange -= LevelCrossingChange;
            }
        }
        void StartMoving(bool isGreen)
        {
            if (isGreen)
            {
                drivingTrafficLights = false;
                isMoving = true;
            }
        }
        void CrosswalkChange(bool crossing)
        {
            if (!crossing && !drivingTrafficLights)
            {
                isMoving = true;
            }
        }
        void LevelCrossingChange(bool crossing)
        {
            if (!crossing)
            {
                isMoving = true;
            }
        }

        IEnumerator StartMovingAfterWait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            isMoving = true;
        }

    }
}