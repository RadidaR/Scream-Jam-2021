using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using Sirenix.OdinInspector;

namespace ScreamJam
{
    public class Controller2D : MonoBehaviour
    {
        [SerializeField] GameData data;
        PlayerInput player;
        BoxCollider2D boxCollider;
        Animator anim;


        [FoldoutGroup("Animations")]
        [SerializeField] string currentAnimState;
        [FoldoutGroup("Animations")]
        [SerializeField] string idleAnim;
        [FoldoutGroup("Animations")]
        [SerializeField] string walkAnim;
        [FoldoutGroup("Animations")]
        [SerializeField] string exorAnim;
        [FoldoutGroup("Animations")]
        [SerializeField] string attackAnim;
        [FoldoutGroup("Animations")]
        [SerializeField] string deadAnim;

        float accelerationTime;
        float deccelerationTime;

        [FoldoutGroup("Events")]
        [SerializeField] GameEvent eClimb;
        [FoldoutGroup("Events")]
        [SerializeField] GameEvent eDrop;
        [FoldoutGroup("Events")]
        [SerializeField] GameEvent eReachTop;
        [FoldoutGroup("Events")]
        [SerializeField] GameEvent eReachBottom;
        [FoldoutGroup("Events")]
        [SerializeField] GameEvent eHide;
        [FoldoutGroup("Events")]
        [SerializeField] GameEvent eReveal;
        [FoldoutGroup("Events")]
        [SerializeField] GameEvent eCaught;

        [FoldoutGroup("Transforms")]
        [SerializeField] Transform exorciseSpot;
        [FoldoutGroup("Transforms")]
        [SerializeField] Transform face;

        bool canMove()
        {
            if (!data.usingStair && !data.stabbing && !data.hiding && !data.dead)
                return true;
            else
                return false;
        }

