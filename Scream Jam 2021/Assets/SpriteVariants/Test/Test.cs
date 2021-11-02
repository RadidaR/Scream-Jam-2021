using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public GameObject variant;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, new Vector2(.5f, .5f));

            if (hit)
            {
                SpriteVariants variantGenerator = hit.transform.gameObject.GetComponent<SpriteVariants>();

                GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (GameObject go in allObjects)
                    if (go.activeInHierarchy && go.transform.name.Contains("Clone"))
                        Destroy(go);

                //Varied Regions
                List<SpriteVariants.ColorRegion> varied = variantGenerator.GetVariedRegions();

                //Custom Variants - Hue
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        varied[0].gen_targetHue = j/8f;
                        varied[0].gen_saturationMod = 0;
                        varied[0].gen_intensityMod = 0;

                        Texture2D variantTex = variantGenerator.GenerateVariant();
                        Sprite nSprite = Sprite.Create(variantTex, new Rect(0, 0, variantTex.width, variantTex.height), new Vector2(.5f, .5f));

                        GameObject variantObj = Instantiate(variant, new Vector3(j * 1f - 0.65f, i * 1f + 0.5f, 0), Quaternion.identity);
                        variantObj.GetComponent<SpriteRenderer>().sprite = nSprite;
                        variantObj.transform.localScale = new Vector3(5, 5, 5);
                    }
                }

                //Custom Variants - Saturation
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        varied[0].gen_targetHue = varied[0].average_hsv.h;
                        varied[0].gen_saturationMod = j/8f*2-1;
                        varied[0].gen_intensityMod = 0;

                        Texture2D variantTex = variantGenerator.GenerateVariant();
                        Sprite nSprite = Sprite.Create(variantTex, new Rect(0, 0, variantTex.width, variantTex.height), new Vector2(.5f, .5f));

                        GameObject variantObj = Instantiate(variant, new Vector3(j * 1f - 0.65f, i * 1f - 0.5f, 0), Quaternion.identity);
                        variantObj.GetComponent<SpriteRenderer>().sprite = nSprite;
                        variantObj.transform.localScale = new Vector3(5, 5, 5);
                    }
                }

                //Custom Variants - Value
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        varied[0].gen_targetHue = varied[0].average_hsv.h;
                        varied[0].gen_saturationMod = 0;
                        varied[0].gen_intensityMod = j/8f*2-1;

                        Texture2D variantTex = variantGenerator.GenerateVariant();
                        Sprite nSprite = Sprite.Create(variantTex, new Rect(0, 0, variantTex.width, variantTex.height), new Vector2(.5f, .5f));

                        GameObject variantObj = Instantiate(variant, new Vector3(j * 1f - 0.65f, i * 1f - 1.5f, 0), Quaternion.identity);
                        variantObj.GetComponent<SpriteRenderer>().sprite = nSprite;
                        variantObj.transform.localScale = new Vector3(5, 5, 5);
                    }
                }

                //Random Variants
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Texture2D variantTex = variantGenerator.GenerateRandomVariant(true, true, true);
                        Sprite nSprite = Sprite.Create(variantTex, new Rect(0, 0, variantTex.width, variantTex.height), new Vector2(.5f, .5f));

                        GameObject variantObj = Instantiate(variant, new Vector3(j * 1f - 0.65f, i * 1f - 2.5f, 0), Quaternion.identity);
                        variantObj.GetComponent<SpriteRenderer>().sprite = nSprite;
                        variantObj.transform.localScale = new Vector3(5, 5, 5);
                    }
                }

                //Bounded Random Variants
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Texture2D variantTex = variantGenerator.GenerateRandomVariantBounded(true, true, true);
                        Sprite nSprite = Sprite.Create(variantTex, new Rect(0, 0, variantTex.width, variantTex.height), new Vector2(.5f, .5f));

                        GameObject variantObj = Instantiate(variant, new Vector3(j * 1f - 0.65f, i * 1f - 3.5f, 0), Quaternion.identity);
                        variantObj.GetComponent<SpriteRenderer>().sprite = nSprite;
                        variantObj.transform.localScale = new Vector3(5, 5, 5);
                    }
                }
            }
        }
    }
}
