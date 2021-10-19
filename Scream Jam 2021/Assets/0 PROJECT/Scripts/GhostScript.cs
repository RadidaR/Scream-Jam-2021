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
        //BehaviourTreeOwner ai;
        //NodeCanvas.BehaviourTrees
        Sensor sensor;
        //[SerializeField] bool hasSpottedPlayer;
        [SerializeField] GameData data;
        [SerializeField] Transform spotA;
        [SerializeField] Transform spotB;

        public Transform PatrolSpotA { get { return spotA; } set { } }
        public Transform PatrolSpotB { get { return spotB; } set { } }

        public float PatrolSpotAx { get { return spotA.position.x; } }
        public float PatrolSpotBx { get { return spotB.position.x; } }

        public float xPosition { get { return transform.position.x; } }

        [SerializeField] bool goingToA;

        public bool GoingToA { get { return goingToA; } set { goingToA = value; } }

        //public Vector3 patrolSpotAPosition {
        //    get
        //    {
        //        return patrolSpotA.position;
        //    }
        //}
        //public Vector3 patrolSpotBPosition
        //{
        //    get
        //    {
        //        return patrolSpotB.position;
        //    }
        //}

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
            set { target = value; }
        }

        private void Awake()
        {
            sensor = GetComponent<Sensor>();
            spotA.parent = null;
            spotB.parent = null;
        }

        // Update is called once per frame
        void Update()
        {
            if (sensor.DetectedObjects.Count != 0)
            {
                if (!data.hiding)
                    Target = sensor.DetectedObjects[0];
            }
            else
                Target = null;

            if (hasTarget)
            {
                ChangeDirection(target.transform.position);
            }
            else
            {
                if (goingToA)
                    ChangeDirection(spotA.position);
                else
                    ChangeDirection(spotB.position);
            }




        }

        void ChangeDirection(Vector3 targetPosition)
        {
            if (xPosition > targetPosition.x)
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spotA.position, 2);
            Gizmos.DrawWireSphere(spotB.position, 2);
        }
    }
}
