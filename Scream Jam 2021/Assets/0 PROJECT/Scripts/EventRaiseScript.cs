using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScreamJam
{
    public class EventRaiseScript : MonoBehaviour
    {
        [SerializeField] GameEvent _event;

        public void RaiseEvent()
        {
            _event.Raise();
        }
    }
}
