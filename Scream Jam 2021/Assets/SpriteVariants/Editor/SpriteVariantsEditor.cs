//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEngine.SceneManagement;
//using System.Collections.Generic;
//using System;
//using System.IO;

//[CustomEditor(typeof(SpriteVariants))]
//public class SpriteVariantsEditor : Editor
//{

//    public Texture2D sample;
//    public Texture2D sampleUpscale;

//    [HideInInspector]
//    public int regionCombine1 = 0;
//    [HideInInspector]
//    public int regionCombine2 = 0;

//    [HideInInspector]
//    public int selectedRegionId = 0;

//    [HideInInspector]
//    public bool autoGenVariant = false;
//    [HideInInspector]
//    public bool settingsChanged = false;

//    private PixelMatrix pixels;

//    //Combine Pixel Regions
//    public void CombineRegions(int r1, int r2)
//    {
//        if (r1 == r2)
//            return;

//        SpriteVariants spriteVariants = (SpriteVariants)target;

//        if (r1 >= spriteVariants.regions.Count || r2 >= spriteVariants.regions.Count)
//            return;

//        SpriteVariants.ColorRegion region1 = spriteVariants.regions[r1];
//        SpriteVariants.ColorRegion region2 = spriteVariants.regions[r2];

//        //Combine positions
//        region1.positions.AddRange(region2.positions);

//        //Combined Texture
//        RegionTexture(region1);

//        //Remove region
//        spriteVariants.regions.RemoveAt(r2);
//    }

//    //Texture for individual region
//    private void RegionTexture(SpriteVariants.ColorRegion region)
//    {
//        //Load pixels (dynamic to sprite sheet pivot)
//        Sprite sprt = ((SpriteVariants)target).gameObject.GetComponent<SpriteRenderer>().sprite;
//        Color[] px = sprt.texture.GetPixels((int)sprt.textureRect.min.x, (int)sprt.textureRect.min.y, (int)sprt.textureRect.width, (int)sprt.textureRect.height);
//        pixels = new PixelMatrix((int)sprt.textureRect.width, (int)sprt.textureRect.height, px);

//        PixelMatrix regionPx = new PixelMatrix(pixels.width, pixels.height, Color.clear);

//        List<SpriteVariants.PosInt> positions = region.positions;
//        int count = region.positions.Count;
//        for (int i = 0; i < count; i++)
//        {
//            SpriteVariants.PosInt pos = positions[i];
//            regionPx.SetPixel(pos.x, pos.y, pixels.GetPixel(pos.x, pos.y));
//        }

//        Texture2D tex = new Texture2D(pixels.width, pixels.height);

//        tex.SetPixels(regionPx.pixels);
//        tex.Apply();

//        //Ratio Rescale for Inspector display
//        float ratio = tex.width / (float)tex.height;
//        int longSize = 250;
//        if (ratio > 1)
//        {
//            TextureScaler.Scale(tex, longSize, Mathf.RoundToInt(ratio * longSize), FilterMode.Point);
//        }
//        else
//        {
//            TextureScaler.Scale(tex, Mathf.RoundToInt((1 - (1 - ratio)) * longSize), longSize, FilterMode.Point);
//        }

//        tex.filterMode = FilterMode.Point;

//        region.previewTexture = tex;
//        region.varied = true;
//    }

//    //All regions texture
//    public void RegionTextures()
//    {
//        SpriteVariants spriteVariants = (SpriteVariants)target;

//        //Build Texture for each region
//        foreach (var region in spriteVariants.regions)
//        {
//            RegionTexture(region);
//        }
//    }

//    public void DetectRegions()
//    {
//        SpriteVariants spriteVariants = (SpriteVariants)target;

//        //Check SpriteRenderer
//        if(spriteVariants.gameObject.GetComponent<SpriteRenderer>() == null)
//        {
//            Debug.LogError("Sprite Variants requires the GameObject to have a SpriteRenderer with the desired Sprite");
//            return;
//        }

//        //Load pixels (dynamic to sprite sheet pivot)
//        Sprite sprt = ((SpriteVariants)target).gameObject.GetComponent<SpriteRenderer>().sprite;
//        Color[] px = sprt.texture.GetPixels((int)sprt.textureRect.min.x, (int)sprt.textureRect.min.y, (int)sprt.textureRect.width, (int)sprt.textureRect.height);
//        pixels = new PixelMatrix((int)sprt.textureRect.width, (int)sprt.textureRect.height, px);

//        spriteVariants.regions.Clear();

