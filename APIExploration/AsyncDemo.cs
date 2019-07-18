using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Build.Evaluation;


namespace APIExploration
{
    public class AsyncDemo
    {
        private static bool verbose;

        public async Task<string> GetValue()
        {
            var val = GetSomeThing();
            if ( val != null)
            {
                return val;
            }
            await Task.Delay(100);
            return "Hello";
            
        }
        public string GetSomeThing()
        {
            return "something";
        }

        public static void ModifyXml()
        {
            var filePath = @"C:\Users\bs_ma\source\repos\DotNetSamples\APIExploration\APIExploration.csproj";
            //var xDoc = XDocument.Load(filePath);
            //var signAssemblyElement = xDoc.Root.XPathSelectElement("//SignAssembly");
            //if (xDoc.Root.XPathSelectElement("//SignAssembly") == null &&
            //    xDoc.Root.XPathSelectElement("//PropertyGroup") != null ) 
            //{
            //    var props = xDoc.Root.XPathSelectElement("//PropertyGroup");
            //    props.Add(new XElement("SignAssembly", "true"), new XElement("AssemblyOriginatorKeyFile", "fo-dicom.snk"));
            //    props.Save(filePath);
            //}
            var projectCollection = new ProjectCollection();
            var proj = projectCollection.LoadProject(filePath);
            proj.SetProperty("SignAssembly", "true");
            proj.SetProperty("AssemblyOriginatorKeyFile", "fo-dicom.snk");
            proj.Save();


        }

        //------- Parses binary ans.1 RSA private key; returns RSACryptoServiceProvider  ---
        public static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();        //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();       //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                Console.WriteLine("showing components ..");
                if (verbose)
                {
                    showBytes("\nModulus", MODULUS);
                    showBytes("\nExponent", E);
                    showBytes("\nD", D);
                    showBytes("\nP", P);
                    showBytes("\nQ", Q);
                    showBytes("\nDP", DP);
                    showBytes("\nDQ", DQ);
                    showBytes("\nIQ", IQ);
                }

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static void showBytes(string v, byte[] mODULUS)
        {
            throw new NotImplementedException();
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            throw new NotImplementedException();
        }

        public static void CertDemo()
        {
            

            

            //var cert = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "idsvrtest.pfx");
             var privateKey = File.ReadAllText(@"C:\dev\jwt-key1");
            var buffer = Convert.FromBase64String(privateKey);
            
            var signingCertificate = new X509Certificate2(buffer, string.Empty, X509KeyStorageFlags.EphemeralKeySet);


            //var cert1 = new X509Certificate(bytes,string.Empty,X509KeyStorageFlags.Exportable);
            //var cert = X509Certificate.CreateFromCertFile(@"C:\dev\rsa_private.pem");
            
        }
    }
}
