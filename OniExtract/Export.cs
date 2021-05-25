using LibNoiseDotNet.Graphics.Tools.Noise.Combiner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OniExtract2
{
    public class Export
    {
        public List<BBuildingFinal> buildings;
        public List<BSpriteInfo> uiSprites;
        public List<BSpriteModifier> spriteModifiers;
        public List<BuildMenuCategory> buildMenuCategories;
        public List<BuildMenuItem> buildMenuItems;
        public List<BElement> elements;

        public Export()
        {
            buildings = new List<BBuildingFinal>();
            uiSprites = new List<BSpriteInfo>();
            spriteModifiers = new List<BSpriteModifier>();

            AddCustomSprites();
            AddCustomModifiers();
        }

        public void AddCustomSprites()
        {
            var gas_tile_front = new BSpriteInfo("gas_tile_front");
            gas_tile_front.uvMin = new BVector2(0, 0);
            gas_tile_front.uvSize = new BVector2(128, 128);
            gas_tile_front.realSize = new BVector2(100, 100);
            gas_tile_front.pivot = new BVector2(1, 0);
            gas_tile_front.isIcon = false;
            gas_tile_front.textureName = "gas_tile_front";
            //uiSprites.Add(gas_tile_front);

            var liquid_tile_front = new BSpriteInfo("liquid_tile_front");
            liquid_tile_front.uvMin = new BVector2(0, 0);
            liquid_tile_front.uvSize = new BVector2(128, 128);
            liquid_tile_front.realSize = new BVector2(100, 100);
            liquid_tile_front.pivot = new BVector2(1, 0);
            liquid_tile_front.isIcon = false;
            liquid_tile_front.textureName = "liquid_tile_front";
            //uiSprites.Add(liquid_tile_front);

            var vacuum_tile_front = new BSpriteInfo("vacuum_tile_front");
            vacuum_tile_front.uvMin = new BVector2(0, 0);
            vacuum_tile_front.uvSize = new BVector2(128, 128);
            vacuum_tile_front.realSize = new BVector2(100, 100);
            vacuum_tile_front.pivot = new BVector2(1, 0);
            vacuum_tile_front.isIcon = false;
            vacuum_tile_front.textureName = "vacuum_tile_front";
            //uiSprites.Add(vacuum_tile_front);

            var gas_tile = new BSpriteInfo("gas_tile");
            gas_tile.uvMin = new BVector2(0, 0);
            gas_tile.uvSize = new BVector2(128, 128);
            gas_tile.realSize = new BVector2(100, 100);
            gas_tile.pivot = new BVector2(1, 0);
            gas_tile.isIcon = false;
            gas_tile.textureName = "gas_tile";
            //uiSprites.Add(gas_tile);

        }
    
        public void AddCustomModifiers()
        {
            var gas_tile = new BSpriteModifier();
            gas_tile.name = "gas_tile";
            gas_tile.type = 0;
            gas_tile.spriteInfoName = "gas_tile";
            gas_tile.translation = new BVector2(0, 0);
            gas_tile.scale = new BVector2(1, 1);
            gas_tile.rotation = 0;
            gas_tile.multColour = new BColor(1, 1, 1, 1);
            gas_tile.tags.Add(SpriteTag.element_gas_back);
            //spriteModifiers.Add(gas_tile);

            var gas_tile_front = new BSpriteModifier();
            gas_tile_front.name = "gas_tile_front";
            gas_tile_front.type = 0;
            gas_tile_front.spriteInfoName = "gas_tile_front";
            gas_tile_front.translation = new BVector2(0, 0);
            gas_tile_front.scale = new BVector2(1, 1);
            gas_tile_front.rotation = 0;
            gas_tile_front.multColour = new BColor(1, 1, 1, 1);
            gas_tile_front.tags.Add(SpriteTag.element_gas_front);
            //spriteModifiers.Add(gas_tile_front);

            var liquid_tile_front = new BSpriteModifier();
            liquid_tile_front.name = "liquid_tile_front";
            liquid_tile_front.type = 0;
            liquid_tile_front.spriteInfoName = "liquid_tile_front";
            liquid_tile_front.translation = new BVector2(0, 0);
            liquid_tile_front.scale = new BVector2(1, 1);
            liquid_tile_front.rotation = 0;
            liquid_tile_front.multColour = new BColor(1, 1, 1, 1);
            liquid_tile_front.tags.Add(SpriteTag.element_liquid_front);
            //spriteModifiers.Add(liquid_tile_front);

            var vacuum_tile_front = new BSpriteModifier();
            vacuum_tile_front.name = "vacuum_tile_front";
            vacuum_tile_front.type = 0;
            vacuum_tile_front.spriteInfoName = "vacuum_tile_front";
            vacuum_tile_front.translation = new BVector2(0, 0);
            vacuum_tile_front.scale = new BVector2(1, 1);
            vacuum_tile_front.rotation = 0;
            vacuum_tile_front.multColour = new BColor(1, 1, 1, 1);
            vacuum_tile_front.tags.Add(SpriteTag.element_vacuum_front);
            //spriteModifiers.Add(vacuum_tile_front);

        }
    }
}
