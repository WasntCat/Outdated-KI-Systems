using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using KINetwork.Global.Utilitys;
using KINetwork.KIServer.CatEngine;

namespace KINetwork.KIServer.Core
{
    internal class Functions
    {

        // Register Function o-o
        internal static void HandleRegister(string[] dat, string ip, NetworkStream str)
        {
            var (usrname, usrpass, usrhwid, usrinv, rank, time) = ((Func<string[], (string, string, string, string, string, DateTime)>)(datbl => (datbl[1], datbl[2], datbl[3], datbl[4], "SemiPublic", DateTime.Now.AddMonths(1))))(dat);
            ILogger.ILog($"Register Request has been made\n     [ Data-Block | Usr : {usrname}, Pass : {usrpass}, Invite Code : {usrinv}, hwid : {usrhwid}, Ip : {ip} ]", 0, 0);
            if (NetChecks.InvCheck(usrinv))
            {
                ILogger.ILog($"Register Request was handled, New User Accoutn Created\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip} ]", 0, 0);
            //    ILogger.WebLogger($"User Account Has been Registered::({usrname})", "USER");
                Mapper.Save(usrname, usrpass, usrhwid, ip, rank, time);
                Mapper.DeleteInv(usrinv);
                Outgoing("__M_Y__L_O_V_E__", str);
            }
            else
            {
                ILogger.ILog($"Register Request had a Hard Fail-Point, Couldn't verify incoming data\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip} ]", 3, 0);
              //  ILogger.WebLogger($"User Registeration had a Full Fail, Couldnt verify Incoming user data::( Usr : {usrname}, hwid : {usrhwid}, Ip : {ip} )", "ERROR");
                Outgoing("__F_A_K_E__", str);
            }
        }

        // Login Function p-p
        internal static void HandleLogin(string[] dat, string ip, NetworkStream str)
        {
            var (usrname, usrpass, usrhwid) = ((Func<string[], (string, string, string)>)(datbl => (datbl[1], datbl[2], datbl[3])))(dat);
            ILogger.ILog($"Login Reqest has been made\n     [ Data-Block | Usr : {usrname}, Pass : {usrpass}, hwid : {usrhwid}, Ip : {ip} ]", 0, 0);
            if (NetChecks.UsrCheck(usrname, usrpass, usrhwid, out string rank, out DateTime time)) {
                if (DateTime.Now > time)
                {
                    ILogger.ILog($"Account Subscription Expired\n     [ Data-Block | Usr : {usrname}, Pass : {usrpass}, hwid : {usrhwid}, Ip : {ip} ]", 1, 0);
                  //  ILogger.WebLogger($"User Account Subscription Expired::({usrname})", "USER");

                    Outgoing("__W_A_S_T_E_D__|Account expired", str);
                } else {
                    string usrtoekn = Generation.GenerateSessionToken();
                    //   Mapper._SessionTokens[usrname] = usrtoekn;
                    Mapper._SessionTokens[usrname] = (usrtoekn, DateTime.Now.AddHours(1));
                    ILogger.ILog($"Login Session Has been Verifed\n     [ Data-Block | Usr : {usrname}, Session Token : {usrtoekn} ]", 0, 0);
                   // ILogger.WebLogger($"User Has Logged in::({usrname})", "USER");
                    Outgoing($"__D_A_R_L_I_N_G__|{usrtoekn}|{rank}|{time}", str);
                }
            }
            else
            {
                ILogger.ILog($"Login Session had a Hard Fail-Point, Couldn't verify incoming data\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip} ]", 3, 0);
               // ILogger.WebLogger($"User Login had a Full Fail, Couldnt verify Incoming user data::( Usr : {usrname}, hwid : {usrhwid}, Ip : {ip} )", "ERROR");
                Outgoing("__D_I_E__", str);
            }
        }

        // Left in here im lazy, Dont toech p-p
        internal static bool ValidateToken(string usr, string tok)  {
            if (Mapper._SessionTokens.TryGetValue(usr, out var storedToken)) {
                if (storedToken.Token == tok) {
                    if (DateTime.Now <= storedToken.Expiry) {
                        return true;
                    }
                } else {
               //     ILogger.ILog($"[DEUBG] Invaild token for {usr}, Stored {storedToken.Token}, Given {token}", 2, 0);
                }
            } else {
               // ILogger.ILog($"[DEUBG] Issue finding token for {usr}", 2, 0);
            }
            return false;
        }

        //Do it yourself c:
        internal static void HandleWebhook(string[] dat) { }


      

        // String Centerianal, Log Centerial :p
        // smal
        internal static void HandleRequest1(string[] dat, string ip, NetworkStream str)
        {
            var (usrname, usrhwid, proxy, usrtoken) = ((Func<string[], (string, string, string, string)>)(datbl => (datbl[1], datbl[2], datbl[3], datbl[4])))(dat);
            string rank;
            DateTime time;

            if (NetChecks.UsrCheck(usrname, null, usrhwid, out rank, out time))
            {
                if (ValidateToken(usrname, usrtoken))
                {
                 //   ILogger.WebLogger($"{usrname}'s Session token is valid::({usrtoken})", "SECURITY");
                    ILogger.ILog($"Session Token is valid\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip}, Session Token : {usrtoken} ]", 0, 0);
                    if (DateTime.Now <= time)
                    {
                     //   ILogger.WebLogger($"{usrname}'s Subscription is active", "LOG");
                        ILogger.ILog($"Subscription is active\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip}, Session Token : {usrtoken} ]", 0, 0);
                        string PrefGet = Mapper._Retive(proxy, rank);
                        if (!string.IsNullOrEmpty(PrefGet))
                        {
                          //  ILogger.WebLogger($"{usrname}'s Encryptor Session has started", "LOG");
                            ILogger.ILog($"Starting Assembly Encryptor for user's request\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip}, Session Token : {usrtoken} ]\n     [ Assembly-Block | Type : {proxy}, Rank : {rank} ]", 0, 0);
                            NetEncryption._AsbAESSmall(PrefGet, str, usrname, usrhwid, proxy);
                        }
                        else
                        {
                          //  ILogger.WebLogger($"Prevented {usrname} from requesting a assembly or application due to there rank not having high enough perms, Possiable brute/Crack attempt", "SECURITY");
                            ILogger.ILog($"User's Assembly request was blocked due to it not aligning with rank permissions\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip} ]", 1, 0);
                        }
                    }
                    else
                    {
                       // ILogger.WebLogger($"Prevented {usrname} from requesting a assembly or application due to there subscription being expired? Shouldnt be possiable unless brute/crack", "SECURITY");
                        ILogger.ILog($"User's Assembly request was blocked due to subscription expiration\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip} ]", 1, 0);
                    }
                }
                else
                {
                  //  ILogger.WebLogger($"{usrname}'s Session token is not Valid::({usrtoken})", "SECURITY");
                    ILogger.ILog($"User's Assembly request was blocked due to an invalid session token\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip}, Token {usrtoken} ]", 1, 0);
                }
            }
            else
            {
              //  ILogger.WebLogger($"{usrname}'s Session due to Verifcation Failure", "SECURITY");
                ILogger.ILog($"User's Assembly request was blocked due to account verification failure\n     [ Data-Block | Usr : {usrname}, hwid : {usrhwid}, Ip : {ip} ]", 1, 0);
            }
        }

        // To send the data p-p
        internal static void Outgoing(string dat, NetworkStream str)
        {
            byte[] byt = Encoding.ASCII.GetBytes(dat);
           // ILogger.WebLogger($"Outing going Server data to client, {dat}", "LOG");
            str.Write(byt, 0, byt.Length);
        }
    }
}
