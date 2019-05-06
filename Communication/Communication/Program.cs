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

        bool setupcomplete = false;


        IMyRadioAntenna radio1;

        IMyProgrammableBlock pb;


        //IMyIntergridCommunicationSystem radio2;
        //List<IMyBroadcastListener> myBroadcastListeners = new List<IMyBroadcastListener>();
        //radio2.SendBroadcastMessage("Tag", "Hello World", TransmissionDistance.TransmissionDistanceMax);

        public void Main(string argument, UpdateType updateSource)
        {
            // If setupcomplete is false, run Setup method.
            if (!setupcomplete)
            {
                Echo("Running setup.");
                Setup();
            }
            else
            {
                // Create a tag. Our friend will use this in his script in order to receive our messages.
                string tag1 = "Channel-1";

                // Create our message. We first make it a string, and then we "box" it as an object type.               
                string messageOut = "Hello world";

                // Through the IGC variable we issue the broadcast method. IGC is "pre-made",
                // so we don't have to declare it ourselves, just go ahead and use it. 
                IGC.SendBroadcastMessage(tag1, messageOut, TransmissionDistance.TransmissionDistanceMax);


                // To create a listener, we use IGC to access the relevant method.
                // We pass the same tag argument we used for our message. 
                IGC.RegisterBroadcastListener(tag1);

            }
        }


        public void Setup()
        {
            radio1 = (IMyRadioAntenna)GridTerminalSystem.GetBlockWithName("Ant");
            pb = (IMyProgrammableBlock)GridTerminalSystem.GetBlockWithName("Programmable block");

            // Connect the PB to the antenna. This can also be done from the grid terminal.
            radio1.AttachedProgrammableBlock = pb.EntityId;

            if (radio1 != null)
            {

                Echo("SetupC omplete");
                setupcomplete = true;
            }
            else
            {
                Echo("Setup failed");
            }

        }
    }
}