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
        string lcdTextSender = "lcdTextSender";
        string MessageReciverScreen = "lcdReciever";
        string antennaName = "Antenna";
        string programBlockName = "Programmable block";
        //The broadcast is like a radio channel. edit channel 1 to what you desire, be aware to change on both platforms.
        string broadcastChannel = "Channel 1";


        //Dont touch theese
        bool setupcomplete = false;
        IMyRadioAntenna radio1;
        IMyProgrammableBlock pb;
        string allmessage = "";




        public void Main(string argument, UpdateType updateSource)
        {
            //screen that you write text into and send
            IMyTextPanel lcdWritingScreen = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(lcdTextSender);

            //screen that recives
            IMyTextPanel lcdRecieverScreen = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(MessageReciverScreen);



            // If setupcomplete is false, run Setup method.
            if (!setupcomplete)
            {
                Echo("Running setup.");
                Setup();
            }
            else
            {

                // Create our message. We first make it a string, and then we "box" it as an object type.               
                string messageOut = lcdWritingScreen.GetText();

                // Through the IGC variable we issue the broadcast method. IGC is "pre-made",
                // so we don't have to declare it ourselves, just go ahead and use it. 
                IGC.SendBroadcastMessage(broadcastChannel, messageOut, TransmissionDistance.TransmissionDistanceMax);

                // To create a listener, we use IGC to access the relevant method.
                // We pass the same tag argument we used for our message. 
                IGC.RegisterBroadcastListener(broadcastChannel);

                // Create a list for broadcast listeners.
                List<IMyBroadcastListener> listeners = new List<IMyBroadcastListener>();

                // The method argument below is the list we wish IGC to populate with all Listeners we've made.
                // Our Listener will be at index 0, since it's the only one we've made so far.
                IGC.GetBroadcastListeners(listeners);

                if (listeners[0].HasPendingMessage)
                {
                    // Let's create a variable for our new message. 
                    // Remember, messages have the type MyIGCMessage.
                    MyIGCMessage message = new MyIGCMessage();

                    // Time to get our message from our Listener (at index 0 of our Listener list). 
                    // We do this with the following method:
                    message = listeners[0].AcceptMessage();


                    // A message is a struct of 3 variables. To read the actual data, 
                    // we access the Data field, convert it to type string (unboxing),
                    // and store it in the variable messagetext.
                    string messagetext = message.Data.ToString();

                    // We can also access the tag that the message was sent with.
                    string messagetag = message.Tag;

                    //Here we store the "address" to the Programmable Block (our friend's) that sent the message.
                    long sender = message.Source;

                    //Do something with the information!
                    Echo("Message received with tag" + messagetag + "\n\r");
                    Echo("from address " + sender.ToString() + ": \n\r");
                    Echo(messagetext);



                    allmessage += $"\n new message \n {messagetext}";
                    lcdRecieverScreen.WriteText(allmessage);
                }
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