using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ScreamJam
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Game/Data")]
    [InlineEditor]
    public class GameData : ScriptableObject
    {
        public float velocity;
        public AnimationCurve accelerationCurve;
        public AnimationCurve deccelerationCurve;

        public bool canGoUp;
        public bool canGoDown;
        public bool canHide;
        public bool usingStair;
        public bool hiding;
        public bool maxSpeed;

        public int playerLayer;
        public int stairLayer;
        public int hideLayer;
        public int itemLayer;
        public LayerMask playerLayerMask;
        public LayerMask stairLayerMask;
        public LayerMask hideLayerMask;
        public LayerMask itemLayerMask;

        public float getToStairTime;
        public float useStairTime;
    }
}
