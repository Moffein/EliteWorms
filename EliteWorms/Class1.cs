using BepInEx;
using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine.AddressableAssets;

namespace EliteWorms
{
    [BepInPlugin("com.Moffein.EliteWorms", "EliteWorms", "1.0.1")]
    public class EliteWorms : BaseUnityPlugin
    {
        public void Awake()
        {
            bool allowMagmaWorm = base.Config.Bind<bool>(new ConfigDefinition("Settings", "Allow Magma Worm"),
                true, new ConfigDescription("Allow Magma Worms to be elite.")).Value;

            bool allowOverloadingWorm = base.Config.Bind<bool>(new ConfigDefinition("Settings", "Allow Overloading Worm"),
                true, new ConfigDescription("Allow Overloading Worms to be elite.")).Value;

            if (allowMagmaWorm)
            {
                CharacterSpawnCard card = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/Base/MagmaWorm/cscMagmaWorm.asset").WaitForCompletion();
                card.noElites = false;
                card.eliteRules = SpawnCard.EliteRules.Default;
            }

            if (allowOverloadingWorm)
            {
                CharacterSpawnCard card = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/Base/ElectricWorm/cscElectricWorm.asset").WaitForCompletion();
                card.noElites = false;
                card.eliteRules = SpawnCard.EliteRules.Default;
            }
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
