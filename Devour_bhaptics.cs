using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;
using Photon.Bolt;

namespace Devour_bhaptics
{
    public class Devour_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;
        public static NetworkId myNetworkId;
        public override void OnInitializeMelon()
        {
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }

        [HarmonyPatch(typeof(BoltEntity), "Awake", new Type[] {  })]
        public class bhaptics_EntityInitilization
        {
            [HarmonyPostfix]
            public static void Postfix(BoltEntity __instance)
            {
                myNetworkId = __instance.NetworkId;
                tactsuitVr.LOG("NetworkId: " + myNetworkId.Entity.ToString());
                tactsuitVr.LOG("Local? " + (myNetworkId == __instance.NetworkId).ToString());
            }
        }

        [HarmonyPatch(typeof(NolanBehaviour), "OnReviveEvent", new Type[] { typeof(BoltEntity), typeof(BoltEntity) })]
        public class bhaptics_PlayerRevived
        {
            [HarmonyPostfix]
            public static void Postfix(NolanBehaviour __instance, BoltEntity player)
            {
                if (player.NetworkId != myNetworkId) return;
                tactsuitVr.PlaybackHaptics("Revived");
            }
        }

        [HarmonyPatch(typeof(SurvivalObjectBurnController), "BurnGoat", new Type[] {  })]
        public class bhaptics_BurnGoat
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("GoatBurned");
            }
        }

        [HarmonyPatch(typeof(NolanBehaviour), "OnDemonHoldingPlayer", new Type[] { typeof(UnityEngine.GameObject), typeof(BoltEntity) })]
        public class bhaptics_DemonHoldingPlayer
        {
            [HarmonyPostfix]
            public static void Postfix(NolanBehaviour __instance, BoltEntity player)
            {
                if (player.NetworkId != myNetworkId) return;
                tactsuitVr.PlaybackHaptics("GrabbedByDemon");
            }
        }

        [HarmonyPatch(typeof(SurvivalAzazelBehaviour), "PlayEnrageEffect", new Type[] { typeof(float) })]
        public class bhaptics_AzazelEnraged
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("DemonEnraged");
            }
        }

    }
}
