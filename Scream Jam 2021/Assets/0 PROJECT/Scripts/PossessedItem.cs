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

        public bool possessed = true;

        public void EndPossession()
        {
            possessedFeedbacks.Stop(transform.position, 1);
            positionFeedback.Stop(transform.position, 1);
            rotationFeedback.Stop(transform.position, 1);
            loop.Stop(transform.position, 1);
            possessed = false;
            endPossessionFeedbacks.Play(transform.position, 1);
        }
    }
}
