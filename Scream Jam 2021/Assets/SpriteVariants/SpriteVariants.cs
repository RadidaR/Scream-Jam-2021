//using System.Collections;
//using System.Collections.Generic;
//using System;
//using UnityEngine;

//public class SpriteVariants : MonoBehaviour
//{

//    //Serializable saves it on the scene despite not showing on inspector due to complex structure

//    [Serializable]
//    public class PosInt
//    {
//        public int x;
//        public int y;
//        public PosInt(int x, int y)
//        {
//            this.x = x;
//            this.y = y;
//        }
//    }

//    [Serializable]
//    public class ColorRegion
//    {
//        //Region positions and preview
//        /// <summary>
//        /// INTERNAL - Pixel structure of this region
//        /// </summary>
//        public PixelMatrix canvas;
//        /// <summary>
//        /// INTERNAL - Pixel positions of this region
//        /// </summary>
//        public List<PosInt> positions = new List<PosInt>();
//        /// <summary>
//        /// INTERNAL - Original Texture of this region
//        /// </summary>
//        public Texture previewTexture;

//        //Average Color
//        /// <summary>
//        /// Average RGB color of region in original texture
//        /// </summary>
//        public Color average_rgb;
//        /// <summary>
//        /// Average HSV color of region in original texture
//        /// </summary>
//        public HSVRGB.HSV average_hsv;
//        /// <summary>
//        /// Average LAB color of region in original texture
//        /// </summary>
//        public LabColor average_lab;

//        //Vary region
//        /// <summary>
//        /// Mark region as varied
//        /// </summary>
//        public bool varied;

//        //HSV Variant
//        /// <summary>
//        /// Hue value of Variant [0,1]
//        /// </summary>
//        public float gen_targetHue = 0;
//        /// <summary>
//        /// Saturation additive of Variant [-1,1]
//        /// </summary>
//        public float gen_saturationMod = 0f;
//        /// <summary>
//        /// Saturation additive of Variant [-1,1]
//        /// </summary>
//        public float gen_intensityMod = 0f;
//    }

//    /// <summary>
//    /// Color Regions
//    /// </summary>
//    [HideInInspector]
//    public List<ColorRegion> regions = new List<ColorRegion>();

//    /// <summary>
//    /// Region Detection Color Space
//    /// 0 - HSV (Hue)
//    /// 1 - RGB
//    /// 2 - LAB
//    /// </summary>
//    [HideInInspector]
//    public int mode = 0;

//    /// <summary>
//    /// Region Detection Color Threshold
//    /// </summary>
//    [HideInInspector]
//    public float threshold = 0.15f;

//    /// <summary>
//    /// Returns the ColorRegion(s) that are flagged for variation
//    /// </summary>
//    public List<ColorRegion> GetVariedRegions()
//    {
//        List<ColorRegion> variedRegions = new List<ColorRegion>(regions.Count);
//        for (int i = 0; i < regions.Count; i++)
//        {
//            if (regions[i].varied)
//                variedRegions.Add(regions[i]);
//        }
//        return variedRegions;
//    }

//    /// <summary>
//    /// Returns a Texture2D variant with optional random Hue, Saturation and Value for each ColorRegion(s) flagged for variation.
//    /// Saturation and Value are applied as additives to the original color, bounded by the region minimum and maximum values of these components.
//    /// </summary>
//    public Texture2D GenerateRandomVariantBounded(bool randomHue, bool randomSaturation, bool randomValue)
//    {
//        Color blank = Color.clear;

//        //Load pixels (dynamic to sprite sheet pivot)
//        Sprite sprt = GetComponent<SpriteRenderer>().sprite;
//        Color[] px = sprt.texture.GetPixels((int)sprt.textureRect.min.x, (int)sprt.textureRect.min.y, (int)sprt.textureRect.width, (int)sprt.textureRect.height);
//        PixelMatrix pixels = new PixelMatrix((int)sprt.textureRect.width, (int)sprt.textureRect.height, px);

//        Texture2D variantTexture = new Texture2D(pixels.width, pixels.height);

