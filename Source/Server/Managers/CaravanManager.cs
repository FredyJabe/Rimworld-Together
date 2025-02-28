﻿using Shared;
using static Shared.CommonEnumerators;

namespace GameServer
{
    public static class CaravanManager
    {
        //Variables

        private static readonly string fileExtension = ".mpcaravan";

        private static readonly double baseMaxTimer = 86400000;

        public static void ParsePacket(ServerClient client, Packet packet)
        {
            CaravanData data = Serializer.ConvertBytesToObject<CaravanData>(packet.contents);

            switch (data._stepMode)
            {
                case CaravanStepMode.Add:
                    AddCaravan(client, data);
                    break;

                case CaravanStepMode.Remove:
                    RemoveCaravan(client, data);
                    break;

                case CaravanStepMode.Move:
                    MoveCaravan(client, data);
                    break;
            }
        }

        private static void AddCaravan(ServerClient client, CaravanData data)
        {
            data._caravanFile.ID = GetNewCaravanID();
            RefreshCaravanTimer(data._caravanFile);

            Packet packet = Packet.CreatePacketFromObject(nameof(PacketHandler.CaravanPacket), data);
            NetworkHelper.SendPacketToAllClients(packet);

            Logger.Message($"[Add Caravan] > {data._caravanFile.ID} > {client.userFile.Username}");
        }

        private static void RemoveCaravan(ServerClient client, CaravanData data)
        {
            CaravanFile toRemove = GetCaravanFromID(client, data._caravanFile.ID);
            if (toRemove == null) return;
            else
            {
                DeleteCaravan(data._caravanFile);

                Packet packet = Packet.CreatePacketFromObject(nameof(PacketHandler.CaravanPacket), data);
                NetworkHelper.SendPacketToAllClients(packet);

                Logger.Message($"[Remove Caravan] > {data._caravanFile.ID} > {client.userFile.Username}");
            }
        }

        private static void MoveCaravan(ServerClient client, CaravanData data)
        {
            CaravanFile toMove = GetCaravanFromID(client, data._caravanFile.ID);
            if (toMove == null) return;
            else
            {
                UpdateCaravan(toMove, data._caravanFile);
                RefreshCaravanTimer(data._caravanFile);

                Packet packet = Packet.CreatePacketFromObject(nameof(PacketHandler.CaravanPacket), data);
                NetworkHelper.SendPacketToAllClients(packet, client);
            }
        }

        private static void SaveCaravan(CaravanFile details)
        {
            Serializer.SerializeToFile(Path.Combine(Master.caravansPath, details.ID + fileExtension), details);
        }

        private static void DeleteCaravan(CaravanFile details)
        {
            File.Delete(Path.Combine(Master.caravansPath, details.ID + fileExtension));
        }

        private static void UpdateCaravan(CaravanFile details, CaravanFile newDetails)
        {
            details.Tile = newDetails.Tile;
        }

        private static void RefreshCaravanTimer(CaravanFile details)
        {
            details.TimeSinceRefresh = TimeConverter.CurrentTimeToEpoch();

            SaveCaravan(details);
        }

        public static void StartCaravanTicker()
        {
            while (true)
            {
                Thread.Sleep(1800000);

                try { IdleCaravanTick(); }
                catch (Exception e) { Logger.Error($"Caravan tick failed, this should never happen. Exception > {e}"); }
            }
        }

        private static void IdleCaravanTick()
        {
            foreach(CaravanFile caravans in GetActiveCaravans())
            {
                if (TimeConverter.CheckForEpochTimer(caravans.TimeSinceRefresh, baseMaxTimer))
                {
                    DeleteCaravan(caravans);

                    CaravanData data = new CaravanData();
                    data._stepMode = CaravanStepMode.Remove;
                    data._caravanFile = caravans;

                    Packet packet = Packet.CreatePacketFromObject(nameof(PacketHandler.CaravanPacket), data);
                    NetworkHelper.SendPacketToAllClients(packet);
                }
            }

            Logger.Message($"[Caravan tick]");
        }

        public static CaravanFile[] GetActiveCaravans()
        {
            List<CaravanFile> activeCaravans = new List<CaravanFile>();
            foreach (string str in Directory.GetFiles(Master.caravansPath))
            {
                activeCaravans.Add(Serializer.SerializeFromFile<CaravanFile>(str));
            }

            return activeCaravans.ToArray();
        }

        public static CaravanFile GetCaravanFromID(ServerClient client, int caravanID)
        {
            CaravanFile toGet = GetActiveCaravans().FirstOrDefault(fetch => fetch.ID == caravanID &&
                fetch.Owner == client.userFile.Username);

            if (toGet == null) return null;
            else return toGet;
        }

        private static int GetNewCaravanID()
        {
            int maxID = 0;
            foreach(CaravanFile caravans in GetActiveCaravans())
            {
                if (caravans.ID >= maxID)
                {
                    maxID = caravans.ID + 1;
                }
            }

            return maxID;
        }
    }
}
