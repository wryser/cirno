using BepInEx;
using System;
using UnityEngine;
using Utilla;
using System.Reflection;
using System.IO;
using UnityEngine.XR;

namespace cirno
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        GameObject realfumo;
        GameObject handl;
        GameObject baka;
        bool Trigger;
        float nextBaka;
        float bakacooldown = 3f;
        bool canBaka = true;
        private readonly XRNode rNode = XRNode.RightHand;

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("cirno.Assets.cirnofumo");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject fumo = bundle.LoadAsset<GameObject>("cirno");
            realfumo = Instantiate(fumo);
            baka = bundle.LoadAsset<GameObject>("baka");
            baka.AddComponent<BakaRemove>();

            handl = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/");
            realfumo.transform.SetParent(handl.transform, false);
            realfumo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            realfumo.transform.localRotation = Quaternion.Euler(0f, 270f, 90f);
            realfumo.transform.localPosition = new Vector3(-0.05f, 0f, 0.07f);
        }

        void FixedUpdate()
        {
            /* Code here runs every frame when the mod is enabled */
            if (Time.time > nextBaka)
            {
                canBaka = true;
                nextBaka = Time.time + bakacooldown;
            }
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out Trigger);

            if (Trigger)
            {
                if (canBaka)
                {
                    Instantiate(baka);
                    canBaka = false;
                }
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
