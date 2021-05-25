using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OniExtract2
{
    public class SpriteName
    {
        public string groupName;
        public List<string> spriteNames;

        public SpriteName(string groupName)
        {
            this.groupName = groupName;
            this.spriteNames = new List<string>();
        }
    }
}
