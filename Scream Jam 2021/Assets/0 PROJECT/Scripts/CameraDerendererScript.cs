using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScreamJam
{
    public class CameraDerendererScript : MonoBehaviour
    {
        [SerializeField] Camera _camera;
        [SerializeField] BoxCollider2D _renderZone;

        private void Awake()
        {
            Vector2 renderSize = _renderZone.size;
            renderSize.x = 4f * _camera.orthographicSize;
            renderSize.y = 3 * _camera.orthographicSize;
            _renderZone.size = renderSize;
        }
    }
}
