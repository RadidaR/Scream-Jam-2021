using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MEC;

namespace ScreamJam
{
    public class PossessedItem : MonoBehaviour
    {
        [SerializeField] GameData data;
        [SerializeField] GameEvent eUpdate;
        [SerializeField] MMFeedback possessedFeedbacks;
        [SerializeField] MMFeedbackPosition positionFeedback;
        [SerializeField] MMFeedbackRotation rotationFeedback;
        [SerializeField] MMFeedbackLooper loop;
        [SerializeField] MMFeedback endPossessionFeedbacks;
        [SerializeField] MMFeedbackPosition endPositionFeedback;

        [SerializeField] SpriteRenderer sprite;
        [SerializeField] SpriteRenderer possessionEffect;

        public bool possessed = true;

        private void Awake()
        {
            possessionEffect.sprite = sprite.sprite;
        }

        public void EndPossession()
        {
            if (!possessed)
                return;

            possessedFeedbacks.Stop(transform.position, 1);
            positionFeedback.Stop(transform.position, 1);
            rotationFeedback.Stop(transform.position, 1);
            loop.Stop(transform.position, 1);
            possessed = false;

            RaycastHit2D ground = Physics2D.Raycast(transform.position, Vector2.down, 100, data.groundLayerMask);

            if (ground)
                endPositionFeedback.DestinationPosition.y = -ground.distance;

            endPossessionFeedbacks.Play(transform.position, 1);
            eUpdate.Raise();
            Timing.RunCoroutine(_RemoveEffect().CancelWith(possessionEffect.gameObject), Segment.Update);
        }

        IEnumerator<float> _RemoveEffect()
        {
            while (possessionEffect.gameObject.transform.localScale.x > 0.75f)
            {
                yield return Timing.WaitForSeconds(Time.deltaTime / 4);
                Vector3 scale = possessionEffect.gameObject.transform.localScale;
                scale.x -= Time.deltaTime / 4;
                scale.y -= Time.deltaTime / 4;
                possessionEffect.gameObject.transform.localScale = scale;
                if (possessionEffect.gameObject.transform.localScale.x <= 0.75f)
                    break;
            }

            possessionEffect.enabled = false;
        }
    }
}
