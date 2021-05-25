using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OniExtract2
{
    public class BSpriteInfo
    {
        public string name;
        public string textureName;
        public bool isIcon;
        public bool isInputOutput;
        public BVector2 uvMin;
        public BVector2 uvSize;
        public BVector2 realSize;
        public BVector2 pivot;

        public BSpriteInfo(string name)
        {
            this.name = name;
        }

        public BSpriteInfo(string name, KAnim.Build.SymbolFrame symbolFrame, Texture2D texture)
        {
            this.name = name;
            this.isIcon = name.Contains("_ui");
            textureName = texture.name;
            uvMin = new BVector2((int)(symbolFrame.uvMin.x * texture.width), (int)((1 - symbolFrame.uvMin.y) * texture.height));
            uvSize = new BVector2(
                (symbolFrame.uvMax.x - symbolFrame.uvMin.x) * texture.width,
                (symbolFrame.uvMin.y - symbolFrame.uvMax.y) * texture.height
                );

            var framePivot = new BVector2(
                (symbolFrame.bboxMax.x + symbolFrame.bboxMin.x) / 2,
                (symbolFrame.bboxMax.y + symbolFrame.bboxMin.y) / 2
                );

            var framePivotSize = new BVector2(
                (symbolFrame.bboxMax.x - symbolFrame.bboxMin.x),
                (symbolFrame.bboxMax.y - symbolFrame.bboxMin.y)
                );

            // From KParser2
            var xy = new BVector2(
                framePivot.x - framePivotSize.x / 2f,
                framePivot.y - framePivotSize.y / 2f
                );

            pivot = new BVector2(
                0 - xy.x / framePivotSize.x,
                1 + xy.y / framePivotSize.y
                );

            realSize = new BVector2(
                (framePivotSize.x / 2),
                (framePivotSize.y / 2)
                );
        }

        public BSpriteInfo(Sprite sprite, string textureName, Texture2D texture)
        {
            name = sprite.name;
            this.textureName = textureName;
            isIcon = true;
            uvMin = new BVector2((float)Math.Round(sprite.textureRect.x), (float)Math.Round(texture.height - sprite.textureRect.y - sprite.textureRect.height));
            uvSize = new BVector2((float)Math.Round(sprite.textureRect.width), (float)Math.Round(sprite.textureRect.height));
            realSize = new BVector2((float)Math.Round(sprite.textureRect.width), (float)Math.Round(sprite.textureRect.height));
            pivot = new BVector2(sprite.pivot.x / uvSize.x, sprite.pivot.y / uvSize.y);
        }
    }
}
