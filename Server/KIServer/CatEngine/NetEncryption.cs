using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;

using Mono.Cecil;
using Mono.Cecil.Cil;

using KINetwork.Global.Utilitys;

namespace KINetwork.KIServer.CatEngine
{
    internal class NetEncryption
    {
        // Utilz Mono Cecil here since didnt feel like editing dnlib or asmresolv + was Lazy
        internal static void _AsmbSig(string asmb, string outp, string usr, string hw) { }

	// Annoying to do
        internal static void _AsmbSigNative(string asmb, string outp, string usr, string hw, string Hash, string Build_Time) { }

	// Exacuatble only (:
        internal static void _AttatchNativeWrapper(string asmb, string outp, string Packmethod) { } 
        // I love my jit hoookaaaaaa
        internal static void _Jit(string target) { }
        internal static void _MethodMangler(string target) { }
        internal static void _OpcodeScrambler(string target) { }


        internal static void _Encryptor(string asmb, NetworkStream str, string usr, string hwid, string prox) { }


        // You can code the things above ^^^ yourself or pay me c: $$$$$$$


	// Only use on the .Net Internals/Dlls, change to just use AES since im not giving my orginal Encryption stuff unless $$$ c:
        internal static void _AsbAESSmall(string asmb, NetworkStream str, string usr, string hwid, string prox)
        {
            byte[] dat = File.ReadAllBytes(asmb);
            byte[] _Encrypt;
            byte[] _key;
            byte[] _IV;

            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                _key = aes.Key;
                _IV = aes.IV;

                using (ICryptoTransform crypto = aes.CreateEncryptor(_key, _IV))
                using (MemoryStream memstr = new MemoryStream())
                using (CryptoStream crpstr = new CryptoStream(memstr, crypto, CryptoStreamMode.Write))
                {
                    crpstr.Write(dat, 0, dat.Length);
                    crpstr.FlushFinalBlock();
                    _Encrypt = memstr.ToArray();
                }
            }

            byte[] _both = new byte[_key.Length + _IV.Length];
            Buffer.BlockCopy(_key, 0, _both, 0, _key.Length);
            Buffer.BlockCopy(_IV, 0, _both, _key.Length, _IV.Length);
            str.Write(_both, 0, _both.Length);
            str.Flush(); 

            byte[] _Size = BitConverter.GetBytes(_Encrypt.Length);
            Array.Reverse(_Size);
            str.Write(_Size, 0, _Size.Length);
            str.Flush();

            str.Write(_Encrypt, 0, _Encrypt.Length);
            str.Flush(); 
        }
    }
}
