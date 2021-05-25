using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Grid;
using static OniExtract2.Patches;

namespace OniExtract2
{
    public class BBuildingFinal
    {
        public string DefaultAnimState;

        public string name;
        public string prefabId;
        public string kanimPrefix;
        public string textureName;
        public bool isTile;
        public bool isUtility;
        public bool isBridge;
        public bool drawSolid;
        public bool dragBuild;
        public int backColor;
        public BVector2 sizeInCells;
        public SceneLayer sceneLayer;
        public ObjectLayer objectLayer;
        public PermittedRotations permittedRotations;
        public ViewMode viewMode;
        public bool tileableLeftRight;
        public bool tileableTopBottom;
        public BuildLocationRule buildLocationRule;
        public List<UtilityInfo> utilities;
        public List<UiScreen> uiScreens;
        public SpriteName sprites;

        public List<string> materialCategory;
        public List<float> materialMass;

        public BBuildingFinal(BuildingDef b, Export export)
        {
            this.name = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{b.PrefabID.ToUpperInvariant()}.NAME");
            //int startIndex = name.IndexOf("\">");
            //if (startIndex != -1) this.name = this.name.Substring(startIndex + 2);
            //int endIndex = this.name.IndexOf("</");
            //if (endIndex != -1) this.name = this.name.Substring(0, endIndex);
            this.prefabId = b.PrefabID;
            this.isTile = false;
            this.isUtility = b.isUtility;
            this.isBridge = false;
            this.sizeInCells = new BVector2(b.WidthInCells, b.HeightInCells);
            this.permittedRotations = b.PermittedRotations;
            this.dragBuild = b.DragBuild;
            this.tileableLeftRight = false;
            this.tileableTopBottom = false;
            this.buildLocationRule = b.BuildLocationRule;

            if (prefabId.Contains("Bridge") && !prefabId.Contains("HighWattage")) isBridge = true;

            this.sceneLayer = b.SceneLayer;
            if ("WireBridgeHighWattage".Equals(this.prefabId) ||
                "WireRefinedBridgeHighWattage".Equals(this.prefabId))
                this.sceneLayer = SceneLayer.Building;

            this.objectLayer = b.ObjectLayer;
            this.viewMode = GetViewMode(b.ViewMode.HashValue);
            if ("ThermalBlock".Equals(this.prefabId) ||
                "ExteriorWall".Equals(this.prefabId))
            {
                this.viewMode = ViewMode.Unknown;
                this.sceneLayer = SceneLayer.Backwall;
            }

            this.DefaultAnimState = b.DefaultAnimState;


            materialCategory = new List<string>();
            foreach (var s in b.MaterialCategory)
            {
                materialCategory.Add(s);
                OniExtract_Game_OnPrefabInit.AddUsefulTag(s);
            }

            materialMass = new List<float>();
            foreach (var m in b.Mass)
                materialMass.Add(m);

            uiScreens = new List<UiScreen>();
            sprites = new SpriteName("all sprites");

            if (isUtility || isBridge)
            {
                if (prefabId.Contains("Radiant"))
                {
                    this.backColor = 0xE6E678;
                }
                else if (prefabId.Contains("Insulated"))
                {
                    this.backColor = 0xE6B671;
                }
                else if (prefabId.Contains("LogicWire"))
                {
                    this.backColor = 0xE3BEE3;
                }
                else if (prefabId.Contains("SolidConduit"))
                {
                    this.backColor = 0x8F551C;
                }
                else if (prefabId.Contains("Wire"))
                {
                    this.backColor = 0xB65D5A;
                }
                else
                {
                    this.backColor = 0xD3D5D7;
                }
            }
            else
            {
                this.backColor = 0xFFFFFF;
            }


            foreach (var p in b.AnimFiles)
            {
                var data = p.GetData();

                if (data.build.textureCount > 0)
                {
                    textureName = data.build.GetTexture(0).name;
                    if (OniExtract_Game_OnPrefabInit.saveBuildingTexture) OniExtract_Game_OnPrefabInit.SaveTexture(textureName, data.build.GetTexture(0));
                }

                kanimPrefix = b.PrefabID + "_";
                for (int indexGetAnim = 0; indexGetAnim < data.animCount; ++indexGetAnim)
                {
                    var anim = data.GetAnim(indexGetAnim);

                    var firstFrame = anim.GetFrame(anim.animFile.animBatchTag, 0);
                    var animationName = kanimPrefix + anim.name;

                    // Find good anims for all
                    if (true)
                    {
                        Debug.Log(this.prefabId + "_" + anim.name);
                        for (int indexElement = 0; indexElement < firstFrame.numElements; indexElement++)
                        {
                            var frameElement = data.GetAnimFrameElement(firstFrame.firstElementIdx + indexElement);
                            var frameElementName = animationName + "_" + indexElement.ToString() + "_" + frameElement.symbol.DebuggerDisplay;

                            //Debug.Log(frameElementName);
                        }
                    }
                    if (this.prefabId.Contains("Door")) this.DefaultAnimState = "closed";
                    if (this.prefabId.Equals("BatteryMedium")) this.DefaultAnimState = "on";
                    if (this.prefabId.Equals("LogicRibbonReader")) this.DefaultAnimState = "idle";
                    if (this.prefabId.Equals("LogicRibbonWriter")) this.DefaultAnimState = "idle";
                    if (this.prefabId.Equals("FloorSwitch")) this.DefaultAnimState = "off_up";

                    bool isUi = anim.name.Equals("ui");
                    bool isPlace = anim.name.Contains("place");
                    bool isDefaultKanim = anim.name.Equals(this.DefaultAnimState);

                    if (!isUi &&
                        !isPlace &&
                        !isUtility &&
                        !isBridge &&
                        !isDefaultKanim) continue;




                    /*
                    newSpriteModifier.framebboxMin = new BVector2(firstFrame.bbox.min);
                    newSpriteModifier.framebboxMax = new BVector2(firstFrame.bbox.max);
                    newSpriteModifier.center = new BVector3(firstFrame.bbox.Center);
                    newSpriteModifier.range = new BVector3(firstFrame.bbox.Range);
                    newSpriteModifier.width = firstFrame.bbox.Width;
                    newSpriteModifier.height = firstFrame.bbox.Height;
                    newSpriteModifier.depth = firstFrame.bbox.Depth;
                    */

                    bool logSup = false;
                    if (firstFrame.numElements > 1)
                    {
                        logSup = true;
                        //Debug.Log("*************");
                        //Debug.Log(b.PrefabID);
                    }

                    // Logic :
                    // If there is only one or two elements, take the last one, it is a "Place" or a "Solid"
                    // If there is three, it is a tileable thing

                    if (firstFrame.numElements == 0)
                    {
                        //Debug.Log("0 element for : " + animationName);
                        continue;
                    }

                    for (int indexElement = 0; indexElement < firstFrame.numElements; indexElement++)
                    {
                        var frameElement = data.GetAnimFrameElement(firstFrame.firstElementIdx + indexElement);
                        var frameElementName = animationName + "_" + indexElement.ToString() + "_" + frameElement.symbol.DebuggerDisplay;

                        if (isUtility && "outline".Equals(frameElement.symbol.DebuggerDisplay)) continue;

                        var newSpriteModifier = new BSpriteModifier();

                        if (isPlace) newSpriteModifier.tags.Add(SpriteTag.place);
                        else if (isUi) newSpriteModifier.tags.Add(SpriteTag.ui);
                        else newSpriteModifier.tags.Add(SpriteTag.solid);



                        newSpriteModifier.name = frameElementName;
                        LoadSpriteModifier(kanimPrefix, newSpriteModifier, frameElement);

                        string[] left = new string[] { "place_left", "cap_left", "cap_left_place", "cap_left_fg" };
                        string[] right = new string[] { "place_right", "cap_right", "cap_right_place", "cap_right_fg" };
                        string[] top = new string[] { "cap_top_place", "cap_top", "cap_topx" };
                        string[] bottom = new string[] { "cap_bottom_place", "cap_bottom", "cap_bottomx" };
                        if (isUtility)
                        {
                            newSpriteModifier.tags.Add(SpriteTag.connection);
                            if (anim.name.Equals("None_place") || anim.name.Equals("None")) newSpriteModifier.tags.Add(SpriteTag.noConnection);
                            else if (anim.name.Equals("LRUD_place") || anim.name.Equals("LRUD")) newSpriteModifier.tags.Add(SpriteTag.LRUD);
                            else if (anim.name.Equals("LRD_place") || anim.name.Equals("LRD")) newSpriteModifier.tags.Add(SpriteTag.LRD);
                            else if (anim.name.Equals("RUD_place") || anim.name.Equals("RUD")) newSpriteModifier.tags.Add(SpriteTag.RUD);
                            else if (anim.name.Equals("LRU_place") || anim.name.Equals("LRU")) newSpriteModifier.tags.Add(SpriteTag.LRU);
                            else if (anim.name.Equals("LUD_place") || anim.name.Equals("LUD")) newSpriteModifier.tags.Add(SpriteTag.LUD);
                            else if (anim.name.Equals("LR_place") || anim.name.Equals("LR")) newSpriteModifier.tags.Add(SpriteTag.LR);
                            else if (anim.name.Equals("UD_place") || anim.name.Equals("UD")) newSpriteModifier.tags.Add(SpriteTag.UD);
                            else if (anim.name.Equals("LD_place") || anim.name.Equals("LD")) newSpriteModifier.tags.Add(SpriteTag.LD);
                            else if (anim.name.Equals("LU_place") || anim.name.Equals("LU")) newSpriteModifier.tags.Add(SpriteTag.LU);
                            else if (anim.name.Equals("RD_place") || anim.name.Equals("RD")) newSpriteModifier.tags.Add(SpriteTag.RD);
                            else if (anim.name.Equals("RU_place") || anim.name.Equals("RU")) newSpriteModifier.tags.Add(SpriteTag.RU);
                            else if (anim.name.Equals("L_place") || anim.name.Equals("L")) newSpriteModifier.tags.Add(SpriteTag.L);
                            else if (anim.name.Equals("U_place") || anim.name.Equals("U")) newSpriteModifier.tags.Add(SpriteTag.U);
                            else if (anim.name.Equals("D_place") || anim.name.Equals("D")) newSpriteModifier.tags.Add(SpriteTag.D);
                            else if (anim.name.Equals("R_place") || anim.name.Equals("R")) newSpriteModifier.tags.Add(SpriteTag.R);
                        }
                        else if (left.Contains(frameElement.symbol.DebuggerDisplay))
                        {
                            this.tileableLeftRight = true;
                            newSpriteModifier.tags.Add(SpriteTag.tileable_left);
                            newSpriteModifier.tags.Add(SpriteTag.tileable);
                        }
                        else if (right.Contains(frameElement.symbol.DebuggerDisplay))
                        {
                            this.tileableLeftRight = true;
                            newSpriteModifier.tags.Add(SpriteTag.tileable_right);
                            newSpriteModifier.tags.Add(SpriteTag.tileable);
                        }
                        else if (top.Contains(frameElement.symbol.DebuggerDisplay))
                        {
                            this.tileableTopBottom = true;
                            newSpriteModifier.tags.Add(SpriteTag.tileable_up);
                            newSpriteModifier.tags.Add(SpriteTag.tileable);
                        }
                        else if (bottom.Contains(frameElement.symbol.DebuggerDisplay))
                        {
                            this.tileableTopBottom = true;
                            newSpriteModifier.tags.Add(SpriteTag.tileable_down);
                            newSpriteModifier.tags.Add(SpriteTag.tileable);
                        }

                        if (AddSpriteInfo(export, newSpriteModifier, data, frameElement, isUtility && !isUi && !isPlace))
                        {
                            this.sprites.spriteNames.Add(newSpriteModifier.name);
                            export.spriteModifiers.Add(newSpriteModifier);
                        }

                    }

                    if (firstFrame.numElements == 1 || firstFrame.numElements == 2)
                    {
                        var newSpriteModifier = new BSpriteModifier();
                        //export.spriteModifiers.Add(newSpriteModifier);
                        newSpriteModifier.name = animationName;

                        var indexElement = firstFrame.firstElementIdx + 0;
                        var frameElement = data.GetAnimFrameElement(indexElement);
                        LoadSpriteModifier(kanimPrefix, newSpriteModifier, frameElement);

                        //AddSpriteInfo(export, newSpriteModifier, data, frameElement, isUtility && !isUi && !isPlace);

                        continue;
                    }
                    else if (firstFrame.numElements == 3)
                    {
                        string[] place = new string[] { "place", "all_place" };
                        string[] left = new string[] { "place_left", "cap_left_place" };
                        string[] right = new string[] { "place_right", "cap_right_place" };
                        string[] top = new string[] { "cap_top_place" };
                        string[] bottom = new string[] { "cap_bottom_place" };

                        for (
                            int indexElement = firstFrame.firstElementIdx;
                            indexElement < firstFrame.firstElementIdx + firstFrame.numElements;
                            indexElement++)
                        {
                            var frameElement = data.GetAnimFrameElement(indexElement);

                            var newSpriteModifier = new BSpriteModifier();
                            //export.spriteModifiers.Add(newSpriteModifier);

                            LoadSpriteModifier(kanimPrefix, newSpriteModifier, frameElement);

                            if (left.Contains(frameElement.symbol.DebuggerDisplay))
                            {
                                newSpriteModifier.type = SpriteModifierType.Left;
                                newSpriteModifier.name = kanimPrefix + "left";
                            }
                            else if (right.Contains(frameElement.symbol.DebuggerDisplay))
                            {
                                newSpriteModifier.type = SpriteModifierType.Right;
                                newSpriteModifier.name = kanimPrefix + "right";
                            }
                            else if (top.Contains(frameElement.symbol.DebuggerDisplay))
                            {
                                newSpriteModifier.type = SpriteModifierType.Top;
                            }
                            else if (bottom.Contains(frameElement.symbol.DebuggerDisplay))
                            {
                                newSpriteModifier.type = SpriteModifierType.Bottom;
                                newSpriteModifier.name = kanimPrefix + "bottom";
                            }
                            else
                            {
                                newSpriteModifier.type = SpriteModifierType.Place;
                                newSpriteModifier.name = kanimPrefix + "place";
                            }

                            //AddSpriteInfo(export, newSpriteModifier, data, frameElement, (isUtility || isBridge) && !isUi && !isPlace);
                        }

                    }
                    //else if (firstFrame.numElements > 3) Debug.Log("More than 2 elements : " + this.prefabId);
                }

            }

            ExportUtilityConnection(b);
            ExportTileInfo(b, export);
        }

