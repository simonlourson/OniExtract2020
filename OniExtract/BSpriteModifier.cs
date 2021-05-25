using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OniExtract2
{
    public class BSpriteModifier
    {
        public string name;
        public SpriteModifierType type;
        public string spriteInfoName;

        public BVector2 translation;
        public BVector2 scale;
        public float rotation;

        public BColor multColour;
        public List<SpriteTag> tags;
        /*
        public BVector2 framebboxMin;
        public BVector2 framebboxMax;
        public BVector3 center;
        public BVector3 range;
        public float width;
        public float height;
        public float depth;
        public List<BSpriteModifierPart> parts;
        */

        public BSpriteModifier()
        {
            tags = new List<SpriteTag>();
        }
    }

    public enum SpriteModifierType
    {
        Place,
        Solid,
        UI,
        Left,
        Right,
        Top,
        Bottom
    }

    public enum SpriteTag
    {
        solid,
        place,
        ui,
        connection,
        tileable,
        tileable_left,
        tileable_right,
        tileable_up,
        tileable_down,
        noConnection,
        L,
        R,
        LR,
        U,
        LU,
        RU,
        LRU,
        D,
        LD,
        RD,
        LRD,
        UD,
        LUD,
        RUD,
        LRUD,
        none,
        white,
        element_gas_back,
        element_gas_front,
        element_liquid_back,
        element_liquid_front,
        element_vacuum_front
    }


}