//        LabColor lab = new LabColor(Color.black);
//        int height = pixels.height;
//        int width = pixels.width;
//        for (int i = 0; i < height; i++)
//        {
//            for (int j = 0; j < width; j++)
//            {
//                Color cl = pixels.GetPixel(i, j);

//                if (cl.a == 0)
//                    continue;

//                //Pixel data
//                float h = 0;
//                float s = 0;
//                float v = 0;
//                switch (spriteVariants.mode)
//                {
//                    //Pixel HSV Data
//                    case 0:
//                        Color.RGBToHSV(cl, out h, out s, out v);
//                        break;
//                    //Pixel LAB Data
//                    case 2:
//                        lab = new LabColor(cl);
//                        break;
//                }

//                //Look for existing color in each region
//                SpriteVariants.ColorRegion bestRegion = null;
//                float closest = 100;
//                int regionCount = spriteVariants.regions.Count;
//                float bestH = 0;
//                float bestS = 0;
//                float bestV = 0;
//                for (int r = 0; r < regionCount; r++)
//                {
//                    SpriteVariants.ColorRegion currRegion = spriteVariants.regions[r];
//                    //Comparison Metric
//                    switch (spriteVariants.mode)
//                    {
//                        //RGB
//                        case 1:
//                            float distRGB = (
//                                (cl.r - currRegion.average_rgb.r) * (cl.r - currRegion.average_rgb.r) +
//                                (cl.g - currRegion.average_rgb.g) * (cl.g - currRegion.average_rgb.g) +
//                                (cl.b - currRegion.average_rgb.b) * (cl.b - currRegion.average_rgb.b)
//                            );

//                            if (distRGB < spriteVariants.threshold)
//                            {
//                                if (closest > distRGB)
//                                {
//                                    closest = distRGB;
//                                    bestRegion = currRegion;
//                                }
//                            }
//                            break;
//                        //HSV - Hue
//                        case 0:
//                            //Region HSV Data
//                            float ht = currRegion.average_hsv.h;
//                            float st = currRegion.average_hsv.s;
//                            float vt = currRegion.average_hsv.v;

//                            float distHue = Math.Abs(h - ht);
//                            if (distHue < spriteVariants.threshold)
//                            {
//                                if (closest > distHue)
//                                {
//                                    closest = distHue;
//                                    bestRegion = currRegion;

//                                    //Also store HSV values of best region
//                                    bestH = ht;
//                                    bestS = st;
//                                    bestV = vt;
//                                }
//                            }
//                            break;
//                        //LAB
//                        case 2:
//                            //Region LAB Data
//                            LabColor labt = currRegion.average_lab;

//                            float distLAB = LabColor.Distance(lab, labt) / 100f;
//                            if (distLAB < spriteVariants.threshold)
//                            {
//                                if (closest > distLAB)
//                                {
//                                    closest = distLAB;
//                                    bestRegion = currRegion;
//                                }
//                            }
//                            break;
//                    }

//                }

//                //If existing, add to it
//                if (bestRegion != null)
//                {
//                    //Add position
//                    bestRegion.positions.Add(new SpriteVariants.PosInt(i, j));

//                    //Average Metric
//                    int positionsCount = bestRegion.positions.Count;
//                    float ratio = 1f / positionsCount;
//                    switch (spriteVariants.mode)
//                    {
//                        //RGB
//                        case 1:
//                            bestRegion.average_rgb = Color.Lerp(bestRegion.average_rgb, cl, ratio);
//                            bestRegion.average_rgb.a = 1;
//                            break;
//                        //HSV - Hue
//                        case 0:
//                            float newHue = bestH * (positionsCount - 1) / positionsCount + h * ratio;

//                            bestRegion.average_rgb = HSVRGB.HSVToRGB(newHue, bestS, bestV);

//                            Color.RGBToHSV(bestRegion.average_rgb, out bestRegion.average_hsv.h, out bestRegion.average_hsv.s, out bestRegion.average_hsv.v);
//                            break;
//                        //LAB
//                        case 2:
//                            //Pixel LAB Data
//                            LabColor labt = new LabColor(bestRegion.average_rgb);

//                            bestRegion.average_rgb = LabColor.ToColor(LabColor.Lerp(lab, labt, ratio));
//                            bestRegion.average_rgb.a = 1;

//                            bestRegion.average_lab = labt;
//                            break;
//                    }
//                }
//                //Create if non existent
//                else
//                {
//                    SpriteVariants.ColorRegion newRegion = new SpriteVariants.ColorRegion();

//                    //Store various color spaces
//                    newRegion.average_rgb = cl;
//                    Color.RGBToHSV(newRegion.average_rgb, out newRegion.average_hsv.h, out newRegion.average_hsv.s, out newRegion.average_hsv.v);
//                    newRegion.average_lab = new LabColor(newRegion.average_rgb);

