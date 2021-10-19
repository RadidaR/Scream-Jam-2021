using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;
//using ParadoxNotion;
using NodeCanvas.BehaviourTrees;

namespace ScreamJam
{
    public class GhostScript : MonoBehaviour
    {
        BehaviourTreeOwner ai;
        //NodeCanvas.BehaviourTrees
        Sensor sensor;
        [SerializeField] bool hasSpottedPlayer;
        [SerializeField] GameData data;

        [SerializeField] Transform patrolSpotA;
        [SerializeField] Transform patrolSpotB;

        public Vector3 patrolSpotAPosition
        {
            get
            {
                return patrolSpotA.position;
            }
        }
        public Vector3 patrolSpotBPosition
        {
            get
            {
                return patrolSpotB.position;
            }
        }

        public bool hasTarget
        {
            get 
            {
                if (target != null)
                    return true;
                else 
                    return false;
            }
        }

        public GameObject target;
        public GameObject Target
        {
            get { return target; }
        }

        private void Awake()
        {
            sensor = GetComponent<Sensor>();
            ai = GetComponent<BehaviourTreeOwner>();
        }

        // Update is called once per frame
        void Update()
        {
            if (sensor.DetectedObjects.Count != 0)
            {
                if (!data.hiding)
                    target = sensor.DetectedObjects[0];
            }
            else
                target = null;

            //if (hasTarget)
            //{
            //    ChangeDirection(target.transform.position);
            //}
            //else
            //{

            //}
        
        }

        void ChangeDirection(Vector3 targetPosition)
        {
            if (transform.position.x < targetPosition.x)
            {
                Vector3 scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
            }
            else
            {
                Vector3 scale = transform.localScale;
                scale.x = 1;
                transform.localScale = scale;
            }
        }
    }
}
