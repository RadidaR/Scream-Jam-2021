using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace ScreamJam
{
    public class StairMaterialScript : MonoBehaviour
    {
        [SerializeField] GameData data;

        [SerializeField] SpriteRenderer sprite;
        Material material;

        int innerOutlineID;
        //oat innerOutlineIDFadeValue;

        [SerializeField] float midValue;
        [SerializeField] float maxValue;
        [SerializeField] float time;

        private void Awake()
        {
            sprite.material = new Material(sprite.material);
            material = sprite.material;
            innerOutlineID = Shader.PropertyToID("_InnerOutlineFade");
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision.gameObject.layer == data.playerLayer)
        //        Timing.RunCoroutine(_InnerOutline(), Segment.Update);
        //}
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == data.playerLayer)
                Timing.RunCoroutine(_InnerOutline(), Segment.Update);
        }

        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.gameObject.layer == data.playerLayer)
        //        Timing.RunCoroutine(_InnerOutlineReverse(), Segment.Update);
        //}
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == data.playerLayer)
                Timing.RunCoroutine(_InnerOutlineReverse(), Segment.Update);
        }

        IEnumerator<float> _InnerOutline()
        {

            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                float innerOutlineFadeValue = Mathf.Lerp(0, midValue, timer / time);
                material.SetFloat(innerOutlineID, innerOutlineFadeValue);
                yield return Timing.WaitForSeconds(Time.deltaTime);
            }
        }
        IEnumerator<float> _InnerOutlineReverse()
        {
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                float innerOutlineFadeValue = Mathf.Lerp(midValue, 0, timer / time);
                material.SetFloat(innerOutlineID, innerOutlineFadeValue);
                yield return Timing.WaitForSeconds(Time.deltaTime);
            }
        }


    }
}
