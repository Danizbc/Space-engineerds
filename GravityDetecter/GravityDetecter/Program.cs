﻿using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // This file contains your actual script.
        //
        // You can either keep all your code here, or you can create separate
        // code files to make your program easier to navigate while coding.
        //
        // In order to add a new utility class, right-click on your project, 
        // select 'New' then 'Add Item...'. Now find the 'Space Engineers'
        // category under 'Visual C# Items' on the left hand side, and select
        // 'Utility Class' in the main area. Name it in the box below, and
        // press OK. This utility class will be merged in with your code when
        // deploying your final script.
        //
        // You can also simply create a new utility class manually, you don't
        // have to use the template if you don't want to. Just do so the first
        // time to see what a utility class looks like.

        public Program()
        {
            // The constructor, called only once every session and
            // always before any other method is called. Use it to
            // initialize your script. 
            //     
            // The constructor is optional and can be removed if not
            // needed.
            // 
            // It's recommended to set RuntimeInfo.UpdateFrequency 
            // here, which will allow your script to run itself without a 
            // timer block.
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource)
        {

            IMyCockpit myCockpit = (IMyCockpit)GridTerminalSystem.GetBlockWithName("Cockpit");
            List<IMyThrust> upthrusters = new List<IMyThrust>();



            upthrusters = GetMyUpThrusts("ThrustUP");


            double gravity = myCockpit.GetNaturalGravity().Length() / 9.81;
            double speed = myCockpit.GetShipSpeed();

            Echo($"ship speed = {speed.ToString()}");
            Echo(gravity.ToString());

            if (gravity > 0.30)
            {
                foreach (IMyThrust ThrustUP in upthrusters)
                {
                    ThrustUP.Enabled = true;
                    ThrustUP.ThrustOverridePercentage = 70;

                }
                Echo("im here 1");
            }
            else
            {
                foreach (IMyThrust ThrustUP in upthrusters)
                {
                    ThrustUP.Enabled = false;
                    ThrustUP.ThrustOverridePercentage = 0;

                }
                Echo("im here 2");
            }

        }


        // New method here

        List<IMyThrust> GetMyUpThrusts(string thrusterName)
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            List<IMyThrust> tempTrust = new List<IMyThrust>();
            List<IMyTerminalBlock> myTerminalBlocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(myTerminalBlocks);


            foreach (IMyThrust thrust in myTerminalBlocks)
            {
                if (thrust.DisplayNameText == thrusterName)
                {
                    tempTrust.Add(thrust);
                    Echo("Up thrust found");

                }
                else
                {

                }
            }

            return tempTrust;
        }





    }
}