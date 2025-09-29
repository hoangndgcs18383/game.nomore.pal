// This file is auto-generated. DO NOT EDIT

namespace SAGE.Framework.Core.Addressable
{
    public static class BaseScreenAddress
    {
        public static string GetName(string key)
        {
            switch (key)
            {
				case "UIGameplay":
					return "b3c2db6c51c485c409215bc899e79767";
				case "UIComplete":
					return "f7c32933797febf44846d5c90cb370e6";
				case "UIIntro":
					return "878ef71f089706441a6f5f141a12cc5e";
				case "UIOuttro":
					return "62e138308edf41e48a984cbe7df36aca";

            }
            return null;
        }
        
        public static bool IsResources(string key)
        {
            switch (key)
            { 

            }
            return false;
        }
        
		public static string UIGAMEPLAY = "UIGameplay";
		public static string UICOMPLETE = "UIComplete";
		public static string UIINTRO = "UIIntro";
		public static string UIOUTTRO = "UIOuttro";

    }
}