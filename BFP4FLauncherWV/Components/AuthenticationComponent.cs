﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BFP4FLauncherWV
{
    public static class AuthenticationComponent
    {

        public static void HandlePacket(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x28:
                    Login(p, pi, ns);
                    break;
                case 0x6E:
                    LoginPersona(p, pi, ns);
                    break;
            }
        }

        public static void Login(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)        
        {
            uint t = Blaze.GetUnixTimeStamp();
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfString.Create("LDHT", ""));
            Result.Add(Blaze.TdfInteger.Create("NTOS", 0));
            Result.Add(Blaze.TdfString.Create("PCTK", ""));
            List<Blaze.TdfStruct> playerentries = new List<Blaze.TdfStruct>();
            List<Blaze.Tdf> PlayerEntry = new List<Blaze.Tdf>();
            PlayerEntry.Add(Blaze.TdfString.Create("DSNM", pi.name));
            PlayerEntry.Add(Blaze.TdfInteger.Create("LAST", t));
            PlayerEntry.Add(Blaze.TdfInteger.Create("PID\0", pi.id));
            PlayerEntry.Add(Blaze.TdfInteger.Create("STAS", 2));
            PlayerEntry.Add(Blaze.TdfInteger.Create("XREF", 0));
            PlayerEntry.Add(Blaze.TdfInteger.Create("XTYP", 0));
            playerentries.Add(Blaze.TdfStruct.Create("0", PlayerEntry));
            Result.Add(Blaze.TdfList.Create("PLST", 3, 1, playerentries));
            Result.Add(Blaze.TdfString.Create("PRIV", ""));
            Result.Add(Blaze.TdfString.Create("SKEY", "some_client_key"));
            Result.Add(Blaze.TdfInteger.Create("SPAM", 1));
            Result.Add(Blaze.TdfString.Create("THST", ""));
            Result.Add(Blaze.TdfString.Create("TSUI", ""));
            Result.Add(Blaze.TdfString.Create("TURI", ""));
            Result.Add(Blaze.TdfInteger.Create("UID\0", pi.userId));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }
        public static void LoginPersona(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            uint t = Blaze.GetUnixTimeStamp();
            List<Blaze.Tdf> SESS = new List<Blaze.Tdf>();
            SESS.Add(Blaze.TdfInteger.Create("BUID", 1));
            SESS.Add(Blaze.TdfInteger.Create("FRST", 0));
            SESS.Add(Blaze.TdfString.Create("KEY\0", "some_client_key"));
            SESS.Add(Blaze.TdfInteger.Create("LLOG", t));
            SESS.Add(Blaze.TdfString.Create("MAIL", ""));
            List<Blaze.Tdf> PDTL = new List<Blaze.Tdf>();
            PDTL.Add(Blaze.TdfString.Create("DSNM", pi.name));
            PDTL.Add(Blaze.TdfInteger.Create("LAST", t));
            PDTL.Add(Blaze.TdfInteger.Create("PID\0", pi.id));
            PDTL.Add(Blaze.TdfInteger.Create("STAS", 0));
            PDTL.Add(Blaze.TdfInteger.Create("XREF", 0));
            PDTL.Add(Blaze.TdfInteger.Create("XTYP", 0));
            SESS.Add(Blaze.TdfStruct.Create("PDTL", PDTL));
            SESS.Add(Blaze.TdfInteger.Create("UID\0", pi.userId));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, SESS);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            List<Blaze.Tdf> USER = new List<Blaze.Tdf>();
            USER.Add(Blaze.TdfInteger.Create("AID\0", pi.id));
            USER.Add(Blaze.TdfInteger.Create("ALOC", 0x656E5553));
            USER.Add(Blaze.TdfInteger.Create("ID\0\0", pi.id));
            USER.Add(Blaze.TdfString.Create("NAME", pi.name));
            Result.Add(Blaze.TdfStruct.Create("USER", USER));
            buff = Blaze.CreatePacket(0x7802, 2, 0, 0x2000, 0, Result);
            ns.Write(buff, 0, buff.Length);                                           //TODO!!
            ns.Flush();                                                               //TODO!!
            Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("FLGS", 3));
            Result.Add(Blaze.TdfInteger.Create("ID\0\0", pi.userId));
            buff = Blaze.CreatePacket(0x7802, 5, 0, 0x2000, 0, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

    }
}