//                    //Create positions
//                    newRegion.positions = new List<SpriteVariants.PosInt>(pixels.width * pixels.height) { new SpriteVariants.PosInt(i, j) };

//                    //Random target hue, just as example
//                    newRegion.gen_targetHue = UnityEngine.Random.Range(0f, 1f);

//                    newRegion.canvas = new PixelMatrix(pixels.width, pixels.height, Color.clear);

//                    spriteVariants.regions.Add(newRegion);
//                }
//            }
//        }

//        //Build Region Textures
//        RegionTextures();
//    }

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        SpriteVariants spriteVariants = (SpriteVariants)target;

//        //Region Detection Settings
//        EditorGUILayout.LabelField(" ", "Region Detection");

//        //Color Space Option
//        string[] options = { "HSV - Hue", "RGB", "LAB" };
//        spriteVariants.mode = EditorGUILayout.Popup(
//            "Color Space:",
//            spriteVariants.mode,
//            options);

//        //Threshold
//        spriteVariants.threshold = EditorGUILayout.Slider("Threshold:", spriteVariants.threshold, 0.01f, 1f);

//        //Detect Regions action
//        if (GUILayout.Button("Detect Regions"))
//        {
//            DetectRegions();
//            if (spriteVariants.regions.Count <= selectedRegionId)
//                selectedRegionId = 0;

//            //Set scene dirty
//            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
//        }

//        //Once detected regions
//        if (spriteVariants.regions.Count > 0)
//        {

//            //Centering
//            GUILayout.BeginVertical();
//            GUILayout.BeginHorizontal();

//            //Selected Region for preview and settings

//            GUILayout.Box(spriteVariants.regions[selectedRegionId].previewTexture);

//            //End Centering
//            GUILayout.EndHorizontal();
//            GUILayout.EndVertical();

//            //Region Info
//            EditorGUILayout.LabelField("Region: " + selectedRegionId.ToString());
//            EditorGUILayout.ColorField("Average Region Color: ", spriteVariants.regions[selectedRegionId].average_rgb);

//            //Select Next Region
//            if (GUILayout.Button("View Next Region"))
//            {
//                selectedRegionId = selectedRegionId + 1;
//                if (selectedRegionId >= spriteVariants.regions.Count)
//                {
//                    selectedRegionId = 0;
//                }
//            }

//            //Region Settings
//            EditorGUILayout.LabelField(" ", "Combine Regions");

//            //Manual Combine Regions
//            string[] regions1 = new string[spriteVariants.regions.Count];
//            for (int i = 0; i < regions1.Length; i++)
//            {
//                regions1[i] = i.ToString();
//            }
//            regionCombine1 = EditorGUILayout.Popup(
//                "First Region:",
//                regionCombine1,
//                regions1);

//            string[] regions2 = new string[spriteVariants.regions.Count];
//            for (int i = 0; i < regions2.Length; i++)
//            {
//                regions2[i] = i.ToString();
//            }
//            regionCombine2 = EditorGUILayout.Popup(
//                "Second Region:",
//                regionCombine2,
//                regions1);

//            if (GUILayout.Button("Combine Regions"))
//            {
//                CombineRegions(regionCombine1, regionCombine2);

//                //Force save scene
//                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

//                selectedRegionId = Math.Min(regionCombine1, regionCombine2);
//            }

//            //Selected Region
//            SpriteVariants.ColorRegion selectedRegion = null;
//            if (spriteVariants.regions.Count > 0)
//                selectedRegion = spriteVariants.regions[selectedRegionId];

//            //Region Settings
//            EditorGUILayout.LabelField(" ", "Region Settings");

//            //Toggle Variation
//            if (spriteVariants.regions.Count > 0)
//            {
//                bool prevVaried = selectedRegion.varied;
//                selectedRegion.varied = GUILayout.Toggle(selectedRegion.varied, "Region Variation");
//                if (prevVaried != selectedRegion.varied)
//                {
//                    //Force save scene
//                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

//                    settingsChanged = true;
//                }
//            }

//            //Generation settings and detect changes for Auto Generation
//            if(selectedRegion.varied)
//            {
//                float prevHue = selectedRegion.gen_targetHue;
//                float prevSat = selectedRegion.gen_saturationMod;
//                float prevInt = selectedRegion.gen_intensityMod;

