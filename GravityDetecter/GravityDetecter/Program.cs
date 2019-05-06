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

            IMyCockpit myCockpit = (IMyCockpit)GridTerminalSystem.GetBlockWithName("");
            List<IMyThrust> thrusters = new List<IMyThrust>();

            thrusters = GetMyThrusts();

            double gravity = myCockpit.GetNaturalGravity().Length() / 9.81;

            if (gravity < 0.5 )
            {
                foreach (IMyThrust ThrustUP in thrusters)
                {
                    ThrustUP.Enabled = true;
                }
            }
            else
            {
                foreach (IMyThrust ThrustUP in thrusters)
                {
                    ThrustUP.Enabled = false;
                }
            }
            
        }

        List<IMyThrust> GetMyThrusts()
        {
            List<IMyThrust> tempTrust = new List<IMyThrust>();
            List<IMyTerminalBlock> myTerminalBlocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyThrust>(myTerminalBlocks);


            foreach (IMyThrust thrust in myTerminalBlocks)
            {
                if (thrust.DisplayNameText == $"ThrustUp")
                {
                    tempTrust.Add(thrust);
                }
                else
                {

                }
            }

            return tempTrust;
        }


        
    }
}