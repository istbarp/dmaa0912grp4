//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//namespace Tenebris
//{
//    namespace DungeonEditor
//    {
//        public class Grid : MonoBehaviour
//        {
//            //GameObject[,] block;
//            public List<GameObject> SelectedGo { get; private set; }
//            private Vector2 Gridsize { get; set; }
//            //public GameObject GoToPlace { get; set; }
//            public Vector2 Size { get; set; }
//            //public EditorTool CurrentTool { get; set; }

//            public Texture2D SelectionHighLight = null;
//            public Rect SelectionArea = new Rect(0, 0, 0, 0);
//            public Vector3 StartPoint { get; private set; }

//            public bool IsDraging { get; set; }
//            public bool DropDraging { get; set; }
//            public bool IsOverLapping { get; private set; }

//            public float timer { get; set; }

//            private bool draw = false;

//            void Start()
//            {
//                DropDraging = true;
//                Gridsize = new Vector2(100, 100);
//                StartPoint = -Vector3.one;
//                //block = new GameObject[(int)Gridsize.x, (int)Gridsize.y];
//            }

//            // Update is called once per frame
//            void Update()
//            {
//                CheckDungeon();
//            }

//            //Clean Up the mess of selections
//            void LateUpdate()
//            {
//                if (IsOverLapping)
//                {
//                    return;
//                }
//                if (Input.GetKey(KeyCode.Mouse0))
//                {
//                    timer += Time.deltaTime;
//                }
//                if (Input.GetKeyUp(KeyCode.Mouse0))
//                {
//                    if (DropDraging)
//                    {
//                        DropDraging = false;
//                    }
//                    else if (timer < 0.5f)
//                    {
//                        DropDraging = true;
//                    }
//                    timer = 0;
//                }
//                if (Input.GetKeyUp(KeyCode.Mouse0) && IsDraging && !DropDraging)
//                {
//                    IsDraging = false;
//                    tempGameObject.transform.DetachChildren();
//                    //draw = true;
//                }
//            }

//            private void CheckDungeon()
//            {
//                if (IsDraging)
//                {
//                    return;
//                }
//                if (Input.GetKeyDown(KeyCode.Mouse0))
//                {
//                    StartPoint = Input.mousePosition;
//                }
//                else if (Input.GetKeyUp(KeyCode.Mouse0))
//                {
//                    if (Vector2.Distance(StartPoint, Input.mousePosition) < 0.1f)
//                    {
//                        return;
//                    }
//                    if (!Input.GetKey(KeyCode.LeftControl))
//                    {
//                        SelectedGo.Clear();
//                    }
//                    StartPoint = -Vector3.one;
//                    draw = false;
//                }

//                if (Input.GetKey(KeyCode.Mouse0))
//                {
//                    if (Vector2.Distance(StartPoint, Input.mousePosition) < 0.1f)
//                    {
//                        draw = false;
//                        return;
//                    }
//                    else
//                    {
//                        draw = true;
//                    }
//                    SelectionArea = new Rect(StartPoint.x, Screen.height - StartPoint.y, Input.mousePosition.x - StartPoint.x,
//                        (Screen.height - Input.mousePosition.y) - (Screen.height - StartPoint.y));

//                    if (SelectionArea.width < 0)
//                    {
//                        SelectionArea.x += SelectionArea.width;
//                        SelectionArea.width = -SelectionArea.width;
//                    }
//                    if (SelectionArea.height < 0)
//                    {
//                        SelectionArea.y += SelectionArea.height;
//                        SelectionArea.height = -SelectionArea.height;
//                    }
//                }
//            }

//            public void MoveObject(List<GameObject> gos, Vector3 size)
//            {
//                IsDraging = true;
//                draw = false;
//                Vector2 vec = Camera.main.ScreenToWorldPlane(Input.mousePosition);
//                if (tempGameObject == null)
//                {
//                    tempGameObject = new GameObject("TEMP_" + Random.Range(0, int.MaxValue));
//                }
//                IsOverLapping = false;
//                for (int i = 0; i < gos.Count; i++)
//                {
//                    gos[i].transform.parent = tempGameObject.transform;
//                    foreach (Transform go in gos[i].GetComponentInChildren<Transform>())
//                    {
//                        if (IsOverLapping)
//                            break;

