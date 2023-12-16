using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 07 August 2019
    /// Modified by: 
    /// quiz label generator . this script should be attached to label quiz main model
    /// </summary>
    public class quizlabeler : MonoBehaviour
    {
        [Tooltip("list of Quiz Label origin points and label follow points after matching")]
        public List<quizLabelClass> quizLabels;
        [SerializeField]
        [Tooltip("list of game objects for spawning labels on")]
        private List<Transform> spawnPoints;
        // label game objects 
        [HideInInspector]
        public List<GameObject> quizLabelGameObjects;
        [SerializeField]
        private LayerMask clickableLayer;

        // label line width
        public float LineWidth = 0.005f;
        // label line color
        public Color LineColor = Color.white;
        // label follow speed
        public float LabelFollowSpeed = 30f;


        [HideInInspector]
        public Transform LabelParent;
        // main camera
        [HideInInspector]
        public Camera MyCamera;
        // line renderer material
        [HideInInspector]
        public Material LineMaterial;

        bool startFacingCamera = false;


        [HideInInspector]
        public Transform ScaleParent;

        // list to store the instantiated line renderers
        List<LineRenderer> lineList;


        public void initQuizLabels(GameObject originPrefab, List<GameObject> labels, Camera camera, Transform labelsParent, Material material)
        {
            MyCamera = camera;
            LineMaterial = material;
            quizLabelGameObjects = labels;
            setOriginPrefabs(originPrefab);
            setLabelRandomLocations(true);
            MakeMyLines();
            startFacingCamera = true;

            // hiding the label for labels that dont have any text.. from localization
            for (int i = 0; i < quizLabelGameObjects.Count; i++)
            {
                labelElement le = quizLabelGameObjects[i].GetComponent<labelElement>();
                if (!le.isLabelActive())
                {
                    quizLabels[i].hideThis();
                }
            }

        }

        void setOriginPrefabs(GameObject originPrefab)
        {
            for (int i = 0; i < quizLabels.Count; i++)
            {
                GameObject go = Instantiate(originPrefab) as GameObject;
                go.transform.SetParent(quizLabels[i].Origin.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                quizLabels[i].Origin = go;
            }
        }

        public int getTotalNumberOfMatches()
        {
            int matches = 0;

            for (int i = 0; i < quizLabelGameObjects.Count; i++)
            {
                labelElement le = quizLabelGameObjects[i].GetComponent<labelElement>();
                if (le.isLabelActive())
                    matches++;

            }

            return matches;
        }


        public void reShuffleQuiz()
        {
            setLabelRandomLocations(false);
            hideLines();
        }

        void hideLines()
        {
            for (int i = 0; i < quizLabelGameObjects.Count; i++)
            {
                quizLabels[i].followTarget = false;
                lineList[i].enabled = false;
            }
        }

        public void startSelectedLabelFollow(int id)
        {
            quizLabels[id].followTarget = true;
        }

        public void labelsBackToSpawnPoint()
        {
            for (int i = 0; i < quizLabelGameObjects.Count; i++)
            {
                quizLabels[i].followTarget = false;

                setToSpawnLocation(i);
                lineList[i].enabled = false;
                // setting done to false so matches can be made again
                quizLabelId qlId = quizLabelGameObjects[i].GetComponent<quizLabelId>();
                qlId.done = false;
                qlId.Reset();
                qlId = quizLabels[i].Origin.GetComponent<quizLabelId>();
                qlId.done = false;
                qlId.Reset();
            }
        }

        // labels follow the target
        void labelsFollowTargets()
        {
            if (quizLabels.Count > 0)
            {
                for (int i = 0; i < quizLabels.Count; i++)
                {
                    if (quizLabels[i].followTarget)
                    {
                        quizLabelGameObjects[i].transform.SetParent(LabelParent);
                        quizLabelGameObjects[i].transform.position = Vector3.Lerp(quizLabelGameObjects[i].transform.position,
                                                                             quizLabels[i].Target.transform.position,
                                                                             Mathf.SmoothStep(0, 1, Time.deltaTime * LabelFollowSpeed));
                    }
                }
            }
        }


        // setting labels to random spawn locations
        void setLabelRandomLocations(bool addQuizId)
        {
            // shuffling the spawn points list order
            spawnPoints.Shuffle();

            for (int i = 0; i < quizLabelGameObjects.Count; i++)
            {
                setToSpawnLocation(i);

                labelElement le = quizLabelGameObjects[i].GetComponent<labelElement>();
                if(le.isLabelActive())
                    quizLabelGameObjects[i].SetActive(true);

                if (addQuizId)
                    addQuizIdComponent(i);
            }
        }


        void setToSpawnLocation(int id)
        {
            quizLabelGameObjects[id].transform.SetParent(spawnPoints[id]);
            quizLabelGameObjects[id].transform.localPosition = Vector3.zero;

        }

        // adding quiz id component to the origin and label with same id so they can be matched
        void addQuizIdComponent(int i)
        {
            quizLabelId qlId = quizLabelGameObjects[i].AddComponent(typeof(quizLabelId)) as quizLabelId;
            qlId.id = i;
            qlId.isLabel = true;
            qlId = quizLabels[i].Origin.AddComponent(typeof(quizLabelId)) as quizLabelId;
            quizLabels[i].Origin.layer = 9;
            qlId.id = i;
            qlId.isLabel = false;
        }


        // making all the labels face the camera
        void OrientLabelsTowardstheCamera()
        {
            if (quizLabelGameObjects.Count > 0)
            {
                for (int i = 0; i < quizLabels.Count; i++)
                {
                    Vector3 v = MyCamera.transform.position - quizLabels[i].Target.transform.position;
                    v.x = v.z = 0.0f;
                    // quizLabels[i].LabelPoint.transform.LookAt(MyCamera.transform.position - v);
                    //  quizLabels[i].LabelPoint.transform.rotation = (MyCamera.transform.rotation); // Take care about camera rotation
                    quizLabels[i].Origin.transform.LookAt(MyCamera.transform.position - v);
                    quizLabels[i].Origin.transform.rotation = (MyCamera.transform.rotation);
                    quizLabelGameObjects[i].transform.LookAt(MyCamera.transform.position - v);
                    quizLabelGameObjects[i].transform.rotation = (MyCamera.transform.rotation);
                }
            }
        }


        private void Update()
        {
            if (startFacingCamera)
            {
                OrientLabelsTowardstheCamera();
                labelsFollowTargets();
                DrawLines();
            }
        }

        // drawing line between label origin and label point.
        void DrawLines()
        {
            if (quizLabels.Count > 0)
            {
                for (int i = 0; i < lineList.Count; i++)
                {
                    if (quizLabels[i].followTarget)
                    {
                        lineList[i].enabled = true;
                        lineList[i].SetPosition(0, quizLabels[i].Origin.transform.position);
                        lineList[i].SetPosition(1, quizLabelGameObjects[i].transform.position);
                    }
                }
            }

        }

        // make a list of line renderers, a line for each label present;
        void MakeMyLines()
        {
            lineList = new List<LineRenderer>();
            if (quizLabels.Count > 0)
            {
                for (int i = 0; i < quizLabels.Count; i++)
                {
                    lineList.Add(createALine(quizLabels[i].Origin));
                }
            }
        }
        // creatting one line renderer and returning it
        LineRenderer createALine(GameObject go)
        {
            // Add a Line Renderer to the GameObject
            LineRenderer line = go.AddComponent<LineRenderer>();
            // Set the width of the Line Renderer
            line.startWidth = LineWidth;
            line.positionCount = 2;
            line.startColor = LineColor;
            line.material = LineMaterial;
            line.enabled = false;
            return line;
        }
    }



}
