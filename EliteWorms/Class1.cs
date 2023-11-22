using BepInEx;
using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EliteWorms
{
    [BepInPlugin("com.Moffein.EliteWorms", "EliteWorms", "1.0.0")]
    public class EliteWorms : BaseUnityPlugin
    {
        public static HashSet<BodyIndex> bodyIndexWhitelist = new HashSet<BodyIndex>();
        public static List<string> bodyNameWhitelist;
        public static bool applyToAll = false;

        public void Awake()
        {
            ReadConfig();

            On.RoR2.CharacterSpawnCard.Awake += RemoveEliteRestriction;
            RoR2.RoR2Application.onLoad += LoadBodyIndex;
        }

        private void RemoveEliteRestriction(On.RoR2.CharacterSpawnCard.orig_Awake orig, CharacterSpawnCard self)
        {
            if (CheckEligibility(self))
            {
                if (self.eliteRules == SpawnCard.EliteRules.ArtifactOnly) self.eliteRules = SpawnCard.EliteRules.Default;
                self.noElites = false;
            }

            orig(self);
        }

        private void ReadConfig()
        {
            applyToAll = base.Config.Bind<bool>(new ConfigDefinition("Settings", "Apply to All"),
                false, new ConfigDescription("Disables elite restrictions on all enemies. (Overrides other settings)")).Value;

            string bodyNames = base.Config.Bind<string>(new ConfigDefinition("Settings", "Body Whitelist"),
                "MagmaWormBody", new ConfigDescription("Bodies to remove elite restrictions from, separated by comma. ex. MagmaWormBody, ElectricWormBody")).Value;

            //parse bodynames
            bodyNames = new string(bodyNames.ToCharArray().Where(c => !System.Char.IsWhiteSpace(c)).ToArray());
            string[] splitBodies = bodyNames.Split(',');
            foreach (string str in splitBodies)
            {

                bodyNameWhitelist.Add(str);
            }
        }

        private void LoadBodyIndex()
        {
            foreach(string str in bodyNameWhitelist)
            {
                BodyIndex index = BodyCatalog.FindBodyIndexCaseInsensitive(str);
                if (index != BodyIndex.None) bodyIndexWhitelist.Add(index);
            }
        }

        public static bool CheckEligibility(CharacterSpawnCard spawnCard)
        {
            if (applyToAll) return true;
            if (spawnCard.prefab)
            {
                CharacterBody body = spawnCard.prefab.GetComponent<CharacterBody>();
                if (body && bodyIndexWhitelist.Contains(body.bodyIndex)) return true;
            }
            return false;
        }
    }
}

namespace R2API.Utils
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ManualNetworkRegistrationAttribute : Attribute
    {
    }
}
