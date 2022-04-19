using UnityEngine;
using System.Collections.Generic;

public class ParabolaController : MonoBehaviour
{
    #region Public Fields

    public float Speed = 1;

    public GameObject ParabolaRoot;

    public bool Autostart = true;

    public bool Animation = true;

    // 공이 날라갈 표적, ParabolaRoot의 마지막 Point를 넣으면 됨 -> 경우에 따라 수정 가능
    public List<Transform> targets;

    public int level;
    public bool isSuccess;

    #endregion

    //next parabola event
    internal bool nextParbola = false;

    //animation time
    protected float animationTime = float.MaxValue;

    //gizmo
    protected ParabolaFly gizmo;

    //draw
    protected ParabolaFly parabolaFly;

    #region Private Fields
    //라인렌더러 추가
    private LineRenderer lineRenderer;

    private int lineCount;

    #endregion

    #region MonoBehaviour Callbacks
    // Scene에서 Gizmo 그려줌
    void OnDrawGizmos()
    {
        // Gizmo가 그려지지 않았다면
        if (gizmo == null)
        {
            gizmo = new ParabolaFly(ParabolaRoot.transform);
        }

        gizmo.RefreshTransforms(1f);
        // point 개수가 짝수라면 -> 오류! 그리지 않음
        if ((gizmo.Points.Length - 1) % 2 != 0)
            return;

        // 점들의 연속 -> 선
        int accur = 50;  // 찍을 점의 개수, 낮을수록 각진 포물선이 그려짐
        Vector3 prevPos = gizmo.Points[0].position;
        for (int c = 1; c <= accur; c++)
        {
            float currTime = c * gizmo.GetDuration() / accur;
            Vector3 currPos = gizmo.GetPositionAtTime(currTime);
            float mag = (currPos - prevPos).magnitude * 2;
            Gizmos.color = new Color(mag, 0, 0, 1);
            Gizmos.DrawLine(prevPos, currPos);
            Gizmos.DrawSphere(currPos, 0.01f);
            prevPos = currPos;
        }
    }



    // Use this for initialization
    void Start()
    {
        parabolaFly = new ParabolaFly(ParabolaRoot.transform);

        isSuccess = false;

        level = 1;
        lineCount = 70;

        targets.Add(ParabolaRoot.transform.GetChild(ParabolaRoot.transform.childCount - 1));    // 마지막 포인트도 targets에 넣어줌

        // 자동시작
        if (Autostart)
        {
            RefreshTransforms(Speed);
            FollowParabola();
        }
    }


    // Update is called once per frame
    void Update()
    {
        nextParbola = false;

        // 포물선을 따라 움직이는 동안
        if (Animation && parabolaFly != null && animationTime < parabolaFly.GetDuration())
        {
            int parabolaIndexBefore;
            int parabolaIndexAfter;
            parabolaFly.GetParabolaIndexAtTime(animationTime, out parabolaIndexBefore); // 이전 구간의 시간
            animationTime += Time.deltaTime;    // 한 프레임만큼 더해줌
            parabolaFly.GetParabolaIndexAtTime(animationTime, out parabolaIndexAfter);  // 다음 구간의 시간

            transform.position = parabolaFly.GetPositionAtTime(animationTime);

            if (parabolaIndexBefore != parabolaIndexAfter)
                nextParbola = true;

            transform.Rotate(Vector3.up * Time.deltaTime * 360f);
            //if (transform.position.y > HighestPoint.y)
            //HighestPoint = transform.position;
        }

        // 포물선 따라 움직이기 끝나면
        else if (Animation && parabolaFly != null && animationTime > parabolaFly.GetDuration())
        {
            print("여기 실행 됨");
            animationTime = float.MaxValue;
            Animation = false;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("공 막음");
            StopFollow();
            Animation = false;
            isSuccess = true;
        }

        else if (collision.gameObject.CompareTag("GoalNet"))
        {
            Debug.Log("골 먹힘");
            StopFollow();
            Animation = false;
            isSuccess = false;
        }
    }

    #endregion

    #region Public Methods

    // 라인렌더러 그리기 & 공 포물선 이동
    public void KickingBall()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            float x = Random.RandomRange(-0.235f, 0.235f);
            float y = Random.RandomRange(0.01f, 0.2f);

