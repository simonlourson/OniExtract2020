using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OniExtract2.Patches;

namespace OniExtract2
{
    public class BElement
    {
        public string name;
        public string id;
        public int tag;
        public List<string> oreTags;
        public int buildMenuSort;

        public int color;
        public int conduitColor;
        public int uiColor;

        public string icon;
        private string textureName;
        private string kanimPrefix;

        public BElement(Element e, Export export)
        {

            this.name = e.name;
            this.id = e.id.ToString();
            this.tag = e.tag.GetHash();
            Debug.Log("*****************");
            Debug.Log(e.name);
            //int startIndex = this.name.IndexOf("\">");
            //if (startIndex != -1) this.name = this.name.Substring(startIndex + 2);
            //int endIndex = this.name.IndexOf("</");
            //if (endIndex != -1) this.name = this.name.Substring(0, endIndex);

            //element.materialCategory = e.materialCategory.Name;
            this.buildMenuSort = e.buildMenuSort;

            this.oreTags = new List<string>();
            foreach (var t in e.oreTags)
                this.oreTags.Add(t.Name);

            var substance = e.substance;

            this.color = (substance.colour.r << 16) | (substance.colour.g << 8) | (substance.colour.b << 0);
            this.conduitColor = (substance.conduitColour.r << 16) | (substance.conduitColour.g << 8) | (substance.conduitColour.b << 0);
            this.uiColor = (substance.uiColour.r << 16) | (substance.uiColour.g << 8) | (substance.uiColour.b << 0);

            export.elements.Add(this);

            if (this.oreTags.Contains("Gas") || this.oreTags.Contains("Liquid")) return;

            var data = substance.anim.GetData();

            if (data.build.textureCount > 0)
            {
                textureName = data.build.GetTexture(0).name;
                if (OniExtract_Game_OnPrefabInit.saveSubstanceTexture) OniExtract_Game_OnPrefabInit.SaveTexture(textureName, data.build.GetTexture(0));
            }

            kanimPrefix = e.id.ToString() + "_";
            for (int indexGetAnim = 0; indexGetAnim < data.animCount; ++indexGetAnim)
            {
                var anim = data.GetAnim(indexGetAnim);
                Debug.Log(anim.name);

                bool isUi = anim.name.Equals("ui");
                if (!isUi) continue;

                var animationName = kanimPrefix + anim.name;

                var firstFrame = anim.GetFrame(anim.animFile.animBatchTag, 0);

                if (firstFrame.numElements == 0)
                {
                    Debug.Log("0 element for : " + animationName);
                    continue;
                }

                if (firstFrame.numElements == 1)
                {
                    var newSpriteModifier = new BSpriteModifier();
                    export.spriteModifiers.Add(newSpriteModifier);
                    newSpriteModifier.name = animationName;

                    var indexElement = firstFrame.firstElementIdx + 0;
                    var frameElement = data.GetAnimFrameElement(indexElement);

                    BBuildingFinal.LoadSpriteModifier(kanimPrefix, newSpriteModifier, frameElement);
                    BBuildingFinal.AddSpriteInfo(export, newSpriteModifier, data, frameElement, false);

                    icon = newSpriteModifier.spriteInfoName;

                    continue;
                }
                else if (firstFrame.numElements > 1) Debug.Log("More than 2 elements : " + this.name);
            }

            
        }
    }
}
