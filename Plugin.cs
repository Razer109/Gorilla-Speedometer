using BepInEx;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utilla;


namespace Spedometer
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        private GameObject speedTextObject;
        private TextMeshPro speedTextMesh;
        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
          
            speedTextObject = new GameObject("SpeedText");

         
            speedTextMesh = speedTextObject.AddComponent<TextMeshPro>();

         
            speedTextMesh.text = "";
            speedTextObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            speedTextMesh.fontSize = 10;
            speedTextMesh.alignment = TextAlignmentOptions.Center;
            speedTextMesh.color = Color.white;
            InvokeRepeating("SpeedometerUpdate", 0, 0.1f);
        }
        void Update()
        {
            if (inRoom)
            {
                if (GorillaLocomotion.Player.Instance != null)
                {
                    speedTextObject.transform.position = GorillaTagger.Instance.leftHandTransform.position + new Vector3(0.1f, 0f, 0f);
                    speedTextObject.transform.eulerAngles = new Vector3(GorillaTagger.Instance.leftHandTransform.rotation.x + 360, GorillaTagger.Instance.leftHandTransform.rotation.y, GorillaTagger.Instance.leftHandTransform.rotation.z);
                }
            }
        }
        void SpeedometerUpdate()
        {
            if (inRoom)
            {
                speedTextMesh.gameObject.SetActive(true);
                if (GorillaLocomotion.Player.Instance != null)
                {

                    speedTextMesh.text = GorillaLocomotion.Player.Instance.currentVelocity.magnitude.ToString("0");
                }
            }
            else
            {
                speedTextMesh.gameObject.SetActive(false);
            }

           }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
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