        public static bool AddSpriteInfo(Export export, BSpriteModifier spriteModifier, KAnimFileData data, KAnim.Anim.FrameElement frameElement, bool isSolid)
        {
            // First we check if the spriteInfo was already added
            if (export.uiSprites.Count(s => s.name == spriteModifier.spriteInfoName) == 0)
            {
                foreach (var s in data.build.symbols)
                {
                    // We find the symbol in file that corresponds the the supplied frameElement
                    if (s.hash == frameElement.symbol)
                    {
                        // We get the symbolFrame (uvInfo)
                        try
                        {
                            int symbolFrameIndex = s.frameLookup[frameElement.frame];
                            var newSpriteInfo = new BSpriteInfo(spriteModifier.spriteInfoName, data.build.frames[symbolFrameIndex], data.build.GetTexture(0));

                            // Solid white texture for wire colors
                            // Texture does not exist in Oni, has to be generated by PIXI
                            //if (isSolid) newSpriteInfo.textureName = newSpriteInfo.textureName + "_solid";

                            export.uiSprites.Add(newSpriteInfo);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("ERROR SAVING SPRITEINFO " + frameElement.symbol.DebuggerDisplay);
                            Debug.Log("frameElement.frame : " + frameElement.frame.ToString());
                            Debug.Log("s.frameLookup.Length : " + s.frameLookup.Length.ToString());
                            return false;
                        }

                    }
                }
            }

            return true;
        }