//                selectedRegion.gen_targetHue = EditorGUILayout.Slider("Hue:", selectedRegion.gen_targetHue, 0f, 0.9999f);
//                EditorGUILayout.ColorField(Color.HSVToRGB(selectedRegion.gen_targetHue, 1, 1));
//                selectedRegion.gen_saturationMod = EditorGUILayout.Slider("Saturation Additive:", selectedRegion.gen_saturationMod, -1f, 1f);
//                selectedRegion.gen_intensityMod = EditorGUILayout.Slider("Value Additive:", selectedRegion.gen_intensityMod, -1f, 1f);

//                if (prevHue != selectedRegion.gen_targetHue ||
//                prevSat != selectedRegion.gen_saturationMod ||
//                prevInt != selectedRegion.gen_intensityMod)
//                    settingsChanged = true;
//            }

//            //Auto Generate Variant on settings change
//            autoGenVariant = GUILayout.Toggle(autoGenVariant, "Auto Generate Variant");

//            //Generate Variant
//            if (GUILayout.Button("Generate Variant") || (autoGenVariant && settingsChanged))
//            {
//                //Reset settings changed detection
//                settingsChanged = false;

//                //Generate
//                sample = spriteVariants.GenerateVariant();
//                sampleUpscale = new Texture2D(sample.width, sample.height);
//                Graphics.CopyTexture(sample, sampleUpscale);

//                //Scale to fit UI
//                float ratio = sampleUpscale.width / (float)sampleUpscale.height;
//                int longSize = 250;
//                if (ratio > 1)
//                {
//                    TextureScaler.Scale(sampleUpscale, longSize, Mathf.RoundToInt(ratio * longSize), FilterMode.Point);
//                }
//                else
//                {
//                    TextureScaler.Scale(sampleUpscale, Mathf.RoundToInt((1 - (1 - ratio)) * longSize), longSize, FilterMode.Point);
//                }
//            }

//            //Generate Random Variant
//            if (GUILayout.Button("Generate Random Variant"))
//            {
//                //Generate
//                sample = spriteVariants.GenerateRandomVariant(true, true, true);
//                sampleUpscale = new Texture2D(sample.width, sample.height);
//                Graphics.CopyTexture(sample, sampleUpscale);

//                //Scale to fit UI
//                float ratio = sampleUpscale.width / (float)sampleUpscale.height;
//                int longSize = 250;
//                if (ratio > 1)
//                {
//                    TextureScaler.Scale(sampleUpscale, longSize, Mathf.RoundToInt(ratio * longSize), FilterMode.Point);
//                }
//                else
//                {
//                    TextureScaler.Scale(sampleUpscale, Mathf.RoundToInt((1 - (1 - ratio)) * longSize), longSize, FilterMode.Point);
//                }
//            }

//            //Generate Random Variant (Bounded)
//            if (GUILayout.Button("Generate Random Variant (Bounded)"))
//            {
//                //Generate
//                sample = spriteVariants.GenerateRandomVariantBounded(true, true, true);
//                sampleUpscale = new Texture2D(sample.width, sample.height);
//                Graphics.CopyTexture(sample, sampleUpscale);

//                //Scale to fit UI
//                float ratio = sampleUpscale.width / (float)sampleUpscale.height;
//                int longSize = 250;
//                if (ratio > 1)
//                {
//                    TextureScaler.Scale(sampleUpscale, longSize, Mathf.RoundToInt(ratio * longSize), FilterMode.Point);
//                }
//                else
//                {
//                    TextureScaler.Scale(sampleUpscale, Mathf.RoundToInt((1 - (1 - ratio)) * longSize), longSize, FilterMode.Point);
//                }
//            }

//            //Display if available
//            if (sampleUpscale != null)
//            {
//                //Centering
//                GUILayout.BeginVertical();
//                GUILayout.BeginHorizontal();

//                //Selected Region for preview and settings
//                GUILayout.Box(sampleUpscale);

//                //End Centering
//                GUILayout.EndHorizontal();
//                GUILayout.EndVertical();
//            }

//            //Save Variant to Texture File in Assets Folder
//            if (sampleUpscale != null)
//            {
//                if (GUILayout.Button("Save Variant"))
//                {
//                    //Encode to PNG
//                    byte[] data = sample.EncodeToPNG();

//                    //Find vacant name
//                    string name = spriteVariants.name + "Variant";
//                    int variantN = 1;
//                    while (File.Exists("Assets/" + name + ".png"))
//                    {
//                        name = spriteVariants.name + "Variant" + "(" + variantN + ")";
//                        variantN++;
//                    }

//                    //Write File
//                    File.WriteAllBytes("Assets/" + name + ".png", data);

//                    //Force Unity to check for new Assets
//                    AssetDatabase.Refresh();
//                }
//            }
//        }
//    }
//}