﻿using Harmony;
using PeterHan.PLib;
using PeterHan.PLib.Lighting;
using System.Collections.Generic;
using UnityEngine;
using static SkyLib.Logger;
using static SkyLib.OniUtils;

namespace ExpandedLights {
	public class ExpandedLightsPatch
    {
        public static string ModName = "ExpandedLights";
        public static bool didStartUp_Building = false;
        public static bool didStartUp_Db = false;

		/// <summary>
		/// Light shape: Directed cone according to component rotation
		/// </summary>
		public static PLightShape DirectedCone, Beam5, SmoothCircle, OffsetCone;

		public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
				PUtil.LogModInit();
                StartLogging(ModName);
				DirectedCone = PLightShape.Register("SkyLib.LightShape.Cone", LightDefs.DoLightCone);
                Beam5 = PLightShape.Register("SkyLib.LightShape.Beam5", LightDefs.LinearLight5);
                SmoothCircle = PLightShape.Register("SkyLib.LightShape.Circle", LightDefs.DoLightCircle);
                OffsetCone = PLightShape.Register("SkyLib.LightShape.Circle", LightDefs.DoOffsetCone);
            }
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Path
        {
            public static void Prefix()
            {
                if (!didStartUp_Building)
                {
                    AddBuildingStrings(FloodlightConfig.Id, FloodlightConfig.DisplayName, FloodlightConfig.Description, FloodlightConfig.Effect);
                    AddBuildingStrings(LEDLightConfig.Id, LEDLightConfig.DisplayName, LEDLightConfig.Description, LEDLightConfig.Effect);
                    AddBuildingStrings(TileLightConfig.Id, TileLightConfig.DisplayName, TileLightConfig.Description, FloodlightConfig.Effect);

                    AddBuildingToBuildMenu("Furniture", FloodlightConfig.Id);
                    AddBuildingToBuildMenu("Furniture", LEDLightConfig.Id);
                    AddBuildingToBuildMenu("Furniture", TileLightConfig.Id);
                    didStartUp_Building = true;
                }
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                if (!didStartUp_Db)
                {
                    AddBuildingToTech("PrettyGoodConductors", FloodlightConfig.Id);
                    AddBuildingToTech("Artistry", TileLightConfig.Id);
                    AddBuildingToTech("Catalytics", LEDLightConfig.Id);
                    didStartUp_Db = true;
                }
            }
        }
    }
}