        public static void LoadSpriteModifier(string kanimPrefix, BSpriteModifier spriteModifier, KAnim.Anim.FrameElement frameElement)
        {
            spriteModifier.spriteInfoName = kanimPrefix + frameElement.symbol.DebuggerDisplay + "_" + frameElement.frame;

            //if (logSup) Debug.Log(frameElement.symbol.DebuggerDisplay);

            // From KParser2
            spriteModifier.translation = new BVector2(
                frameElement.transform.m02 * 0.5f,
                frameElement.transform.m12 * -0.5f);

            var scaleX = (float)Math.Sqrt(frameElement.transform.m00 * frameElement.transform.m00 + frameElement.transform.m10 * frameElement.transform.m10);
            var scaleY = (float)Math.Sqrt(frameElement.transform.m01 * frameElement.transform.m01 + frameElement.transform.m11 * frameElement.transform.m11);
            var det = frameElement.transform.m00 * frameElement.transform.m11 - frameElement.transform.m01 * frameElement.transform.m10;
            if (det < 0) scaleY = -scaleY;
            spriteModifier.scale = new BVector2(scaleX, scaleY);

            var sinApprox = 0.5f * (frameElement.transform.m01 / scaleY - frameElement.transform.m10 / scaleX);
            var cosApprox = 0.5f * (frameElement.transform.m00 / scaleX + frameElement.transform.m11 / scaleY);
            spriteModifier.rotation = (float)Math.Atan2(sinApprox, cosApprox);
            if (spriteModifier.rotation < 0) spriteModifier.rotation += (float)(2 * Math.PI);
            spriteModifier.rotation *= (float)(180.0f / Math.PI);

            spriteModifier.multColour = new BColor(frameElement.multColour);

        }

