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
        [FoldoutGroup("Movement")]
        [LabelWidth(80)]
        public float velocity;
        [FoldoutGroup("Movement")]
        [LabelText("Acceleration")]
        [LabelWidth(80)]
        public AnimationCurve accelerationCurve;
        [FoldoutGroup("Movement")]
        [LabelText("Decceleration")]
        [LabelWidth(80)]
        public AnimationCurve deccelerationCurve;

        [FoldoutGroup("Booleans")]
        [HorizontalGroup("Booleans/Split")]
        [VerticalGroup("Booleans/Split/Left")]
        [LabelWidth(80)]
        public bool canGoUp;
        [VerticalGroup("Booleans/Split/Left")]
        [LabelWidth(80)]
        public bool canGoDown;
        [VerticalGroup("Booleans/Split/Left")]
        [LabelWidth(80)]
        public bool canHide;
        [VerticalGroup("Booleans/Split/Left")]
        [LabelWidth(80)]
        public bool canStab;
        [VerticalGroup("Booleans/Split/Right")]
        [LabelWidth(80)]
        public bool usingStair;
        [VerticalGroup("Booleans/Split/Right")]
        [LabelWidth(80)]
        public bool maxSpeed;
        [VerticalGroup("Booleans/Split/Right")]
        [LabelWidth(80)]
        public bool hiding;
        [VerticalGroup("Booleans/Split/Right")]
        [LabelWidth(80)]
        public bool stabbing;

        [FoldoutGroup("Layers")]
        [HorizontalGroup("Layers/Split")]
        [VerticalGroup("Layers/Split/Left")]
        [LabelText("Ground")]
        [LabelWidth(75)]
        public int groundLayer;
        [VerticalGroup("Layers/Split/Left")]
        [LabelText("Player")]
        [LabelWidth(75)]
        public int playerLayer;
        [VerticalGroup("Layers/Split/Left")]
        [LabelText("Stair")]
        [LabelWidth(75)]
        public int stairLayer;
        [VerticalGroup("Layers/Split/Left")]
        [LabelText("Hide")]
        [LabelWidth(75)]
        public int hideLayer;
        [VerticalGroup("Layers/Split/Left")]
        [LabelText("Item")]
        [LabelWidth(75)]
        public int itemLayer;
        [VerticalGroup("Layers/Split/Left")]
        [LabelText("Ghost")]
        [LabelWidth(75)]
        public int ghostLayer;
        [VerticalGroup("Layers/Split/Left")]
        [LabelText("Stab")]
        [LabelWidth(75)]
        public int stabLayer;
        [HideLabel]
        [VerticalGroup("Layers/Split/Right")]
        public LayerMask groundLayerMask;
        [HideLabel]
        [VerticalGroup("Layers/Split/Right")]
        public LayerMask playerLayerMask;
        [HideLabel]
        [VerticalGroup("Layers/Split/Right")]
        public LayerMask stairLayerMask;
        [HideLabel]
        [VerticalGroup("Layers/Split/Right")]
        public LayerMask hideLayerMask;
        [HideLabel]
        [VerticalGroup("Layers/Split/Right")]
        public LayerMask itemLayerMask;
        [HideLabel]
        [VerticalGroup("Layers/Split/Right")]
        public LayerMask ghostLayerMask;
        [HideLabel]
        [VerticalGroup("Layers/Split/Right")]
        public LayerMask stabLayerMask;

        [FoldoutGroup("Stairs")]
        [LabelWidth(100)]
        public float getToStairTime;

        public void ResetValues()
        {
            canGoUp = false;
            canGoDown = false;
            canHide = false;
            canStab = false;
            usingStair = false;
            maxSpeed = false;
            hiding = false;
            stabbing = false;
        }
    }
}
