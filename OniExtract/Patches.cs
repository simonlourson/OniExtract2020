using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OniExtract2
{
    public class Patches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Debug.Log("Hello world!");
            }
        }

        [HarmonyPatch(typeof(Assets))]
        [HarmonyPatch("AddPrefab")]
        public class Assets_AddPrefab_Patch
        {
            public static void Postfix(KPrefabID prefab)
            {
                OniExtract_Game_OnPrefabInit.AddPrefab(prefab);
            }
        }

        [HarmonyPatch(typeof(Game))]
        [HarmonyPatch("OnPrefabInit")]
        public class Game_OnPrefabInit
        {
            public static void Postfix()
            {
                if (OniExtract_Game_OnPrefabInit.savePipesTexture)
                {
                    Debug.Log("Saving liquid texture");
                    OniExtract_Game_OnPrefabInit.SaveTexture(Lighting.Instance.Settings.LiquidConduit.backgroundTexture.name, Lighting.Instance.Settings.LiquidConduit.backgroundTexture);
                    OniExtract_Game_OnPrefabInit.SaveTexture(Lighting.Instance.Settings.LiquidConduit.foregroundTexture.name, Lighting.Instance.Settings.LiquidConduit.foregroundTexture);
                    Debug.Log("Saving gas texture");
                    OniExtract_Game_OnPrefabInit.SaveTexture(Lighting.Instance.Settings.GasConduit.backgroundTexture.name, Lighting.Instance.Settings.GasConduit.backgroundTexture);
                    OniExtract_Game_OnPrefabInit.SaveTexture(Lighting.Instance.Settings.GasConduit.foregroundTexture.name, Lighting.Instance.Settings.GasConduit.foregroundTexture);
                }
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        internal class OniExtract_Game_OnPrefabInit
        {
            public static bool saveIconTexture = false;
            public static bool saveTileTexture = false;
            public static bool saveBuildingTexture = false;
            public static bool saveSubstanceTexture = false;
            public static bool savePrefabTexture = false;
            public static bool savePipesTexture = true;
            public static bool saveAllSprites = false;
            static List<string> names = new List<string>();


            static List<string> usefulTags = new List<string>();
            public static void AddUsefulTag(string tag)
            {
                if (!usefulTags.Contains(tag)) usefulTags.Add(tag);
            }
            static bool IsTagUseful(string tag)
            {
                return usefulTags.Contains(tag);
            }

            public static void AddPrefab(KPrefabID prefab)
            {
                bool isUseful = false;
                Debug.Log(prefab.name);
                var tags = "";
                foreach (var tag in prefab.Tags)
                {
                    tags = tags + ";" + tag.Name;
                    if (IsTagUseful(tag.Name)) isUseful = true;
                }
                if (isUseful) Debug.Log("This is Useful !!!");
                Debug.Log(tags);

                if ("BasicFabric".Equals(prefab.name))
                {
                    var test = prefab.gameObject.GetComponent<KBatchedAnimController>();
                    foreach (var p in test.AnimFiles)
                    {
                        var data = p.GetData();

                        if (data.build.textureCount > 0)
                        {
                            var textureName = data.build.GetTexture(0).name;
                            if (OniExtract_Game_OnPrefabInit.savePrefabTexture) OniExtract_Game_OnPrefabInit.SaveTexture(textureName, data.build.GetTexture(0));
                        }
                        Debug.Log(test.debugName);
                    }
                }
            }

            public static string GetImagesDirectory()
            {
                string imagesLocation = Path.Combine(Util.RootFolder(), "export", "images");
                if (!Directory.Exists(imagesLocation))
                    Directory.CreateDirectory(imagesLocation);

                return imagesLocation;
            }

            public static string GetDatabaseDirectory()
            {
                string databaseLocation = Path.Combine(Util.RootFolder(), "export", "database");
                if (!Directory.Exists(databaseLocation))
                    Directory.CreateDirectory(databaseLocation);

                return databaseLocation;
            }

            public static void SaveTexture(string name, Texture2D source)
            {
                if (names.Contains(name)) return;
                else names.Add(name);

                try
                {

                    //Debug.Log("OniExtract: " + "Saving Texture " + name);

                    RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

                    Graphics.Blit(source, renderTex);
                    RenderTexture previous = RenderTexture.active;
                    RenderTexture.active = renderTex;
                    Texture2D readableText = new Texture2D(source.width, source.height);
                    readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                    readableText.Apply();
                    RenderTexture.active = previous;
                    RenderTexture.ReleaseTemporary(renderTex);

                    byte[] bytes = readableText.EncodeToPNG();
                    var dirPath = GetImagesDirectory();

                    File.WriteAllBytes(Path.Combine(dirPath, name + ".png"), bytes);
                }
                catch (Exception)
                {
                    Debug.Log("OniExtract: " + " Error Saving Texture " + name);
                }
            }

            private static void ExportC()
            {
                Debug.Log("***** Start SideScreens *****");
                List<SideScreenContent> sideScreens = new List<SideScreenContent>();
                foreach (Type type in
                Assembly.GetAssembly(typeof(SideScreenContent)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(SideScreenContent))))
                {
                    sideScreens.Add((SideScreenContent)Activator.CreateInstance(type, new object[0]));
                    Debug.Log(type.ToString());
                }

                var export = new Export();

                Debug.Log("***** Start buildings *****");
                for (int indexBuidling = 0; indexBuidling < Assets.BuildingDefs.Count; ++indexBuidling)
                {
                    var buildingDef = Assets.BuildingDefs[indexBuidling];
                    Debug.Log("************");
                    Debug.Log(buildingDef.PrefabID); 
                    
                    var bBuilding = new BBuildingFinal(buildingDef, export);

                    foreach (var sideScreen in sideScreens)
                    {
                        if (sideScreen.IsValidForTarget(buildingDef.BuildingComplete))
                        {
                            string screendId = sideScreen.GetType().ToString();
                            Debug.Log("==>" + screendId);

                            if (screendId.Equals("SingleSliderSideScreen"))
                            {
                                var target = buildingDef.BuildingComplete.GetComponent<ISingleSliderControl>();
                                if (target == null)
                                {
                                    Debug.LogError((object)"The gameObject received does not contain a Manual Generator component");
                                }
                                else
                                {
                                    var targetCast = (ISliderControl)target;

                                    var screen = new BSingleSliderSideScreen(screendId);
                                    screen.title = Strings.Get(targetCast.SliderTitleKey);
                                    screen.sliderUnits = targetCast.SliderUnits;
                                    screen.min = targetCast.GetSliderMin(0);
                                    screen.max = targetCast.GetSliderMax(0);
                                    screen.sliderDecimalPlaces = targetCast.SliderDecimalPlaces(0);
                                    screen.tooltip = Strings.Get(targetCast.GetSliderTooltipKey(0));
                                    screen.defaultValue = targetCast.GetSliderValue(0);

                                    bBuilding.uiScreens.Add(screen);
                                }
                            }
                            else if (screendId.Equals("ThresholdSwitchSideScreen"))
                            {
                                Debug.Log("====>" + STRINGS.UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.ABOVE_BUTTON);
                                Debug.Log("====>" + STRINGS.UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BELOW_BUTTON);
                                var target = buildingDef.BuildingComplete.GetComponent<IThresholdSwitch>();
                                if (target == null) Debug.LogError((object)"The gameObject received does not contain a Manual Generator component");
                                else
                                {
                                    var screen = new BThresholdSwitchSideScreen(screendId);
                                    screen.title = target.Title;
                                    screen.aboveToolTip = target.AboveToolTip;
                                    screen.belowToolTip = target.BelowToolTip;
                                    screen.thresholdValueName = target.ThresholdValueName;
                                    screen.thresholdValueUnits = target.ThresholdValueUnits();
                                    screen.rangeMin = target.RangeMin;
                                    screen.rangeMax = target.RangeMax;
                                    screen.incrementScale = target.IncrementScale;
                                    screen.defaultValue = target.Threshold;
                                    screen.defaultBoolean = target.ActivateAboveThreshold;

                                    bBuilding.uiScreens.Add(screen);
                                }
                            }
                            else if (screendId.Equals("ActiveRangeSideScreen"))
                            {
                                var target = buildingDef.BuildingComplete.GetComponent<IActivationRangeTarget>();
                                if (target != null)
                                {
                                    var screen = new BActiveRangeSideScreen(screendId);
                                    screen.title = target.ActivationRangeTitleText;
                                    screen.minValue = target.MinValue;
                                    screen.maxValue = target.MaxValue;
                                    screen.defaultActivateValue = target.ActivateValue;
                                    screen.defaultDeactivateValue = target.DeactivateValue;
                                    screen.activateSliderLabelText = target.ActivateSliderLabelText;
                                    screen.deactivateSliderLabelText = target.DeactivateSliderLabelText;
                                    screen.activateTooltip = target.ActivateTooltip;
                                    screen.deactivateTooltip = target.DeactivateTooltip;

                                    bBuilding.uiScreens.Add(screen);
                                }

                            }
                            else if (screendId.Equals("LogicBitSelectorSideScreen"))
                            {
                                var target = buildingDef.BuildingComplete.GetComponent<ILogicRibbonBitSelector>();
                                if (target != null)
                                {
                                    var screen = new BBitSelectorSideScreen(screendId);
                                    screen.title = Strings.Get(target.SideScreenTitle);
                                    screen.description = target.SideScreenDescription;

                                    bBuilding.uiScreens.Add(screen);
                                }
                                else Debug.Log("No UI screen found for " + buildingDef.PrefabID);
                            }
                        }
                    }

                    export.buildings.Add(bBuilding);
                }

                ExportBuildMenu(export);
                ExportElements(export);
                ExportSprites(export);

                /*
                foreach (var animFile in Assets.Anims)
                {
                    var data = animFile.GetData();

                    if (data.build != null && data.build.textureCount > 0)
                    {
                        var textureName = data.build.GetTexture(0).name;
                        OniExtract_Game_OnPrefabInit.SaveTexture(textureName, data.build.GetTexture(0));
                    }

                    for (int indexGetAnim = 0; indexGetAnim < data.animCount; ++indexGetAnim)
                    {
                        var anim = data.GetAnim(indexGetAnim);
                        Debug.Log(anim.name);
                    }
                }
                */

                var dirPath = GetDatabaseDirectory();

                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.Formatting = Formatting.Indented;

                File.WriteAllText(Path.Combine(dirPath, "database" + ".json"), JsonConvert.SerializeObject(export, settings));
            }

            private static void ExportSprites(Export export)
            {
                // Some useful sprites
                var usefulSprites = new List<string>() {
                "logicInput",
                "logicOutput",
                "input",
                "output",
                "electrical_disconnected",
                "logic_ribbon_all_in",
                "logic_ribbon_all_out",
                "logicResetUpdate"
            };

                var textureDic = new Dictionary<string, string>();
                Debug.Log("*************");
                Debug.Log("List of sprites");
                foreach (var key in Assets.Sprites.Keys)
                {
                    var sprite = Assets.Sprites[key];



                    Debug.Log(sprite.name);
                    bool isUseful = usefulSprites.Contains(sprite.name);
                    bool isMenuIcon = sprite.name.Contains("icon_category") && !sprite.name.Contains("disabled");

                    // We only want some sprites
                    if (!saveAllSprites && !isUseful && !isMenuIcon) continue;

                    var texId = sprite.texture.GetNativeTexturePtr().ToString();
                    if (!textureDic.Keys.Contains(texId))
                        textureDic.Add(texId, sprite.name);

                    var texName = textureDic[texId];

                    //Debug.Log(sprite.name + " : " + texName + " : " +
                    //    sprite.textureRect.x + ";" + sprite.textureRect.y + ";" + sprite.textureRect.width + ";" + sprite.textureRect.height);

                    if (saveIconTexture && sprite.texture != null) SaveTexture(texName, sprite.texture);

                    var newSpriteInfo = new BSpriteInfo(sprite, texName, sprite.texture);
                    newSpriteInfo.isInputOutput = isUseful;
                    export.uiSprites.Add(newSpriteInfo);
                }
            }

            private static void ExportBuildMenu(Export export)
            {
                export.buildMenuCategories = new List<BuildMenuCategory>();
                export.buildMenuItems = new List<BuildMenuItem>();

                foreach (var planInfo in TUNING.BUILDINGS.PLANORDER)
                {
                    if (export.buildMenuCategories.Count(b => b.category == planInfo.category.HashValue) == 0)
                        export.buildMenuCategories.Add(new BuildMenuCategory()
                        {
                            category = planInfo.category.HashValue,
                            categoryName = BuildMenuCategory.GetName(planInfo.category.HashValue),
                            categoryIcon = BuildMenuCategory.GetIcon(planInfo.category.HashValue)
                        });

                    var buildings = (List<string>)planInfo.data;
                    foreach (var building in buildings)
                        export.buildMenuItems.Add(new BuildMenuItem()
                        {
                            category = planInfo.category.HashValue,
                            buildingId = building
                        });

                }
            }

            private static void ExportElements(Export export)
            {
                export.elements = new List<BElement>();
                foreach (var e in ElementLoader.elements)
                {
                    var element = new BElement(e, export);

                }
            }

            private static void Postfix()
            {
                Debug.Log("OniExtract: " + "OnPrefabInit");

                ExportC();

            }
        }
    }
}