        public void ExportUtilityConnection(BuildingDef b)
        {
            utilities = new List<UtilityInfo>();
            if (b.PowerInputOffset != null && b.RequiresPowerInput)
                utilities.Add(new UtilityInfo() { offset = new BVector2(b.PowerInputOffset), type = UtilityType.POWER_INPUT, isSecondary = false });

            if (b.PowerOutputOffset != null && (b.RequiresPowerOutput || b.GeneratorWattageRating > 0))
                utilities.Add(new UtilityInfo() { offset = new BVector2(b.PowerOutputOffset), type = UtilityType.POWER_OUTPUT, isSecondary = false });

            if (b.InputConduitType != ConduitType.None && b.UtilityInputOffset != null)
                utilities.Add(new UtilityInfo() { offset = new BVector2(b.UtilityInputOffset), type = UtilityInfo.GetUtilityType(b.InputConduitType, true), isSecondary = false });

            if (b.OutputConduitType != ConduitType.None && b.UtilityOutputOffset != null)
                utilities.Add(new UtilityInfo() { offset = new BVector2(b.UtilityOutputOffset), type = UtilityInfo.GetUtilityType(b.OutputConduitType, false), isSecondary = false });

            ISecondaryInput secondaryInput = b.BuildingComplete.GetComponent<ISecondaryInput>();
            if (secondaryInput != null)
                utilities.Add(new UtilityInfo()
                {
                    offset = new BVector2(secondaryInput.GetSecondaryConduitOffset()),
                    type = UtilityInfo.GetUtilityType(secondaryInput.GetSecondaryConduitType(), true),
                    isSecondary = true
                });

            ISecondaryOutput secondaryOutput = b.BuildingComplete.GetComponent<ISecondaryOutput>();
            if (secondaryOutput != null)
                utilities.Add(new UtilityInfo()
                {
                    offset = new BVector2(secondaryOutput.GetSecondaryConduitOffset()),
                    type = UtilityInfo.GetUtilityType(secondaryOutput.GetSecondaryConduitType(), false),
                    isSecondary = true
                });

            LogicPorts logicPorts = b.BuildingComplete.GetComponent<LogicPorts>();
            if (logicPorts != null)
            {
                if (logicPorts.inputPortInfo != null)
                    foreach (var p in logicPorts.inputPortInfo)
                        utilities.Add(new UtilityInfo() { offset = new BVector2(p.cellOffset), type = UtilityInfo.GetLogicUtilityType(p.spriteType, true), isSecondary = false });

                if (logicPorts.outputPortInfo != null)
                    foreach (var p in logicPorts.outputPortInfo)
                        utilities.Add(new UtilityInfo() { offset = new BVector2(p.cellOffset), type = UtilityInfo.GetLogicUtilityType(p.spriteType, false), isSecondary = false });

            }

            LogicGate logicGate = b.BuildingComplete.GetComponent<LogicGate>();
            if (logicGate != null)
            {
                if (logicGate.inputPortOffsets != null)
                    foreach (var c in logicGate.inputPortOffsets)
                        utilities.Add(new UtilityInfo() { offset = new BVector2(c), type = UtilityType.LOGIC_INPUT, isSecondary = false });

                if (logicGate.outputPortOffsets != null)
                    foreach (var c in logicGate.outputPortOffsets)
                        utilities.Add(new UtilityInfo() { offset = new BVector2(c), type = UtilityType.LOGIC_OUTPUT, isSecondary = false });
            }

            WireUtilityNetworkLink wireLink = b.BuildingComplete.GetComponent<WireUtilityNetworkLink>();
            if (wireLink != null)
            {
                utilities.Add(new UtilityInfo() { offset = new BVector2(wireLink.link1), type = UtilityType.POWER_INPUT, isSecondary = false });
                utilities.Add(new UtilityInfo() { offset = new BVector2(wireLink.link2), type = UtilityType.POWER_INPUT, isSecondary = false });
            }
        }