            if (i.Equals(targets.Count - 1))
                targets[i].localPosition = new Vector3(x, y, targets[i].localPosition.z);
            else
                targets[i].localPosition = new Vector3(x, targets[i].localPosition.y, targets[i].localPosition.z);
        }

        lineRenderer = ParabolaRoot.transform.GetChild(0).GetComponent<LineRenderer>();

        ParabolaFly line = new ParabolaFly(ParabolaRoot.transform);

        line.RefreshTransforms(1f);
        // point 개수가 짝수라면 -> 오류! 그리지 않음
        if ((line.Points.Length - 1) % 2 != 0)
            return;

        // 점들의 연속 -> 선
        int accur = 100;  // 찍을 점의 개수, 낮을수록 각진 포물선이 그려짐

        Vector3 prevPos = line.Points[0].position;

        lineRenderer.SetColors(Color.red, Color.yellow);
        lineRenderer.SetWidth(0.05f, 0.1f);
        lineRenderer.SetVertexCount(lineCount);
        lineRenderer.SetPosition(0, prevPos);

        for (int c = 1; c <= accur; c++)
        {
            float currTime = c * line.GetDuration() / accur;
            Vector3 currPos = line.GetPositionAtTime(currTime);
            float mag = (currPos - prevPos).magnitude * 2;

            if (c < lineCount)
                lineRenderer.SetPosition(c, currPos);
            prevPos = currPos;
        }
        RefreshTransforms(Speed);
        FollowParabola();
    }

    public void LevelUp(int level)
    {
        if (level == 2)
        {
            for (int i = 1; i < ParabolaRoot.transform.childCount; i += 2)
            {
                targets.Add(ParabolaRoot.transform.GetChild(i));   // 타겟은 짝수번째의 point
            }
            lineCount = 60;
        }
    }

    public void FollowParabola()
    {
        RefreshTransforms(Speed);
        animationTime = 0f;
        transform.position = parabolaFly.Points[0].position;
        Animation = true;
        //HighestPoint = points[0].position;
    }

    public Vector3 getHighestPoint(int parabolaIndex)
    {
        return parabolaFly.getHighestPoint(parabolaIndex);
    }

    public Transform[] getPoints()
    {
        return parabolaFly.Points;
    }

    public Vector3 GetPositionAtTime(float time)
    {
        return parabolaFly.GetPositionAtTime(time);
    }

    public float GetDuration()
    {
        return parabolaFly.GetDuration();
    }

    public void StopFollow()
    {
        animationTime = float.MaxValue;
    }
    /// <summary>
    /// Returns children transforms, sorted by name.
    /// </summary>
    public void RefreshTransforms(float speed)
    {
        parabolaFly.RefreshTransforms(speed);
    }


    public static float DistanceToLine(Ray ray, Vector3 point)
    {
        //see:http://answers.unity3d.com/questions/62644/distance-between-a-ray-and-a-point.html
        return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
    }

    public static Vector3 ClosestPointInLine(Ray ray, Vector3 point)
    {
        return ray.origin + ray.direction * Vector3.Dot(ray.direction, point - ray.origin);
    }

    #endregion

    #region ParabolaFly Class
    // 포물선의 궤적에 해당하는 클래스
    public class ParabolaFly
    {

        public Transform[] Points;  // 포물선의 기준이 되는 점의 개수(홀수개여야 함)
        protected Parabola3D[] parabolas;   // 포물선의 개수를 의미하는 듯
        protected float[] partDuration;
        protected float completeDuration;

        // 생성자
        public ParabolaFly(Transform ParabolaRoot)
        {
            // ParabolaRoot 오브젝트 + 자식오브젝트의 Transform 컴포넌트 받아옴 => 일단 리턴값은 Component형
            List<Component> components = new List<Component>(ParabolaRoot.GetComponentsInChildren(typeof(Transform)));

            // Component형이었던 것들을 Transform형으로 변환
            List<Transform> transforms = components.ConvertAll(c => (Transform)c);

            // 자식오브젝트의 Transform만 남겨둠
            transforms.Remove(ParabolaRoot.transform);
            // List 안의 내용을 이름(오름차순)으로 정렬
            transforms.Sort(delegate (Transform a, Transform b)
            {
                return a.name.CompareTo(b.name);
            });

            Points = transforms.ToArray();  // List를 Array로 변환

            //check if odd
            if ((Points.Length - 1) % 2 != 0)   // 짝수개라면
                throw new UnityException("ParabolaRoot needs odd number of points");

            //check if larger is needed
            // parabolas가 아직 생성되지 않았거나, parboloas의 길이가 Points 길이의 절반보다 작다면 => 다시 생성해줌
            if (parabolas == null || parabolas.Length < (Points.Length - 1) / 2)
            {
                parabolas = new Parabola3D[(Points.Length - 1) / 2];
                partDuration = new float[parabolas.Length];
            }

        }

        public Vector3 GetPositionAtTime(float time)
        {
            int parabolaIndex;
            float timeInParabola;
            GetParabolaIndexAtTime(time, out parabolaIndex, out timeInParabola);

            var percent = timeInParabola / partDuration[parabolaIndex];
            return parabolas[parabolaIndex].GetPositionAtLength(percent * parabolas[parabolaIndex].Length);
        }

        // 왜 2단으로 만든거지 개빡취네
        public void GetParabolaIndexAtTime(float time, out int parabolaIndex)
        {
            float timeInParabola;   // 리턴받기 위한 float 변수
            GetParabolaIndexAtTime(time, out parabolaIndex, out timeInParabola);    // 넣어준다
        }

        public void GetParabolaIndexAtTime(float time, out int parabolaIndex, out float timeInParabola)
        {
            //f(x) = axÂ² + bx + c
            timeInParabola = time;
            parabolaIndex = 0;

            //determine parabola
            while (parabolaIndex < parabolas.Length - 1 && partDuration[parabolaIndex] < timeInParabola)
            {
                timeInParabola -= partDuration[parabolaIndex];  // timeParabola가 더 커지면 포물선 한 파트 이동 끝난거임
                parabolaIndex++;

            }
        }

        public float GetDuration()
        {
            return completeDuration;
        }

        public Vector3 getHighestPoint(int parabolaIndex)
        {
            return parabolas[parabolaIndex].getHighestPoint();
        }

        /// <summary>
        /// Returns children transforms, sorted by name.
        /// </summary>
        public void RefreshTransforms(float speed)
        {
            if (speed <= 0f)
                speed = 1f;

            if (Points != null)
            {

                completeDuration = 0;

                //create parabolas
                for (int i = 0; i < parabolas.Length; i++)
                {
                    if (parabolas[i] == null)
                        parabolas[i] = new Parabola3D();

                    parabolas[i].Set(Points[i * 2].position, Points[i * 2 + 1].position, Points[i * 2 + 2].position);
                    partDuration[i] = parabolas[i].Length / speed;
                    completeDuration += partDuration[i];
                }


            }

        }
    }

    #endregion

    #region Parabola3D Class

    // 포물선 part 하나를 나타내는 클래스
    public class Parabola3D
    {
        public float Length { get; private set; }

        public Vector3 A;
        public Vector3 B;
        public Vector3 C;

        protected Parabola2D parabola2D;
        protected Vector3 h;
        protected bool tooClose;
        
        // 기본 생성자
        public Parabola3D()
        {
        }

        // 생성자
        public Parabola3D(Vector3 A, Vector3 B, Vector3 C)
        {
            Set(A, B, C);
        }

        // Vector3 A, B, C을 설정해주는 메소드
        public void Set(Vector3 A, Vector3 B, Vector3 C)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            refreshCurve();
        }

        // 가장 높은 Point가 어딘지
        public Vector3 getHighestPoint()
        {
            var d = (C.y - A.y) / parabola2D.Length;
            var e = A.y - C.y;

            var parabolaCompl = new Parabola2D(parabola2D.a, parabola2D.b + d, parabola2D.c + e, parabola2D.Length);

            Vector3 E = new Vector3();
            E.y = parabolaCompl.E.y;
            E.x = A.x + (C.x - A.x) * (parabolaCompl.E.x / parabolaCompl.Length);
            E.z = A.z + (C.z - A.z) * (parabolaCompl.E.x / parabolaCompl.Length);

            return E;
        }

        public Vector3 GetPositionAtLength(float length)
        {
            //f(x) = axÂ² + bx + c
            var percent = length / Length;

            var x = percent * (C - A).magnitude;
            if (tooClose)
                x = percent * 2f;

            Vector3 pos;

            pos = A * (1f - percent) + C * percent + h.normalized * parabola2D.f(x);
            if (tooClose)
                pos.Set(A.x, pos.y, A.z);

            return pos;
        }

        private void refreshCurve()
        {

            if (Vector2.Distance(new Vector2(A.x, A.z), new Vector2(B.x, B.z)) < 0.1f &&
                Vector2.Distance(new Vector2(B.x, B.z), new Vector2(C.x, C.z)) < 0.1f)
                tooClose = true;
            else
                tooClose = false;

            Length = Vector3.Distance(A, B) + Vector3.Distance(B, C);

            if (!tooClose)
            {
                refreshCurveNormal();
            }
            else
            {
                refreshCurveClose();
            }
        }


        private void refreshCurveNormal()
        {
            //                        .  E   .
            //                   .       |       point[1]
            //             .             |h         |       .
            //         .                 |       ___v1------point[2]
            //      .            ______--vl------    
            // point[0]---------
            //

            //lower v1
            Ray rl = new Ray(A, C - A);
            var v1 = ClosestPointInLine(rl, B);

            //get A=(x1,y1) B=(x2,y2) C=(x3,y3)
            Vector2 A2d, B2d, C2d;

            A2d.x = 0f;
            A2d.y = 0f;
            B2d.x = Vector3.Distance(A, v1);
            B2d.y = Vector3.Distance(B, v1);
            C2d.x = Vector3.Distance(A, C);
            C2d.y = 0f;

            parabola2D = new Parabola2D(A2d, B2d, C2d);

            //lower v
            //var p = parabola.E.x / parabola.Length;
            //Vector3 vl = points[0].position * (1f - p) + points[2].position * p;

            //h
            h = (B - v1) / Vector3.Distance(v1, B) * parabola2D.E.y;
        }

        private void refreshCurveClose()
        {
            //distance to x0 - x2 line = |(x1-x0)x(x1-x2)|/|x2-x0|
            var fac01 = (A.y <= B.y) ? 1f : -1f;
            var fac02 = (A.y <= C.y) ? 1f : -1f;

            Vector2 A2d, B2d, C2d;

            //get A=(x1,y1) B=(x2,y2) C=(x3,y3)
            A2d.x = 0f;
            A2d.y = 0f;

            //b = sqrt(cÂ²-aÂ²)
            B2d.x = 1f;
            B2d.y = Vector3.Distance((A + C) / 2f, B) * fac01;

            C2d.x = 2f;
            C2d.y = Vector3.Distance(A, C) * fac02;

            parabola2D = new Parabola2D(A2d, B2d, C2d);
            h = Vector3.up;
        }
    }

    #endregion

    #region Parabola2D Class
    public class Parabola2D
    {
        public float a { get; private set; }
        public float b { get; private set; }
        public float c { get; private set; }

        public Vector2 E { get; private set; }
        public float Length { get; private set; }

        // float 형태의 값들이 모두 주어진 경우 -> 그냥 set만 해주면 됨
        public Parabola2D(float a, float b, float c, float length)
        {
            this.a = a;
            this.b = b;
            this.c = c;

            setMetadata();
            this.Length = length;
        }

        // Vector2 형태의 값들만이 주어진 경우 -> 계산 필요
        public Parabola2D(Vector2 A, Vector2 B, Vector2 C)
        {
            //f(x) = axÂ² + bx + c
            //a = (x1(y2 - y3) + x2(y3 - y1) + x3(y1 - y2)) / ((x1 - x2)(x1 - x3)(x3 - x2))
            //b = (x1Â²(y2 - y3) + x2Â²(y3 - y1) + x3Â²(y1 - y2))/ ((x1 - x2)(x1 - x3)(x2 - x3))
            //c = (x1Â²(x2y3 - x3y2) + x1(x3Â²y2 - x2Â²y3) + x2x3y1(x2 - x3))/ ((x1 - x2)(x1 - x3)(x2 - x3))
            var divisor = ((A.x - B.x) * (A.x - C.x) * (C.x - B.x));
            if (divisor == 0f)
            {
                A.x += 0.00001f;
                B.x += 0.00002f;
                C.x += 0.00003f;
                divisor = ((A.x - B.x) * (A.x - C.x) * (C.x - B.x));
            }
            // 알아서 착착착 구해줌~
            a = (A.x * (B.y - C.y) + B.x * (C.y - A.y) + C.x * (A.y - B.y)) / divisor;
            b = (A.x * A.x * (B.y - C.y) + B.x * B.x * (C.y - A.y) + C.x * C.x * (A.y - B.y)) / divisor;
            c = (A.x * A.x * (B.x * C.y - C.x * B.y) + A.x * (C.x * C.x * B.y - B.x * B.x * C.y) + B.x * C.x * A.y * (B.x - C.x)) / divisor;

            b = b * -1f;//hack

            setMetadata();
            Length = Vector2.Distance(A, C);
        }

        // 이차곡선그래프 수식
        public float f(float x)
        {
            return a * x * x + b * x + c;
        }

        // 필드 값 중 하나인 Vector2 E를 구해줌
        private void setMetadata()
        {
            //derive
            //a*xÂ²+b*x+c = 0
            //2ax+b=0
            //x = -b/2a
            var x = -b / (2 * a);
            E = new Vector2(x, f(x));
        }



    }

    #endregion
}