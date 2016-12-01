using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	public class Skills
	{
		public static int skillsCount = 25;
		public static String[] skillNames = {
        "attack", "defence", "strength", "hitpoints", "ranged", "prayer", "magic", "cooking", "woodcutting", "fletching", 
        "fishing", "firemaking", "crafting", "smithing", "mining", "herblore", "agility", "thieving", "slayer", "farming", 
        "runecraft", "-unused-", "-unused-", "-unused-", "-unused-"
    };
		public static bool[] skillEnabled = {
        true, true, true, true, true, true, true, true, true, true, 
        true, true, true, true, true, true, true, true, true, false, 
        true, false, false, false, false
    };
	}
}
