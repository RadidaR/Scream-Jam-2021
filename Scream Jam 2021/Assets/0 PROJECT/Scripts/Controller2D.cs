using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace ScreamJam
{
    public class Controller2D : MonoBehaviour
    {
        [SerializeField] GameData data;
        PlayerInput player;
        BoxCollider2D boxCollider;

        float accelerationTime;
        float deccelerationTime;

        [SerializeField] GameEvent eClimb;
        [SerializeField] GameEvent eDrop;
        [SerializeField] GameEvent eReachTop;
        [SerializeField] GameEvent eReachBottom;

        [SerializeField] Transform exorciseSpot;
        [SerializeField] float exorciseRadius;

        bool canMove()
        {
            if (!data.usingStair && !data.stabbing)
                return true;
            else
                return false;
        }

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            player = new PlayerInput();

            player.input.UseStairs.performed += ctx => CheckForStairs();
            player.input.UseCross.performed += ctx => CheckForPossessions();

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

            
        }

        void Move()
        {
            if (!canMove())
                return;

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

            Vector3 scale = transform.localScale;
            scale.x = 1 * Mathf.Sign(player.input.Move.ReadValue<float>());
            transform.localScale = scale;
        }

        void Stop()
        {
            if (!canMove())
                return;

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
        }

        void CheckForPossessions()
        {
            GameObject possessedItem = Physics2D.OverlapCircle(exorciseSpot.position, exorciseRadius, data.itemLayerMask).gameObject;
            if (possessedItem != null)
            {
                possessedItem.GetComponent<PossessedItem>().EndPossession();
            }
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
                data.canHide = true;
            }
            else if (collision.gameObject.layer == data.ghostLayer)
            {
                Debug.Log("Caught by ghost");
            }
            else if (collision.gameObject.layer == data.stabLayer)
            {
                data.canStab = true;
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
                data.canHide = true;
            }
            else if (collision.gameObject.layer == data.ghostLayer)
            {
                Debug.Log("Caught by ghost");
            }
            else if (collision.gameObject.layer == data.stabLayer)
            {
                data.canStab = true;
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

            if (exorciseRadius > 0)
                Gizmos.DrawWireSphere(exorciseSpot.position, exorciseRadius);
        }
    }
}