        RaycastHit2D wallAhead()
        {
            return Physics2D.Raycast(face.position, Vector2.right * Mathf.Sign(transform.localScale.x), 2, data.groundLayerMask);
        }

        
        RaycastHit2D groundBelow()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, 10, data.groundLayerMask);
        }



        public bool canExorcise;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            anim = GetComponentInChildren<Animator>();
            player = new PlayerInput();

            player.input.UseStairs.performed += ctx => CheckForStairs();
            player.input.UseCross.performed += ctx => Exorcise();
            player.input.Hide.performed += ctx => HideAndReveal();

            data.ResetValues();
        }

        private void FixedUpdate()
        {
            if (player.input.Move.ReadValue<float>() != 0)
            {
                deccelerationTime = 0;
                Move();
            }
            else
            {
                accelerationTime = 0;
                if (data.velocity != 0)
                    Stop();
            }

            if (data.inSight)
                data.canHide = false;

            if (canMove())
                Timing.RunCoroutine(_GroundCharacter(), Segment.FixedUpdate);
        }

        void Update()
        {
            data.canExorcise = Physics2D.OverlapCircle(exorciseSpot.position, data.exorciseRadius, data.itemLayerMask);
        }

        IEnumerator<float> _GroundCharacter()
        {
            while (groundBelow().distance > 0.5f)
            {
                yield return Timing.WaitForSeconds(Time.fixedDeltaTime);
                Vector3 pos = transform.position;
                pos.y -= Time.fixedDeltaTime;
                transform.position = pos;

                if (groundBelow().distance <= 0.5f)
                    break;
            }
        }

        void Move()
        {
            if (!canMove())
                return;

            Vector3 scale = transform.localScale;
            scale.x = 1 * Mathf.Sign(player.input.Move.ReadValue<float>());
            transform.localScale = scale;

            //RaycastHit2D ray = Physics2D.Raycast(face.position, Vector2.right * Mathf.Sign(player.input.Move.ReadValue<float>()), 1.5f, data.groundLayerMask);

            if (wallAhead())
                return;

            //if (currentAnimState != exorAnim)
                ChangeAnimationState(walkAnim);

            if (Mathf.Sign(data.velocity) != Mathf.Sign(player.input.Move.ReadValue<float>()))
                data.velocity = 0;

            if (Mathf.Abs(data.velocity) < data.accelerationCurve.keys[data.accelerationCurve.keys.Length - 1].value)
            {
                data.maxSpeed = false;
                data.velocity = data.accelerationCurve.Evaluate(accelerationTime) * player.input.Move.ReadValue<float>();
                accelerationTime += Time.fixedDeltaTime;
            }
            else
            {
                data.maxSpeed = true;
                data.velocity = data.accelerationCurve.keys[data.accelerationCurve.keys.Length - 1].value * player.input.Move.ReadValue<float>();
                accelerationTime = 0;
            }

            Vector2 position = transform.position;
            position.x += data.velocity;
            transform.position = position;

            
        }

        void Stop()
        {
            if (!canMove())
                return;

            //RaycastHit2D ray = Physics2D.Raycast(face.position, Vector2.right * Mathf.Sign(transform.localScale.x), 1.5f, data.groundLayerMask);

            if (wallAhead())
            {
                data.velocity = 0;
                data.maxSpeed = false;
                ChangeAnimationState(idleAnim);
                return;
            }

            if (data.maxSpeed)
            {
                if (Mathf.Abs(data.velocity) > 0)
                {
                    data.velocity = data.deccelerationCurve.Evaluate(deccelerationTime) * Mathf.Sign(data.velocity);
                    deccelerationTime += Time.fixedDeltaTime;

                    Vector2 position = transform.position;
                    position.x += data.velocity;
                    transform.position = position;

                    if (data.velocity == 0)
                        data.maxSpeed = false;
                }
                else
                {
                    deccelerationTime = 0;
                    //data.maxSpeed = false;
                }
            }
            else
            {
                data.velocity = Mathf.Lerp(data.accelerationCurve.keys[data.accelerationCurve.keys.Length - 1].value / 2, 0, deccelerationTime / (data.deccelerationCurve.keys[data.accelerationCurve.keys.Length - 1].time / 2)) * Mathf.Sign(data.velocity);
                deccelerationTime += Time.fixedDeltaTime;

                Vector2 position = transform.position;
                position.x += data.velocity;
                transform.position = position;
            }

            if (currentAnimState != exorAnim && Mathf.Abs(data.velocity) < 0.2f)
                ChangeAnimationState(idleAnim);
        }

        void CheckForStairs()
        {
            if ((!data.canGoUp && !data.canGoDown) || data.usingStair)
                return;

            Vector2 colliderPosition = transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0);

            Collider2D stairCollider = Physics2D.OverlapBox(colliderPosition, boxCollider.size, 0, data.stairLayerMask);
            Transform stairs = stairCollider.gameObject.transform.parent;

            if (stairCollider != null)
            {
                Timing.RunCoroutine(_UseStairs(stairs), Segment.FixedUpdate);
            }


        }

        IEnumerator<float> _UseStairs(Transform stairs)
        {
            data.usingStair = true;
            Transform upper = stairs.GetChild(0);
            Transform lower = stairs.GetChild(1);
            Vector3 stairPosition = stairs.position;
            yield return Timing.WaitForOneFrame;

            if (data.canGoUp)
                stairPosition = lower.position;
            else if (data.canGoDown)
                stairPosition = upper.position;

            ChangeAnimationState(walkAnim);

            float timer = 0;
            Vector3 originalPosition = transform.position;
            while (timer < data.getToStairTime)
            {
                yield return Timing.WaitForSeconds(Time.fixedDeltaTime);
                timer += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(originalPosition, stairPosition, timer / data.getToStairTime);
                if (timer >= data.getToStairTime || transform.position == stairPosition)
                    break;
            }

            bool goingUp = false;
            if (data.canGoUp)
            {
                goingUp = true;
                eClimb.Raise();
            }
            else if (data.canGoDown)
            {
                goingUp = false;
                eDrop.Raise();
            }

            yield return Timing.WaitForSeconds(0.35f);

            if (goingUp)
            {
                Vector3 newPos = upper.position;
                newPos.y -= 2;
                transform.position = newPos;
                eReachTop.Raise();
            }
            else
            {
                Vector3 newPos = lower.position;
                newPos.y += 8;
                transform.position = newPos;
                eReachBottom.Raise();
            }

            data.usingStair = false;

            ChangeAnimationState(idleAnim);
        }

        void Exorcise()
        {
            if (!canMove())
                return;

            if (data.canStab)
            {
                GameObject stabZone = null;

                if (Physics2D.OverlapCircle(transform.position, 1f, data.stabLayerMask))
                    stabZone = Physics2D.OverlapCircle(transform.position, 1f, data.stabLayerMask).gameObject;

                if (stabZone != null)
                {
                    if (!wallAhead())
                        Timing.RunCoroutine(_StabGhost(stabZone.gameObject), Segment.FixedUpdate);
                }
            }
            else
            {
                GameObject possessedItem = null;

                if (Physics2D.OverlapCircle(exorciseSpot.position, data.exorciseRadius, data.itemLayerMask))
                    possessedItem = Physics2D.OverlapCircle(exorciseSpot.position, data.exorciseRadius, data.itemLayerMask).gameObject;

                if (possessedItem != null)
                {
                    //data.velocity = 0;
                    ChangeAnimationState(exorAnim);
                    possessedItem.GetComponent<PossessedItem>().EndPossession();
                }
            }
        }

        IEnumerator<float> _StabGhost(GameObject stabPosition)
        {
            data.stabbing = true;

            ChangeAnimationState(attackAnim);

            float timer = 0;
            Vector3 originalPosition = transform.position;
            while (timer < data.getToGhostTime)
            {
                yield return Timing.WaitForSeconds(Time.fixedDeltaTime);
                timer += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(originalPosition, stabPosition.transform.position, timer / data.getToGhostTime);
                if (timer >= data.getToGhostTime || transform.position == stabPosition.transform.position)
                    break;
            }

            stabPosition.GetComponentInParent<GhostScript>().DestroyGhost();
            data.stabbing = false;
        }

        void HideAndReveal()
        {
            GameObject hidingSpot = null;
            if (Physics2D.OverlapBox(transform.position, boxCollider.size, 0, data.hideLayerMask))
                hidingSpot = Physics2D.OverlapBox(transform.position, boxCollider.size, 0, data.hideLayerMask).gameObject;

            if (!data.hiding)
            {
                if (!data.canHide)
                    return;                

                if (hidingSpot != null)
                    Timing.RunCoroutine(_Hide(hidingSpot), Segment.FixedUpdate);
            }
            else
            {
                //PLAY hidingSpot.GetComponent<Animator>().Play();
                hidingSpot.GetComponent<Animator>().Play($"{hidingSpot.transform.GetChild(0).gameObject.name}_Exit");
                data.hiding = false;
                eReveal.Raise();
            }
        }

        IEnumerator<float> _Hide(GameObject hidingSpot)
        {
            hidingSpot.GetComponent<Animator>().Play($"{hidingSpot.transform.GetChild(0).gameObject.name}_Enter");
            float timer = 0;
            Vector3 originalPosition = transform.position;
            while (timer < data.getToHidingSpotTime)
            {
                yield return Timing.WaitForSeconds(Time.fixedDeltaTime);
                timer += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(originalPosition, hidingSpot.transform.position, timer / data.getToHidingSpotTime);
                if (timer >= data.getToHidingSpotTime || transform.position == hidingSpot.transform.position)
                    break;
            }

            data.canHide = false;
            data.hiding = true;
            eHide.Raise();
        }

        void ChangeAnimationState(string newState)
        {
            if (newState == currentAnimState)
                return;

            currentAnimState = newState;
            anim.Play(newState);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == data.stairLayer)
            {
                if (collision.gameObject.tag == "Upper")
                {
                    data.canGoDown = true;
                }
                else if (collision.gameObject.tag == "Lower")
                {
                    data.canGoUp = true;
                }
            }
            else if (collision.gameObject.layer == data.hideLayer)
            {
                if (!data.inSight)
                    data.canHide = true;
            }
            else if (collision.gameObject.layer == data.ghostLayer)
            {
                if (!data.dead)
                {
                    ChangeAnimationState(deadAnim);
                    Timing.RunCoroutine(_Die(), Segment.Update);
                }
            }
            else if (collision.gameObject.layer == data.stabLayer)
            {
                data.canStab = true;
            }
            else if (collision.gameObject.layer == data.sightLayer)
            {
                if (!data.hiding)
                {
                    GhostScript[] ghosts = FindObjectsOfType<GhostScript>();

                    bool spotted = false;
                    foreach (GhostScript ghost in ghosts)
                    {
                        if (ghost.hasTarget)
                            spotted = true;
                    }

                    if (spotted)
                        data.inSight = true;
                }
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == data.stairLayer)
            {
                if (collision.gameObject.tag == "Upper")
                {
                    data.canGoDown = true;
                }
                else if (collision.gameObject.tag == "Lower")
                {
                    data.canGoUp = true;
                }
            }
            else if (collision.gameObject.layer == data.hideLayer)
            {
                if (!data.inSight)
                    data.canHide = true;
            }
            else if (collision.gameObject.layer == data.ghostLayer)
            {
                if (!data.dead)
                {
                    ChangeAnimationState(deadAnim);
                    Timing.RunCoroutine(_Die(), Segment.Update);
                }

            }
            else if (collision.gameObject.layer == data.stabLayer)
            {
                data.canStab = true;
            }
            else if (collision.gameObject.layer == data.sightLayer)
            {
                if (!data.hiding)
                {
                    GhostScript[] ghosts = FindObjectsOfType<GhostScript>();

                    bool spotted = false;
                    foreach (GhostScript ghost in ghosts)
                    {
                        if (ghost.hasTarget)
                            spotted = true;
                    }

                    if (spotted)
                        data.inSight = true;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == data.stairLayer)
            {
                data.canGoDown = false;
                data.canGoUp = false;
            }
            else if (collision.gameObject.layer == data.hideLayer)
            {
                data.canHide = false;
            }
            else if (collision.gameObject.layer == data.stabLayer)
            {
                data.canStab = false;
            }
            else if (collision.gameObject.layer == data.sightLayer)
            {
                data.inSight = false;
            }
        }     

        IEnumerator<float> _Die()
        {
            data.dead = true;
            yield return Timing.WaitForSeconds(data.deathTime);
            eCaught.Raise();
        }

        private void OnEnable()
        {
            player.Enable();
        }

        private void OnDisable()
        {
            player.Disable();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            if (data.exorciseRadius > 0)
                Gizmos.DrawWireSphere(exorciseSpot.position, data.exorciseRadius);
        }
    }
}
