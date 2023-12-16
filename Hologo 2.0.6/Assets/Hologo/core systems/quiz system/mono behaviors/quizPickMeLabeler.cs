using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 19 August 2019
    /// Modified by: 
    /// quiz pickme label generator . this script should be attached to pick me quiz main model. 
    /// </summary>
    public class quizPickMeLabeler : MonoBehaviour
    {

        [SerializeField]
        [Tooltip("list of game objects for spawning the pick me models on and labels")]
        private List<twoTransforms> spawnAndLabelPoints;



        [HideInInspector]
        public Camera MyCamera;

        bool startFacingCamera = false;

        public void initQuizLabels(List<GameObject> labels, Camera camera)
        {
            MyCamera = camera;
            startFacingCamera = true;
        }



        public List<twoTransforms> getSpawnPoints()
        {
            spawnAndLabelPoints.Shuffle();
            return spawnAndLabelPoints;
        }


        // making all the labels face the camera
        void OrientLabelsTowardstheCamera()
        {
            if (spawnAndLabelPoints.Count > 0)
            {
                for (int i = 0; i < spawnAndLabelPoints.Count; i++)
                {
                    Vector3 v = MyCamera.transform.position - spawnAndLabelPoints[i].labelPoint.position;
                    v.x = v.z = 0.0f;
                    spawnAndLabelPoints[i].labelPoint.LookAt(MyCamera.transform.position - v);
                    spawnAndLabelPoints[i].labelPoint.transform.rotation = (MyCamera.transform.rotation);
                }
            }
        }

        private void Update()
        {
            if (startFacingCamera)
            {
                OrientLabelsTowardstheCamera();
            }
        }


    }

    [System.Serializable]
    public class twoTransforms
    {
        public Transform spawnPoint;
        public Transform labelPoint;
    }

}