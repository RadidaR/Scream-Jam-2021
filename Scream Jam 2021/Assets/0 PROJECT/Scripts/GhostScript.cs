using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;
//using ParadoxNotion;
using NodeCanvas.BehaviourTrees;
using MEC;
using UnityEngine.Experimental.Rendering.Universal;

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
        [SerializeField] BoxCollider2D stabZone;
        Light2D visionLight;
        [SerializeField] Transform eyeLevel;
        float visionLightLength;

        int innerOutlineID;
        int glowDissolveID;

        bool destroying;
        [SerializeField] float destructionTime;


        //int addColorID;
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
        [SerializeField] GameEvent eUpdate;
        [SerializeField] GameEvent eGhostDestroyed;
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
            //addColorID = Shader.PropertyToID("_AddColorFade");
            sprite.material = new Material(sprite.material);
            innerOutlineID = Shader.PropertyToID("_InnerOutlineFade");
            glowDissolveID = Shader.PropertyToID("_FullGlowDissolveFade");
            visionLight = GetComponentInChildren<Light2D>();
            visionLightLength = visionLight.pointLightOuterRadius;

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
                //sprite.material.SetFloat(addColorID, 1);
            }
            else if (lostTarget)
                anim.Play($"{type}_Ghost_Idle");
            else
            {
                anim.Play($"{type}_Ghost_Scouting");
                //sprite.material.SetFloat(addColorID, 0);
            }

            if (!destroying)
            {
                if (Physics2D.OverlapBox(stabZone.gameObject.transform.position/* + new Vector3(stabZone.offset.x, stabZone.offset.y, 0)*/, stabZone.size, 0, data.playerLayerMask))
                    sprite.material.SetFloat(innerOutlineID, 1);
                else
                    sprite.material.SetFloat(innerOutlineID, 0);
            }

            RaycastHit2D wallAhead = Physics2D.Raycast(eyeLevel.position, Vector2.right * transform.localScale.x, visionLightLength, data.groundLayerMask);
            if (wallAhead)
                visionLight.pointLightOuterRadius = wallAhead.distance;
            else
                visionLight.pointLightOuterRadius = visionLightLength;
        }

        public void DestroyGhost()
        {
            Timing.RunCoroutine(_GetDestroyed().CancelWith(gameObject), Segment.Update);
        }

        IEnumerator<float> _GetDestroyed()
        {
            destroying = true;
            sensor.enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            visionLight.enabled = false;
            eGhostDestroyed.Raise();

            yield return Timing.WaitForOneFrame;
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
            sprite.material.SetFloat(innerOutlineID, 0);

            foreach (SpriteRenderer renderer in sprites)
            {
                renderer.material = sprite.material;
            }

            float timer = 0;

            while (timer < destructionTime)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime);
                timer += Time.deltaTime;
                sprite.material.SetFloat(glowDissolveID, Mathf.Lerp(1, 0, timer / destructionTime));
            }

            eUpdate.Raise();
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
