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



        //code the sender need to send to you.
        string acceptCode = "666";


        //An lcd text panel that save known ips. put down a lcd screen change name to lcd-SavePanel 
        string lcdSavePanel = "LCD-SavePanel";


        //The name of your antenna
        string antennaName = "Antenna";

        //The name of your programmable block
        string programBlockName = "Programblock-detecter";

        //The broadcast is like a radio channel. edit channel 1 to what you desire, be aware to change on both platforms.
        string broadcastChannel = "Channel-1";


        //Alarm for unknown ips
        string AlarmBlockName = "GateAlarm";

        //An lcd screen for messages
        string messagePanelName = "MessagePanel";




        //Dont touch theese
        string TextMessage = "";
        bool setupcomplete = false;
        IMyRadioAntenna radio1;
        IMyProgrammableBlock pb;
        IMyTextPanel userIpPanel;
        IMyTextPanel AcceptPanel;
        IMySoundBlock soundBlock;
        List<string> knownIp = new List<string>();



        public void SaveData()
        {

            userIpPanel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(lcdSavePanel);
            string[] ips = userIpPanel.GetText().Split(' ');
            for (int i = 0; i < ips.Length; i++)
            {
                knownIp.Add(ips[i]);
                Echo(ips[i]);
            }

        }




        public void Main(string argument, UpdateType updateSource)
        {


            soundBlock = (IMySoundBlock)GridTerminalSystem.GetBlockWithName(AlarmBlockName);
            Runtime.UpdateFrequency = UpdateFrequency.Update100;


            IMyTextPanel MessagePanel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(messagePanelName);










            // If setupcomplete is false, run Setup method.
            if (!setupcomplete)
            {

                Echo("Running setup.");
                Setup();
                SaveData();

            }
            else
            {


                // To create a listener, we use IGC to access the relevant method.
                // We pass the same tag argument we used for our message. 
                IGC.RegisterBroadcastListener(broadcastChannel);


                // Create a list for broadcast listeners.
                List<IMyBroadcastListener> listeners = new List<IMyBroadcastListener>();

                // The method argument below is the list we wish IGC to populate with all Listeners we've made.
                // Our Listener will be at index 0, since it's the only one we've made so far.
                IGC.GetBroadcastListeners(listeners);

                foreach (var item in listeners)
                {
                    Echo("Somone listen");
                }
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

                    MessagePanel.WriteText("Error");

                    if (knownIp.Contains(sender.ToString()))
                    {
                        Echo("Here222");
                        Gates("y");
                        MessagePanel.WriteText(messagetext);

                    }
                    else if (messagetext.Contains(acceptCode))
                    {
                        MessagePanel.WriteText("Err33or");
                        soundBlock.Play();

                        MessagePanel.WriteText("accepted part 2");


                        Echo("Accepted");

                        Gates("y");


                        if (knownIp.Contains(sender.ToString()))
                        {
                            MessagePanel.WriteText("accepted part 3");
                        }
                        else
                        {
                            knownIp.Add(sender.ToString());
                            MessagePanel.WriteText(messagetext);
                        }

                        TextMessage = "";
                        for (int i = 0; i < knownIp.Count; i++)
                        {


                            TextMessage += $"{knownIp[i]}\n";

                        }

                        userIpPanel.WriteText(TextMessage);



                    }
                    else
                    {
                        Echo("Im here");
                        MessagePanel.WriteText("Acces denied" + "\n" + TextMessage);
                        Gates("n");

                    }




                }

            }
        }

        public void Gates(string input)
        {
            List<IMyAirtightHangarDoor> hangarDoors = new List<IMyAirtightHangarDoor>();
            GridTerminalSystem.GetBlocksOfType<IMyAirtightHangarDoor>(hangarDoors);
            foreach (IMyAirtightHangarDoor door in hangarDoors)
            {
                if (input == "y")
                {
                    door.OpenDoor();
                }
                else
                {
                    door.CloseDoor();
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
