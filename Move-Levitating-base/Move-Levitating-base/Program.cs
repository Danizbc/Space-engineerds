using Sandbox.Game.EntityComponents;
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



        public void Main(string argument, UpdateType updateSource)

        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            IMyCockpit myCockpit = (IMyCockpit)GridTerminalSystem.GetBlockWithName("Cockpit");

            List<IMyThrust> forwardthrusters = new List<IMyThrust>();
            List<IMyThrust> breakthrusters = new List<IMyThrust>();

            forwardthrusters = GetMyForwardThrusts("ThrustForward");
            breakthrusters = GetMyBreakThrusts("ThrustBackward");


            double speed = myCockpit.GetShipSpeed();

            Echo($"ship speed = {speed.ToString()}");


            if (speed < 2 && 4 > speed)
            {
                foreach (IMyThrust forwardThrust in forwardthrusters)
                {
                    forwardThrust.Enabled = true;
                    forwardThrust.ThrustOverridePercentage = 1;

                }
            }
            else if (speed > 20 && speed < 30)
            {
                foreach (IMyThrust forwardThrust in forwardthrusters)
                {
                    forwardThrust.Enabled = false;
                    forwardThrust.ThrustOverridePercentage = 1;

                }
            }
        }













        //Methode der henter alle break thrustere

        List<IMyThrust> GetMyBreakThrusts(string thrusterName)
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            List<IMyThrust> tempTrust = new List<IMyThrust>();
            List<IMyTerminalBlock> myTerminalBlocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(myTerminalBlocks);


            foreach (IMyThrust thrust in myTerminalBlocks)
            {
                if (thrust.DisplayNameText == $"ThrustBreak")
                {
                    tempTrust.Add(thrust);
                    Echo("Break thrust found");
                }
                else
                {

                }
            }

            return tempTrust;
        }











        //Metode der henter alle forward thrustere

        List<IMyThrust> GetMyForwardThrusts(string thrusterName)
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            List<IMyThrust> tempTrust = new List<IMyThrust>();
            List<IMyTerminalBlock> myTerminalBlocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(myTerminalBlocks);


            foreach (IMyThrust thrust in myTerminalBlocks)
            {
                if (thrust.DisplayNameText == $"ThrustForward")
                {
                    tempTrust.Add(thrust);
                    Echo("Forward thrust found");
                }
                else
                {

                }
            }

            return tempTrust;
        }

    }
}