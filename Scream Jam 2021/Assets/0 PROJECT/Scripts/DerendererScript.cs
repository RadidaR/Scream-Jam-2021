using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Sirenix.OdinInspector;

namespace ScreamJam
{
    public class DerendererScript : MonoBehaviour
    {
        Light2D[] lights;
        SpriteRenderer[] sprites;
        ShadowCaster2D[] shadowCasters;
        [SerializeField] float spriteRenderRange;
        [SerializeField] GameData data;

        private void Awake()
        {
            lights = GetComponentsInChildren<Light2D>();
            sprites = GetComponentsInChildren<SpriteRenderer>();
            shadowCasters = GetComponentsInChildren<ShadowCaster2D>();
            InvokeRepeating("RenderBoth", 0, 0.25f);
        }

        void RenderBoth()
        {
            RenderSprites();
            RenderLights();
        }


        //private void Update()
        //{
        //    CheckRender();
        //}

        void RenderSprites()
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                if (sprite != null)
                {
                    if (!Physics2D.OverlapCircle(sprite.gameObject.transform.position, spriteRenderRange, data.renderLayerMask))
                    {
                        sprite.enabled = false;
                    }
                    else
                    {
                        sprite.enabled = true;
                    }
                }
            }
        }

        void RenderLights()
        {
            foreach (Light2D light in lights)
            {
                if (light != null)
                {
                    if (light.GetComponentInParent<GhostScript>() != null)
                    {
                        if (!light.GetComponentInParent<GhostScript>().destroying)
                        {
                            if (light.GetComponentInParent<GhostScript>().gameObject.GetComponentInChildren<SpriteRenderer>().enabled)
                            {
                                light.enabled = true;
                            }
                            else
                            {
                                light.enabled = false;
                            }
                        }
                        else
                        {
                            light.enabled = false;
                        }
                    }
                    else
                    {
                        if (!Physics2D.OverlapCircle(light.gameObject.transform.position, light.pointLightOuterRadius, data.renderLayerMask))
                        {
                            light.enabled = false;
                        }
                        else
                        {
                            light.enabled = true;
                        }
                    }
                }
            }
        }

        //void CheckRender()
        //{
        //    foreach (Light2D light in lights)
        //    {
        //        if (light != null)
        //        {
        //            if (!Physics2D.OverlapCircle(light.gameObject.transform.position, light.pointLightOuterRadius, data.renderLayerMask))
        //            {
        //                light.enabled = false;
        //            }
        //            else
        //            {
        //                light.enabled = true;
        //            }
        //        }
        //    }

        //    foreach (SpriteRenderer sprite in sprites)
        //    {
        //        if (sprite != null)
        //        {
        //            if (!Physics2D.OverlapCircle(sprite.gameObject.transform.position, 5f, data.renderLayerMask))
        //            {
        //                sprite.enabled = false;
        //            }
        //            else
        //            {
        //                sprite.enabled = true;
        //            }
        //        }
        //    }
        //}

    }
}
