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



        //Change theese variables to the correct name of the blocks in your ship.
        string remoteControllerName = "RemoteControll";
        string lcdName = "LCD";
        string antennaName = "Antenna";
        string programBlockName = "Programmable block";
        //The broadcast is like a radio channel. edit channel 1 to what you desire, be aware to change on both platforms.
        string broadcastChannel = "Channel-2";


        MyWaypointInfo wayPoint = new MyWaypointInfo();


        //Dont touch theese
        bool setupcomplete = false;
        IMyRadioAntenna radio1;
        IMyProgrammableBlock pb;





        public void Main(string argument, UpdateType updateSource)
        {
            IMyTextPanel lcdWriteScreen = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(lcdName);


            IMyRemoteControl remoteControl = (IMyRemoteControl)GridTerminalSystem.GetBlockWithName(remoteControllerName);

            List<MyWaypointInfo> mywaypoint = new List<MyWaypointInfo>();

            remoteControl.GetWaypointInfo(mywaypoint);

            foreach (MyWaypointInfo item in mywaypoint)
            {
                Echo(item.Coords.ToString());
            }
            wayPoint = mywaypoint[0];

            Echo(wayPoint.ToString());

            // If setupcomplete is false, run Setup method.
            if (!setupcomplete)
            {
                Echo("Running setup.");
                Setup();
            }
            else
            {

                // Create our message. We first make it a string, and then we "box" it as an object type.               
                string sendCords = lcdWriteScreen.GetText();

                // Through the IGC variable we issue the broadcast method. IGC is "pre-made",
                // so we don't have to declare it ourselves, just go ahead and use it. 
                if (sendCords.ToLower() == "send" && wayPoint.Coords != null)
                {
                    IGC.SendBroadcastMessage(broadcastChannel, wayPoint.ToString(), TransmissionDistance.TransmissionDistanceMax);
                }
                else
                {
                    Echo("Broadcasting failed.");
                }
                // To create a listener, we use IGC to access the relevant method.
                // We pass the same tag argument we used for our message. 
                IGC.RegisterBroadcastListener(broadcastChannel);

            }
        }


        public void Setup()
        {
            radio1 = (IMyRadioAntenna)GridTerminalSystem.GetBlockWithName(antennaName);
            pb = (IMyProgrammableBlock)GridTerminalSystem.GetBlockWithName(programBlockName);

            // Connect the PB to the antenna. This can also be done from the grid terminal.
            radio1.AttachedProgrammableBlock = pb.EntityId;

            if (radio1 != null)
            {

                Echo("Setup Complete");
                setupcomplete = true;
            }
            else
            {
                Echo("Setup failed");
            }

        }
    }
}