        public void ExportTileInfo(BuildingDef b, Export export)
        {
            if (b.BlockTilePlaceAtlas != null && b.BlockTileAtlas != null)
            {
                this.isTile = true;
                this.textureName = b.BlockTilePlaceAtlas.name;
                if (OniExtract_Game_OnPrefabInit.saveTileTexture) OniExtract_Game_OnPrefabInit.SaveTexture(b.BlockTilePlaceAtlas.name, b.BlockTilePlaceAtlas.texture);
                if (OniExtract_Game_OnPrefabInit.saveTileTexture) OniExtract_Game_OnPrefabInit.SaveTexture(b.BlockTileAtlas.name, b.BlockTileAtlas.texture);

                var toAdd = new string[] { "solid_", "place_" };
                foreach (var suffix in toAdd)
                {
                    var rIndex = 0;
                    var uIndex = 0;
                    var dIndex = 0;
                    var l = false;
                    var r = false;
                    var u = false;
                    var d = false;

                    var motifStart = 40;
                    var currentUv = new BVector2(motifStart, motifStart);

                    var size = 128;
                    var uvSize = new BVector2(size, size);

                    var margin = 30;
                    var motifRepeatedEvery = 208;
                    var deltaPivot = margin / (2f * size + 2f * margin); // Do the math lol

                    for (var indexSpriteSheet = 0; indexSpriteSheet <= 15; indexSpriteSheet++)
                    {
                        var connectionModifier = new BSpriteModifier();
                        connectionModifier.name = kanimPrefix + suffix + BBuildingFinal.connectionString[indexSpriteSheet];
                        connectionModifier.multColour = new BColor(1, 1, 1, 1);
                        connectionModifier.scale = new BVector2(1, 1);
                        connectionModifier.rotation = 0;
                        connectionModifier.tags.Add(SpriteTag.connection);
                        connectionModifier.translation = new BVector2(0, 50);
                        connectionModifier.spriteInfoName = connectionModifier.name;
                        this.sprites.spriteNames.Add(connectionModifier.name);

                        export.spriteModifiers.Add(connectionModifier);
                        if (suffix.Equals("solid_")) connectionModifier.tags.Add(SpriteTag.solid);
                        else if (suffix.Equals("place_")) connectionModifier.tags.Add(SpriteTag.place);


                        var newSourceUv = new BSpriteInfo(connectionModifier.spriteInfoName);
                        export.uiSprites.Add(newSourceUv);
                        if (suffix.Equals("solid_")) newSourceUv.textureName = b.BlockTileAtlas.name;
                        else if (suffix.Equals("place_")) newSourceUv.textureName = b.BlockTilePlaceAtlas.name;

                        //console.log(l+';'+r+';'+u+';'+d);

                        var pivot = new BVector2(0.5f, 0.5f);
                        var uv = new BVector2(currentUv);
                        var sizeS = new BVector2(uvSize);

                        //if (!l && !r && !u && !d) newSourceUv.name = newSourceUv.name + 'None';

                        var connection = 0;

                        if (l) connection += 1;
                        else
                        {
                            uv.x -= margin;
                            sizeS.x += margin;
                            pivot.x += deltaPivot;
                        }
                        if (r) connection += 2;
                        else
                        {
                            sizeS.x += margin;
                            pivot.x -= deltaPivot;
                        }
                        if (u) connection += 4;
                        else
                        {
                            uv.y -= margin;
                            sizeS.y += margin;
                            pivot.y -= deltaPivot;
                        }
                        if (d) connection += 8;
                        else
                        {
                            sizeS.y += margin;
                            pivot.y += deltaPivot;
                        }

                        connectionModifier.tags.Add(BBuildingFinal.connectionTag[connection]);

                        newSourceUv.uvMin = new BVector2(uv);
                        newSourceUv.uvSize = new BVector2(sizeS);
                        newSourceUv.realSize = new BVector2(sizeS.x / 1.28f, sizeS.y / 1.28f);
                        newSourceUv.pivot = new BVector2(pivot);

                        //console.log(newSourceUv);

                        /*
                        console.log(uv);
                        console.log(size);
                        console.log(pivot);
                        */

                        l = !l;

                        rIndex = (rIndex + 1) % 8;
                        if (rIndex == 0) r = !r;

                        uIndex = (uIndex + 1) % 2;
                        if (uIndex == 0) u = !u;

                        dIndex = (dIndex + 1) % 4;
                        if (dIndex == 0) d = !d;

                        currentUv.y += motifRepeatedEvery;
                        if (currentUv.y == motifStart + 4 * motifRepeatedEvery)
                        {
                            currentUv.y = motifStart;
                            currentUv.x += motifRepeatedEvery;
                        }
                    }
                }

            }
        }