//                        go.gameObject.layer = 2;
//                        CheckOverlay(go.gameObject);
//                        go.gameObject.layer = 0;
//                    }
//                }
//                //tempGameObject.transform.position = new Vector3(Mathf.RoundToInt(vec.x), 0, Mathf.RoundToInt(vec.y));
//                tempGameObject.transform.position = new Vector3(Mathf.RoundToInt(vec.x) + (size.x % 2 != 0 ? 0.0f : 0.5f), 0,
//                    Mathf.RoundToInt(vec.y) + (size.z % 2 != 0 ? 0.0f : 0.5f));
//                SelectObjects();
//            }

//            bool CheckOverlay(GameObject go)
//            {
//                if (Physics.OverlapSphere(go.transform.position, 0.1f, 1).Count() - 1 >= 1 || go.transform.position.x < 0 || go.transform.position.z < 0
//                    || go.transform.position.x > Gridsize.x || go.transform.position.z > Gridsize.y)
//                {
//                    //Debug.Log("WARNING: OVERLAPPING!!!" + Physics.OverlapSphere(go.transform.position, 0.1f).Count());
//                    return IsOverLapping = true;
//                }
//                return IsOverLapping = false;
//            }

//            bool CheckOverlay(List<GameObject> gos, Vector3 size)
//            {
//                foreach (GameObject go in gos)
//                {
//                    if (Physics.OverlapSphere(go.transform.position, 0.1f, 1).Count() - 1 >= 1)
//                    {
//                        Debug.Log("WARNING: OVERLAPPING!!!" + Physics.OverlapSphere(go.transform.position, 0.1f).Count());
//                    }
//                }

//                //for (int i = 0; i < size.x; i++)
//                //{
//                //    for (int j = 0; j < size.z; j++)
//                //    {
//                //        if (block[(int)(pos.x / 2) + i, (int)(pos.y / 2) + j])
//                //        {
//                //            return true;
//                //        }
//                //    }
//                //}
//                return false;
//            }

//            Rect ToRect(Vector2 vec1, Vector2 vec2)
//            {
//                Rect rec = new Rect();
//                if (vec1.x < vec2.x)
//                {
//                    rec.x = (int)vec1.x;
//                    rec.width = (int)(vec2.x - vec1.x);
//                }
//                else
//                {
//                    rec.x = (int)vec2.x;
//                    rec.width = (int)(vec1.x - vec2.x);
//                }
//                if (vec1.y < vec2.y)
//                {
//                    rec.y = (int)vec1.y;
//                    rec.height = (int)(vec2.y - vec1.y);
//                }
//                else
//                {
//                    rec.y = (int)vec2.y;
//                    rec.height = (int)(vec1.y - vec2.y);
//                }
//                return rec;
//            }
//        }

//        public enum EditorTool
//        {
//            Select, Place, Delete, Define, Copy,
//        }

//        public static class CameraExtensions
//        {
//            public static Vector3 ScreenToWorld(this Camera cam, Vector2 screenPos)
//            {
//                // Create a ray going into the scene starting 
//                // from the screen position provided 
//                Ray ray = cam.ScreenPointToRay(screenPos);


//                // ray hit an object, return intersection point
//                RaycastHit hit;
//                if (Physics.Raycast(ray, out hit))
//                    return hit.point;

//                // ray didn't hit any solid object, so return the 
//                // intersection point between the ray and 
//                // the Y=0 plane (horizontal plane)
//                float t = -ray.origin.y / ray.direction.y;
//                return ray.GetPoint(t);
//            }

//            public static Vector2 ScreenToWorldPlane(this Camera cam, Vector2 screenPos)
//            {
//                // Create a ray going into the scene starting 
//                // from the screen position provided 
//                Ray ray = cam.ScreenPointToRay(screenPos);

//                // ray didn't hit any solid object, so return the 
//                // intersection point between the ray and 
//                // the Y=0 plane (horizontal plane)
//                float t = -ray.origin.y / ray.direction.y;
//                return new Vector2(ray.GetPoint(t).x, ray.GetPoint(t).z);
//            }
//        }
//    }
//}