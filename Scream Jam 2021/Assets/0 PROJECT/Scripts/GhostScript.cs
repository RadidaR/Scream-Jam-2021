using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;
//using ParadoxNotion;
using NodeCanvas.BehaviourTrees;
using MEC;

namespace ScreamJam
{
    public enum GhostState
    {
        Patrolling,
        Chasing,
        LostTarget
    }

    public class GhostScript : MonoBehaviour
    {
        public string type;
        Animator anim;

        [SerializeField] SpriteRenderer sprite;
        int addColorID;
        //BehaviourTreeOwner ai;
        //NodeCanvas.BehaviourTrees
        Sensor sensor;

        public GhostState state;
        public GhostState State
        {
            get { return state; }
            set { state = value; }
        }

        //[SerializeField] bool hasSpottedPlayer;
        [SerializeField] GameData data;
        [SerializeField] Transform spotA;
        [SerializeField] Transform spotB;
        GameObject player;

        public Transform PatrolSpotA { get { return spotA; } set { } }
        public Transform PatrolSpotB { get { return spotB; } set { } }

        public float PatrolSpotAx { get { return spotA.position.x; } }
        public float PatrolSpotBx { get { return spotB.position.x; } }

        public float xPosition { get { return transform.position.x; } }

        [SerializeField] bool goingToA;

        public bool GoingToA { get { return goingToA; } set { goingToA = value; } }


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

        public bool lostTarget;
        public bool LostTarget
        {
            get
            {
                return lostTarget;
            }

            set
            {
                lostTarget = value;
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
            //mat = GetComponent<Material>();
            addColorID = Shader.PropertyToID("_AddColorFade");
            sprite.material = new Material(sprite.material);

            sensor = GetComponent<Sensor>();
            anim = GetComponentInChildren<Animator>();
            spotA.parent = null;
            spotB.parent = null;
            player = FindObjectOfType<Controller2D>().gameObject;
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

            if (state == GhostState.Chasing)
            {
                if (target != null)
                    ChangeDirection(target.transform.position);
            }
            else if (state == GhostState.LostTarget)
            {
                if (player != null)
                    ChangeDirection(player.transform.position);
            }
            else
            {
                    if (goingToA)
                        ChangeDirection(spotA.position);
                    else
                        ChangeDirection(spotB.position);
            }

            if (hasTarget)
            {
                anim.Play($"{type}_Ghost_Attack");
                sprite.material.SetFloat(addColorID, 1);
            }
            else if (lostTarget)
                anim.Play($"{type}_Ghost_Idle");
            else
            {
                anim.Play($"{type}_Ghost_Scouting");
                sprite.material.SetFloat(addColorID, 0);
            }


        }

        public void DestroyGhost()
        {
            Timing.RunCoroutine(_GetDestroyed(), Segment.FixedUpdate);
        }

        IEnumerator<float> _GetDestroyed()
        {
            yield return Timing.WaitForOneFrame;
            Destroy(gameObject, 0);
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

        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawWireSphere(spotA.position, 2);
        //    Gizmos.DrawWireSphere(spotB.position, 2);
        //}
    }
}
