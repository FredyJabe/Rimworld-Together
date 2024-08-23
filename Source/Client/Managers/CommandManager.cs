using System.Collections.Generic;
using RimWorld;
using Shared;
using Verse;
using static Shared.CommonEnumerators;

namespace GameClient
{
    //Class that handles how the client will answer to incoming server commands

    public static class CommandManager
    {
        //Parses the received packet into a command to execute

        public static void ParseCommand(Packet packet)
        {
            CommandData commandData = Serializer.ConvertBytesToObject<CommandData>(packet.contents);

            switch(commandData.commandMode)
            {
                case CommandMode.Op:
                    OnOpCommand();
                    break;

                case CommandMode.Deop:
                    OnDeopCommand();
                    break;

                case CommandMode.Broadcast:
                    OnBroadcastCommand(commandData);
                    break;

                case CommandMode.ForceSave:
                    OnForceSaveCommand();
                    break;

                case CommandMode.SpawnThing:
                    OnSpawnThingCommand(commandData);
                    break;
            }
        }

        //Executes the command depending on the type

        private static void OnOpCommand()
        {
            ServerValues.isAdmin = true;
            ClientValues.ManageDevOptions();
            DialogManager.PushNewDialog(new RT_Dialog_OK("You are now an admin!"));
        }

        private static void OnDeopCommand()
        {
            ServerValues.isAdmin = false;
            ClientValues.ManageDevOptions();
            DialogManager.PushNewDialog(new RT_Dialog_OK("You are no longer an admin!"));
        }

        private static void OnBroadcastCommand(CommandData commandData)
        {
            RimworldManager.GenerateLetter("Server Broadcast", commandData.commandDetails, LetterDefOf.PositiveEvent);
        }

        private static void OnForceSaveCommand()
        {
            if (!ClientValues.isReadyToPlay) DisconnectionManager.DisconnectToMenu();
            else
            {
                ClientValues.SetIntentionalDisconnect(true, DisconnectionManager.DCReason.SaveQuitToMenu);
                SaveManager.ForceSave();
            }
        }

        private static void OnSpawnThingCommand(CommandData commandData)
        {
            string message = "Received items from the server:\n\n";
            string[] thingsToSpawn = commandData.commandDetails.Split('/');
            List<Thing> things = new();

            foreach(string s in thingsToSpawn)
            {
                string[] thingParams = s.Split('|');
                ThingDef thingToSpawn = DefDatabase<ThingDef>.GetNamed(thingParams[0]);

                if (thingToSpawn != null)
                {
                    ThingData thingData = new ThingData();
                    thingData.defName = thingToSpawn.defName;
                    thingData.quantity = int.Parse(thingParams[1]);
                    thingData.quality = "null";
                    thingData.hitpoints = thingToSpawn.BaseMaxHitPoints;

                    things.Add(ThingScribeManager.StringToItem(thingData));

                    message += $"{thingData.defName} x{thingData.quantity}\n";
                }
            }

            if (things.Count > 0)
            {
                TransferManager.GetTransferedItemsToSettlement(things.ToArray(), true, false, false, true);
                RimworldManager.GenerateLetter("Received items", message, LetterDefOf.PositiveEvent);
            }
        }
    }
}
