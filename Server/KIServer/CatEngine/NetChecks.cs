using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using KINetwork.Global.Utilitys;
using KINetwork.KIServer.Core;

namespace KINetwork.KIServer.CatEngine
{
    internal class NetChecks
    {
        internal static bool BlacklistCheck(string ip, string usr, string hwid) => Mapper._IPBlacklist.Contains(ip) || Mapper._USERNAMEBlacklist.Contains(usr) || Mapper._HWIDBlacklist.Contains(hwid);
        internal static void Blacklist(string ip, string usr, string hwid) {
            if (!string.IsNullOrEmpty(ip) && !Mapper._IPBlacklist.Contains(ip))
            {
                ILogger.ILog($"[ BLACKLIST ] The Following IP [ {ip} ], Has been added to the blacklist", 1, 0);
                Mapper._IPBlacklist.Add(ip);
                File.AppendAllText("blacklisted_ips.txt", ip + Environment.NewLine);
            } if (!string.IsNullOrEmpty(usr) && !Mapper._USERNAMEBlacklist.Contains(usr)) {
                ILogger.ILog($"[ BLACKLIST ] The Following Username [ {usr} ], Has been added to the blacklist", 1, 0);
                Mapper._USERNAMEBlacklist.Add(usr);
                File.AppendAllText("blacklisted_usernames.txt", usr + Environment.NewLine);
            } if (!string.IsNullOrEmpty(hwid) && !Mapper._HWIDBlacklist.Contains(hwid))  {
                ILogger.ILog($"[ BLACKLIST ] The Following Hwid [ {hwid} ], Has been added to the blacklist", 1, 0);
                Mapper._HWIDBlacklist.Add(hwid);
                File.AppendAllText("blacklisted_hwids.txt", hwid + Environment.NewLine);
            }
        }

        internal static bool InvCheck(string dat)  {
            foreach (string inv in File.ReadAllLines(Mapper._InviteDB))
                if (inv == dat)
                    return true;
            return false;
        }
        internal static bool UsrCheck(string usr, string pass, string hwid, out string rank, out DateTime time) {
            rank = null;
            time = DateTime.MinValue;
            foreach (string user in File.ReadAllLines(Mapper._UserDB)) {
                string[] dat = user.Split('|');
                if (dat[0] == usr && (pass == null || dat[1] == pass) && dat[2] == hwid) {
                    rank = dat[4];
                    time = DateTime.Parse(dat[5]);
                    return true;
                }
            }
            return false;
        }

        internal static bool RatelimitCheck(string ip)  {
            DateTime time = DateTime.Now;
            if (!Mapper._TRequest.ContainsKey(ip)){
                Mapper._TRequest[ip] = new List<DateTime> { time };
                return false;
            }
            Mapper._TRequest[ip].Add(time);
            Mapper._TRequest[ip] = Mapper._TRequest[ip].Where(_time => (time - _time).TotalSeconds <= Mapper._TRequestTimeLimit).ToList();
            return Mapper._TRequest[ip].Count > Mapper._TRequestLimit;
        }
    }
}
