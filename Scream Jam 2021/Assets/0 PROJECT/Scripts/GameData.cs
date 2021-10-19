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
        [VerticalGroup("Booleans/Split/Right")]
        [LabelWidth(80)]
        public bool usingStair;
        [VerticalGroup("Booleans/Split/Right")]
        [LabelWidth(80)]
        public bool hiding;
        [VerticalGroup("Booleans/Split/Right")]
        [LabelWidth(80)]
        public bool maxSpeed;

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

        [FoldoutGroup("Stairs")]
        [LabelWidth(100)]
        public float getToStairTime;
    }
}
