using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyPerfect.City
{
    [RequireComponent(typeof(PathFinding)), RequireComponent(typeof(Rigidbody))]
    public class TrainController : MonoBehaviour
    {
        //[HideInInspector]
        public List<Path> trajectory = new List<Path>();
        [HideInInspector]
        public List<Wagon> wagons = new List<Wagon>(64);
        private PathFinding pathFinding;
        public float maxspeed = 5.0f;
        private float currentMaxSpeed;
        [Range(0f,1f)]
        public float acceleration = 0.05f;

        private const float KMHTOMS = 0.27777f;
        private float speed = 0;
        private int activepoint = 0;
        private int activePath = 0;
        bool isMoving = false;
        private Vector3 targetDrivePoint;
        public List<Vector3> checkpoints = new List<Vector3>();
        private Vector3 start;
        private BoxCollider trainCollider;
        private void Awake()
        {
            pathFinding = GetComponent<PathFinding>();
            trainCollider = GetComponent<BoxCollider>();
        }

        void Start()
        {
            trajectory = pathFinding.GetPath(transform.position, checkpoints[0], PathType.Rail);
            if (trajectory != null)
            {
                isMoving = true;
                currentMaxSpeed = maxspeed;
                SetUpWagons();
                targetDrivePoint = trajectory[0].pathPositions[0].transform.position;
                checkpoints.Reverse();
            }
            else
            {
                Debug.Log(name +": Path not found");
            }
        }
        void FixedUpdate()
        {
            if (isMoving && trajectory != null)
            {

                if (Vector3.Dot(targetDrivePoint - transform.position, transform.forward) <= 0)
                {
                    MoveToNextPoint();
                }
                Vector3 direction = targetDrivePoint - transform.position;

                if(speed < currentMaxSpeed)
                {
                    speed += ((maxspeed * Mathf.Cos((speed/maxspeed) *  0.5f * Mathf.PI)) * Time.deltaTime) * acceleration;
                }
                else
                {
                    speed = Mathf.Lerp(speed, currentMaxSpeed, 10 * Time.deltaTime);
                }
               
                Vector3 newPosition = transform.position + (direction.normalized * speed * KMHTOMS * Time.deltaTime);
                transform.position = newPosition;

                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), maxspeed * Time.deltaTime * 15);
                }
                
                foreach (Wagon wagon in wagons)
                {
                    if (Vector3.Dot(wagon.target - wagon.transform.position, wagon.transform.forward) <= 0 )
                    {

                        if (wagon.activePath == trajectory.Count - 1)
                        {
                            if(wagon.activepoint < trajectory[wagon.activePath].pathPositions.Count)
                                wagon.activepoint++;
                        }
                        else
                        {
                            if (wagon.activepoint == trajectory[wagon.activePath].pathPositions.Count - 1)
                            {
                                wagon.activePath++;
                                wagon.activepoint = 1;
                            }
                            else
                            {
                                wagon.activepoint++;
                            }
                        }
                        wagon.target = trajectory[wagon.activePath].pathPositions[wagon.activepoint].transform.position;
                    }
                    Vector3 wagonDirection = wagon.target - wagon.transform.position;
                    Vector3 position = wagon.transform.position + (wagonDirection.normalized * speed * KMHTOMS * Time.deltaTime);
                    wagon.transform.position = position;
                    if (wagonDirection != Vector3.zero)
                    {
                        wagon.transform.rotation = Quaternion.RotateTowards(wagon.transform.rotation, Quaternion.LookRotation(wagonDirection, Vector3.up), maxspeed * Time.deltaTime * 15);
                    }
                }
            }
            else
            {
                speed = 0;
            }
        }
        //Gets next target point of the trajectory
        public void MoveToNextPoint()
        {
            if(activePath == trajectory.Count -1)
            {
                if (activepoint == trajectory[activePath].pathPositions.Count - 1)
                {

                    isMoving = false;
                    RotateTrain();
                    
                    SetTrajectory();
                    if (trajectory != null)
                    {
                        for (int i = 0; i < wagons.Count; i++)
                        {
                            wagons[i].target = trajectory[1].pathPositions[1].transform.position;
                            wagons[i].activePath = 1;
                            wagons[i].activepoint = 1;
                        }
                        activePath = 1;
                        activepoint = 1;
                        speed = 0;
                        if (wagons.Count > 1)
                            start = wagons[wagons.Count - 1].transform.position;
                        else
                            start = transform.position;

                        StartCoroutine(StartMoving());
                    }
                    else
                    {
                        Debug.Log(name + ": Path not found");
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
                targetDrivePoint = trajectory[activePath].pathPositions[activepoint].transform.position;
        }
        private void SetTrajectory()
        {
            checkpoints.Reverse();
            trajectory = pathFinding.GetPathWithCheckpoints(checkpoints, PathType.Rail);
        }
        //Rotates the whole train
        public void RotateTrain()
        {
            transform.position = wagons[wagons.Count - 1].transform.position;
            transform.Rotate(Vector3.up,180);
            SetUpWagons();
        }
        //Sets up wagons in line behind locomotive
        private void SetUpWagons()
        {
            for (int i = 0; i < wagons.Count; i++)
            {
                if (i == 0)
                {
                    wagons[i].transform.position = transform.position - ((wagons[i].Collider.size.z / 2 + trainCollider.size.z / 2)* wagons[i].transform.lossyScale.x) * transform.forward;
                }
                else
                {
                    wagons[i].transform.position = wagons[i - 1].transform.position - ((wagons[i].Collider.size.z / 2 + wagons[i - 1].Collider.size.z / 2)*wagons[i].transform.lossyScale.x)  * transform.forward;
                }
                wagons[i].target = trajectory[0].pathPositions[0].position;
                wagons[i].transform.rotation = Quaternion.LookRotation(transform.forward);
            }
        }
        IEnumerator StartMoving()
        {
            yield return new WaitForSeconds(1.5f);
            isMoving = true;
        }
    }
}