        static string[] connectionString = new string[] {
            "noConnection",
            "L",
            "R",
            "LR",
            "U",
            "LU",
            "RU",
            "LRU",
            "D",
            "LD",
            "RD",
            "LRD",
            "UD",
            "LUD",
            "RUD",
            "LRUD"
        };

        static SpriteTag[] connectionTag = new SpriteTag[] {
            SpriteTag.noConnection,
            SpriteTag.L,
            SpriteTag.R,
            SpriteTag.LR,
            SpriteTag.U,
            SpriteTag.LU,
            SpriteTag.RU,
            SpriteTag.LRU,
            SpriteTag.D,
            SpriteTag.LD,
            SpriteTag.RD,
            SpriteTag.LRD,
            SpriteTag.UD,
            SpriteTag.LUD,
            SpriteTag.RUD,
            SpriteTag.LRUD
        };

        public static ViewMode GetViewMode(int hash)
        {
            switch (hash)
            {
                case 0:
                    return ViewMode.Base;
                case 517759365:
                    return ViewMode.Power;
                case 1315322370:
                    return ViewMode.Liquid;
                case -1649085947:
                    return ViewMode.Gas;
                case 122547806:
                    return ViewMode.Automation;
                case -1528777920:
                    return ViewMode.Oxygen;
                case 1692648534:
                    return ViewMode.Conveyor;
                case 347378277:
                    return ViewMode.Decor;
                case -266073674:
                    return ViewMode.Light;
                case 412392084:
                    return ViewMode.Temperature;
                case -1563646600:
                    return ViewMode.Room;
                default:
                    return ViewMode.Unknown;
            }
        }
    }

    public enum ViewMode
    {
        Base,
        Power,
        Liquid,
        Gas,
        Automation,
        Oxygen,
        Conveyor,
        Decor,
        Light,
        Temperature,
        Room,
        Unknown
    }

}
