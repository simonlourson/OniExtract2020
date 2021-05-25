using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OniExtract2
{
    public class BuildMenuCategory
    {
        public int category;
        public string categoryName;
        public string categoryIcon;

        public static string GetName(int category)
        {
            switch (category)
            {
                case -2955855:
                    return "Base";
                case -1528777920:
                    return "Oxygen";
                case 517759365:
                    return "Power";
                case -1060175682:
                    return "Food";
                case -1014650956:
                    return "Plumbing";
                case 557910864:
                    return "HVAC";
                case 8593386:
                    return "Refining";
                case -1098138447:
                    return "Medical";
                case -1264537710:
                    return "Furniture";
                case -154862930:
                    return "Equipment";
                case -1237303894:
                    return "Utilities";
                case -470492617:
                    return "Automation";
                case -1745293257:
                    return "Conveyance";
                case 104161307:
                    return "Rocketry";
                default:
                    return "Unkown";
            }
        }

        public static string GetIcon(int category)
        {
            switch (category)
            {
                case -2955855:
                    return "icon_category_base";
                case -1528777920:
                    return "icon_category_oxygen";
                case 517759365:
                    return "icon_category_electrical";
                case -1060175682:
                    return "icon_category_food";
                case -1014650956:
                    return "icon_category_plumbing";
                case 557910864:
                    return "icon_category_ventilation";
                case 8593386:
                    return "icon_category_refinery";
                case -1098138447:
                    return "icon_category_medical";
                case -1264537710:
                    return "icon_category_furniture";
                case -154862930:
                    return "icon_category_misc";
                case -1237303894:
                    return "icon_category_utilities";
                case -470492617:
                    return "icon_category_automation";
                case -1745293257:
                    return "icon_category_shipping";
                case 104161307:
                    return "icon_category_rocketry";
                default:
                    return "Unkown";
            }
        }

    }

    public class BuildMenuItem
    {
        public int category;
        public string buildingId;

    }
}
