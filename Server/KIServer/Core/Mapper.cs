using System;
using System.IO;
using System.Collections.Generic;

using KINetwork.Global.Utilitys;

namespace KINetwork.KIServer.Core
{
    internal class Mapper // Very Messy
    {
        internal static string _App = "ApplicationConfig";
        internal static string _AppBL = Path.Combine(_App, "Security");

        internal static string _UserDB = Path.Combine(_App, "userdb.txt");
        internal static string _InviteDB = Path.Combine(_App, "invites.txt");

        internal static string _IPbannedDB = Path.Combine(_AppBL, "IP_Blacklist.txt");
        internal static string _UsrnamebannedDB = Path.Combine(_AppBL, "USERNAME_Blacklist.txt");
        internal static string _HwidbannedDB = Path.Combine(_AppBL, "HWID_Blacklist.txt");

        internal static string _FILEStorageFree = Path.Combine("Storage", "Free");
        internal static string _FILEStoragePaied = Path.Combine("Storage", "Paid");
        internal static string _FILEStorageBeta = Path.Combine("Storage", "Beta");
        internal static string _FILEStorageAdnim = Path.Combine("Storage", "Admin");

        internal static string _FILEStorageTesting = Path.Combine("Storage", "TESTING BUILDS");

        internal static HashSet<string> _IPBlacklist = new HashSet<string>();
        internal static HashSet<string> _USERNAMEBlacklist = new HashSet<string>();
        internal static HashSet<string> _HWIDBlacklist = new HashSet<string>();

        internal static Dictionary<string, (string Token, DateTime Expiry)> _SessionTokens = new Dictionary<string, (string, DateTime)>();
        internal static Dictionary<string, List<DateTime>> _TRequest = new Dictionary<string, List<DateTime>>();


        internal const int _TRequestLimit = 180;
        internal const int _TRequestTimeLimit = 5;

        internal static void _Blacklist()
        {
            if (!File.Exists(_IPbannedDB))
            {
                Directory.CreateDirectory(_App);
                Directory.CreateDirectory(_AppBL);


                Directory.CreateDirectory(_FILEStorageFree);
                Directory.CreateDirectory(_FILEStoragePaied);
                Directory.CreateDirectory(_FILEStorageBeta);
                Directory.CreateDirectory(_FILEStorageAdnim);


                File.Create(_IPbannedDB);
                File.Create(_UsrnamebannedDB);
                File.Create(_HwidbannedDB);

                File.Create(_UserDB); // Im lazy
                File.Create(_InviteDB);
            }

            if (File.Exists(_IPbannedDB))
                _IPBlacklist = new HashSet<string>(File.ReadAllLines(_IPbannedDB));
            if (File.Exists(_UsrnamebannedDB))
                _USERNAMEBlacklist = new HashSet<string>(File.ReadAllLines(_UsrnamebannedDB));
            if (File.Exists(_HwidbannedDB))
                _HWIDBlacklist = new HashSet<string>(File.ReadAllLines(_HwidbannedDB));
        }

        internal static string _Retive(string proxy, string rank) {
            switch (proxy) {
                // Examp
                case "__D_I_E__F_O_R__M_E__":
                    if (rank == "SemiPublic")
                    {
                        return Path.Combine(_FILEStoragePaied, "meow.dll");
                    }
                    else if (rank == "KatnipBeta")
                    {
                        return Path.Combine(_FILEStorageBeta, "meow.dll");
                    }
                    break;
            }
            return string.Empty;
        }

        internal static void DeleteInv(string dat) {
            var inv = new List<string>(File.ReadAllLines(_InviteDB));
            inv.Remove(dat);
            File.WriteAllLines(_InviteDB, inv);
        }
        internal static void Save(string usr, string pass, string hwid, string ip, string rank, DateTime time) => File.AppendAllLines(_UserDB, new[] { $"{usr}|{pass}|{hwid}|{ip}|{rank}|{time}" });
    }
}
