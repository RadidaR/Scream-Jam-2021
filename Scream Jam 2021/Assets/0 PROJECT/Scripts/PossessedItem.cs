using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace ScreamJam
{
    public class PossessedItem : MonoBehaviour
    {
        [SerializeField] MMFeedback possessedFeedbacks;
        [SerializeField] MMFeedbackPosition positionFeedback;
        [SerializeField] MMFeedbackRotation rotationFeedback;
        [SerializeField] MMFeedbackLooper loop;
        [SerializeField] MMFeedback endPossessionFeedbacks;
        [SerializeField] MMFeedbackPosition endPositionFeedback;

        public bool possessed = true;

        public void EndPossession()
        {
            if (!possessed)
                return;

            possessedFeedbacks.Stop(transform.position, 1);
            positionFeedback.Stop(transform.position, 1);
            rotationFeedback.Stop(transform.position, 1);
            loop.Stop(transform.position, 1);
            possessed = false;

            RaycastHit2D ground = Physics2D.Raycast(transform.position, Vector2.down);

            if (ground)
                endPositionFeedback.DestinationPosition.y = -ground.distance;

            endPossessionFeedbacks.Play(transform.position, 1);
        }
    }
}