//        foreach (var region in regions)
//        {
//            int positionCount = region.positions.Count;

//            //Check if Region is varied
//            if (!region.varied)
//            {
//                for (int j = 0; j < positionCount; j++)
//                {
//                    PosInt coordinates = region.positions[j];
//                    int x = coordinates.x;
//                    int y = coordinates.y;
//                    pixels.SetPixel(x, y, pixels.GetPixel(x, y));
//                }

//                continue;
//            }

//            //HSV values for each position (symmetric of region.positions)
//            List<HSVRGB.HSV> positionsHSV = new List<HSVRGB.HSV>(positionCount);

//            //Min/Max Saturation and Intensity (to know uniform modification range)
//            float minSat = 1;
//            float maxSat = 0;
//            float minVal = 1;
//            float maxVal = 0;

//            for (int j = 0; j < positionCount; j++)
//            {
//                PosInt coordinates = region.positions[j];
//                int x = coordinates.x;
//                int y = coordinates.y;

//                //RGB to HSV - Unity faster
//                HSVRGB.HSV hsv;
//                float h, s, v = 0;
//                Color.RGBToHSV(pixels.GetPixel(x, y), out h, out s, out v);
//                hsv.h = h;
//                hsv.s = s;
//                hsv.v = v;

//                //Check max and min
//                if (hsv.s > maxSat)
//                    maxSat = hsv.s;
//                if (hsv.s < minSat)
//                    minSat = hsv.s;
//                if (hsv.v > maxVal)
//                    maxVal = hsv.v;
//                if (hsv.v < minVal)
//                    minVal = hsv.v;

//                //Store HSV values
//                positionsHSV.Add(hsv);
//            }

//            //Region randoms
//            float randomHueValue = UnityEngine.Random.Range(0f, 1f);
//            float randomSaturationModValue = UnityEngine.Random.Range(-minSat, 1 - maxSat);
//            float randomValueModValue = UnityEngine.Random.Range(-minVal, 1 - maxVal);

//            for (int j = 0; j < positionCount; j++)
//            {
//                PosInt coordinates = region.positions[j];
//                int x = coordinates.x;
//                int y = coordinates.y;

//                //Re-use HSV value
//                HSVRGB.HSV hsv = positionsHSV[j];

//                //HSV to RGB - Custom faster
//                Color genColor = blank;
//                genColor = HSVRGB.HSVToRGB(
//                    randomHue ? randomHueValue : hsv.h,
//                    randomSaturation ? Mathf.Clamp(hsv.s + randomSaturationModValue, 0, 1) : hsv.s,
//                    randomValue ? Mathf.Clamp(hsv.v + randomValueModValue, 0, 1) : hsv.v
//                );

//                pixels.SetPixel(x, y, genColor);
//            }
//        }

//        variantTexture.SetPixels(pixels.pixels);
//        variantTexture.Apply();

//        variantTexture.filterMode = FilterMode.Point;

//        return variantTexture;
//    }

//    /// <summary>
//    /// Returns a Texture2D variant with optional random Hue, Saturation and Value for each ColorRegion(s) flagged for variation.
//    /// Saturation and Value are applied as additives to the original color and are completely random.
//    /// </summary>
//    public Texture2D GenerateRandomVariant(bool randomHue, bool randomSaturation, bool randomValue)
//    {
//        Color blank = Color.clear;

//        //Load pixels (dynamic to sprite sheet pivot)
//        Sprite sprt = GetComponent<SpriteRenderer>().sprite;
//        Color[] px = sprt.texture.GetPixels((int)sprt.textureRect.min.x, (int)sprt.textureRect.min.y, (int)sprt.textureRect.width, (int)sprt.textureRect.height);
//        PixelMatrix pixels = new PixelMatrix((int)sprt.textureRect.width, (int)sprt.textureRect.height, px);

//        Texture2D variantTexture = new Texture2D(pixels.width, pixels.height);

//        foreach (var region in regions)
//        {
//            int positionCount = region.positions.Count;

