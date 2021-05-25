using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OniExtract2
{
    public abstract class UiScreen
    {
        public string id;
        public List<string> inputs;

        public UiScreen(string id)
        {
            this.id = id;
            this.inputs = new List<string>();
        }
    }

    public class BSingleSliderSideScreen : UiScreen
    {
        public string title;
        public string sliderUnits;
        public float min;
        public float max;
        public string tooltip;
        public int sliderDecimalPlaces;
        public float defaultValue;

        public BSingleSliderSideScreen(string id) : base(id)
        {
            this.inputs.Add("number");
        }
    }

    public class BThresholdSwitchSideScreen : UiScreen
    {
        public string title;
        public string aboveToolTip;
        public string belowToolTip;
        public string thresholdValueName;
        public string thresholdValueUnits;
        public float rangeMin;
        public float rangeMax;
        public int incrementScale;
        public float defaultValue;
        public bool defaultBoolean;

        public BThresholdSwitchSideScreen(string id) : base(id)
        {
            this.inputs.Add("number");
            this.inputs.Add("boolean");
        }
    }

    public class BActiveRangeSideScreen : UiScreen
    {
        public float minValue;
        public float maxValue;
        public float defaultActivateValue;
        public float defaultDeactivateValue;
        public string title;
        public string activateSliderLabelText;
        public string deactivateSliderLabelText;
        public string activateTooltip;
        public string deactivateTooltip;

        public BActiveRangeSideScreen(string id) : base(id)
        {
            this.inputs.Add("number");
            this.inputs.Add("number");
        }
    }

    public class BBitSelectorSideScreen : UiScreen
    {
        public string title;
        public string description;
        public BBitSelectorSideScreen(string id) : base(id)
        {
            this.inputs.Add("number");
        }
    }
}
