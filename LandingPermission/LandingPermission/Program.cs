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

        /*Change theese variables to the correct name of the blocks in your ship.
         * 
         * IMPORTANT THEESE BLOCK MUST BE BUILD BEFORE SCRIPT WILL WORK!
         * 
         * LCD panel with name          = LCD-SavePanel
         * LCD panel with name          = LCD-Accept
         * ANTENNA with name            = Antenna
         * Programable block with name  = Programblock-detecter
         * TimerBlock with name         = TimeUser
         * SoundBlock with name         = GateAlarm
         * 
         * IMPORTANT THEESE BLOCK MUST BE BUILD BEFORE SCRIPT WILL WORK!
         * 
         * This script wil set up an reciver that gets player message and source (ip i guess)
         * if message is the same as string AskPermissionToLand and it knows your IP it will open the doors
         * if it doesnt know you ip it will start alarm and await base user to input text into lcd screen, he got 2 choices accept or denied
         * accept will open door and save ip to an lcd screen for furture visit.
         * denied will close door and activate turrets
         */

        //code you write into screen to allo unknown people.
        string acceptCode = "accept";
        //code you write into screen to deny people
        string deniedCode = "denied";

        //An lcd text panel that save known ips. put down a lcd screen change name to lcd-SavePanel 
        string lcdSavePanel = "LCD-SavePanel";
        //The name of the screen where you write into and if code is = y    then it will save user
        string lcdAcceptPanel = "LCD-Accept";
        //The name of your antenna
        string antennaName = "Antenna";
        //The name of your programmable block
        string programBlockName = "Programblock-detecter";
        //The broadcast is like a radio channel. edit channel 1 to what you desire, be aware to change on both platforms.
        string broadcastChannel = "landing channel";
        //use = looking player that want to land writes "im asking for permission to land" (without "") and the person ip is accepted it will open hangar doors.
        string askPermissionToLand = "im asking for permission to land";
        //Alarm for unknown ips
        string AlarmBlockName = "GateAlarm";



        string TextMessage;


        //Dont touch theese
        bool setupcomplete = false;
        IMyRadioAntenna radio1;
        IMyProgrammableBlock pb;
        IMyTextPanel userIpPanel;
        IMyTextPanel AcceptPanel;
        IMyTimerBlock timerBlock;
        IMySoundBlock soundBlock;
        List<string> knownIp = new List<string>();
        bool running = false;



        public Program()
        {
            userIpPanel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(lcdSavePanel);

            string[] ips = userIpPanel.GetText().Split(' ');
            foreach (string ip in ips)
            {
                knownIp.Add(ip);
            }
        }



        public void Save()
        {


        }



        public void Main(string argument, UpdateType updateSource)
        {
            AcceptPanel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(lcdAcceptPanel);
            soundBlock = (IMySoundBlock)GridTerminalSystem.GetBlockWithName(AlarmBlockName);

            // If setupcomplete is false, run Setup method.
            if (!setupcomplete)
            {
                Echo("Running setup.");
                Setup();
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

                    if (knownIp.Contains(sender.ToString()) && messagetext.Contains(askPermissionToLand.ToLower()))
                    {
                        Echo("Here");
                    }
                    else if (!knownIp.Contains(sender.ToString()) && messagetext.Contains(askPermissionToLand.ToLower()))
                    {
                        //insert alarm for landing and when writing y into panel 
                        soundBlock.Play();
                        while (running == false)
                        {
                            if (AcceptPanel.GetText() == acceptCode)
                            {
                                
                                Gates("y");
                                running = true;
                                knownIp.Add(sender.ToString());
                                foreach (string ipaddress in knownIp)
                                {
                                    TextMessage += $"{ipaddress} \n";
                                }
                                userIpPanel.WriteText(TextMessage);
                            }
                            else if (AcceptPanel.GetText() == deniedCode)
                            {

                                Gates("n");
                                running = true;
                            }
                        }
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