//            //Region randoms
//            float randomHueValue = UnityEngine.Random.Range(0f, 1f);
//            float randomSaturatonModValue = UnityEngine.Random.Range(-1f, 1f);
//            float randomValueModValue = UnityEngine.Random.Range(-1f, 1f);

//            for (int j = 0; j < positionCount; j++)
//            {
//                PosInt coordinates = region.positions[j];
//                int x = coordinates.x;
//                int y = coordinates.y;

//                //No variation, use original
//                if (!region.varied)
//                {
//                    pixels.SetPixel(x, y, pixels.GetPixel(x, y));
//                }
//                //Variation HSV
//                else
//                {
//                    //RGB to HSV - Unity faster
//                    HSVRGB.HSV hsv;
//                    float h, s, v = 0;
//                    Color.RGBToHSV(pixels.GetPixel(x, y), out h, out s, out v);
//                    hsv.h = h;
//                    hsv.s = s;
//                    hsv.v = v;

//                    //HSV to RGB - Custom faster
//                    Color genColor = blank;
//                    genColor = HSVRGB.HSVToRGB(
//                        randomHue ? randomHueValue : hsv.h,
//                        randomSaturation ? Mathf.Clamp(hsv.s + randomSaturatonModValue, 0, 1) : hsv.s,
//                        randomValue ? Mathf.Clamp(hsv.v + randomValueModValue, 0, 1) : hsv.v
//                    );

//                    pixels.SetPixel(x, y, genColor);
//                }
//            }
//        }

//        variantTexture.SetPixels(pixels.pixels);
//        variantTexture.Apply();

//        variantTexture.filterMode = FilterMode.Point;

//        return variantTexture;
//    }

//    /// <summary>
//    /// Returns a Texture2D variant with each ColorRegion(s) defined Hue, Saturation and Value modifications, if they are flagged for variation.
//    /// Alter SpriteVariants.regions defined gen_ HSV values to customize the variant produced.
//    /// Can use SpriteVariants.GetVariedRegions to get the ColorRegion(s) flagged for variation or iterate through the ColorRegion(s) manually.
//    /// </summary>
//    public Texture2D GenerateVariant()
//    {
//        Color blank = Color.clear;

//        //Load pixels (dynamic to sprite sheet pivot)
//        Sprite sprt = GetComponent<SpriteRenderer>().sprite;
//        Color[] px = sprt.texture.GetPixels((int)sprt.textureRect.min.x, (int)sprt.textureRect.min.y, (int)sprt.textureRect.width, (int)sprt.textureRect.height);
//        PixelMatrix pixels = new PixelMatrix((int)sprt.textureRect.width, (int)sprt.textureRect.height, px);

//        Texture2D variantTexture = new Texture2D(pixels.width, pixels.height);

//        foreach (var region in regions)
//        {
//            int positionCount = region.positions.Count;
//            for (int j = 0; j < positionCount; j++)
//            {
//                PosInt coordinates = region.positions[j];
//                int x = coordinates.x;
//                int y = coordinates.y;

//                //No variation, use original
//                if (!region.varied)
//                {
//                    pixels.SetPixel(x, y, pixels.GetPixel(x, y));
//                }
//                //Variation HSV
//                else
//                {
//                    //RGB to HSV - Unity faster
//                    HSVRGB.HSV hsv;
//                    float h, s, v = 0;
//                    Color.RGBToHSV(pixels.GetPixel(x, y), out h, out s, out v);
//                    hsv.h = h;
//                    hsv.s = s;
//                    hsv.v = v;

//                    //HSV to RGB - Custom faster
//                    Color genColor = blank;
//                    genColor = HSVRGB.HSVToRGB(
//                        region.gen_targetHue,
//                        Mathf.Clamp(hsv.s + region.gen_saturationMod, 0, 1),
//                        Mathf.Clamp(hsv.v + region.gen_intensityMod, 0, 1)
//                    );

//                    pixels.SetPixel(x, y, genColor);
//                }
//            }
//        }

//        variantTexture.SetPixels(pixels.pixels);
//        variantTexture.Apply();

//        variantTexture.filterMode = FilterMode.Point;

//        return variantTexture;
//    }
//}
