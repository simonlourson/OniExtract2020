using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OniExtract2
{
    public class UtilityInfo
    {
        public BVector2 offset;
        public UtilityType type;
        public bool isSecondary;

        public static UtilityType GetUtilityType(ConduitType type, bool input)
        {
            if (type == ConduitType.Gas && input) return UtilityType.GAS_INPUT;
            if (type == ConduitType.Gas && !input) return UtilityType.GAS_OUTPUT;
            if (type == ConduitType.Liquid && input) return UtilityType.LIQUID_INPUT;
            if (type == ConduitType.Liquid && !input) return UtilityType.LIQUID_OUTPUT;
            if (type == ConduitType.Solid && input) return UtilityType.SOLID_INPUT;
            if (type == ConduitType.Solid && !input) return UtilityType.SOLID_OUTPUT;

            return UtilityType.NONE;
        }

        public static UtilityType GetLogicUtilityType(LogicPortSpriteType type, bool input)
        {
            if (type == LogicPortSpriteType.Input && input) return UtilityType.LOGIC_INPUT;
            if (type == LogicPortSpriteType.Output && !input) return UtilityType.LOGIC_OUTPUT;
            if (type == LogicPortSpriteType.ControlInput) return UtilityType.LOGIC_CONTROL_INPUT;
            if (type == LogicPortSpriteType.ResetUpdate) return UtilityType.LOGIC_RESET_UPDATE;
            if (type == LogicPortSpriteType.RibbonInput && input) return UtilityType.LOGIC_RIBBON_INPUT;
            if (type == LogicPortSpriteType.RibbonOutput && !input) return UtilityType.LOGIC_RIBBON_OUTPUT;

            return UtilityType.NONE;
        }
    }
}
