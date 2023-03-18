using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cloudmersive.APIClient.NETCore.VirusScan.Api;
using Cloudmersive.APIClient.NETCore.VirusScan.Client;
using Cloudmersive.APIClient.NETCore.VirusScan.Model;

namespace Omega_Drive_Server
{
    class Server_Cryptographic_Functions:Server_Application_Variables
    {
        private static string server_certificate_name = "Omega_Drive.crt";


        protected static async Task<bool> Create_X509_Server_Certificate(string password, int certificate_valid_time_period_in_days)
        {

            bool server_certificate_creation_successful = false;


            try
            {
                Org.BouncyCastle.Crypto.Prng.CryptoApiRandomGenerator randomGenerator = new Org.BouncyCastle.Crypto.Prng.CryptoApiRandomGenerator();
                Org.BouncyCastle.Security.SecureRandom random = new Org.BouncyCastle.Security.SecureRandom(randomGenerator);


                Org.BouncyCastle.X509.X509V3CertificateGenerator certificateGenerator = new Org.BouncyCastle.X509.X509V3CertificateGenerator();


                Org.BouncyCastle.Math.BigInteger serialNumber = Org.BouncyCastle.Utilities.BigIntegers.CreateRandomInRange(Org.BouncyCastle.Math.BigInteger.One, Org.BouncyCastle.Math.BigInteger.ValueOf(int.MaxValue), random);
                certificateGenerator.SetSerialNumber(serialNumber);



                Org.BouncyCastle.Asn1.X509.X509Name subjectDN = new Org.BouncyCastle.Asn1.X509.X509Name("CN=Omega_Drive_Certificate");
                Org.BouncyCastle.Asn1.X509.X509Name issuerDN = subjectDN;
                certificateGenerator.SetIssuerDN(issuerDN);
                certificateGenerator.SetSubjectDN(subjectDN);



                DateTime notBefore = DateTime.UtcNow.Date;
                DateTime notAfter = notBefore.AddDays(certificate_valid_time_period_in_days);

                certificateGenerator.SetNotBefore(notBefore);
                certificateGenerator.SetNotAfter(notAfter);



                const int strength = 2048;
                Org.BouncyCastle.Crypto.KeyGenerationParameters keyGenerationParameters = new Org.BouncyCastle.Crypto.KeyGenerationParameters(random, strength);

                Org.BouncyCastle.Crypto.Generators.RsaKeyPairGenerator keyPairGenerator = new Org.BouncyCastle.Crypto.Generators.RsaKeyPairGenerator();
                keyPairGenerator.Init(keyGenerationParameters);
                Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair subjectKeyPair = keyPairGenerator.GenerateKeyPair();

                certificateGenerator.SetPublicKey(subjectKeyPair.Public);


                Org.BouncyCastle.Crypto.ISignatureFactory signatureFactory = new Org.BouncyCastle.Crypto.Operators.Asn1SignatureFactory("SHA256WithRSA", subjectKeyPair.Private, random);



                Org.BouncyCastle.X509.X509Certificate certificate = certificateGenerator.Generate(signatureFactory);



                Org.BouncyCastle.Pkcs.Pkcs12Store store = new Org.BouncyCastle.Pkcs.Pkcs12Store();


                string friendlyName = certificate.SubjectDN.ToString();



                Org.BouncyCastle.Pkcs.X509CertificateEntry certificateEntry = new Org.BouncyCastle.Pkcs.X509CertificateEntry(certificate);
                store.SetCertificateEntry(friendlyName, certificateEntry);




                store.SetKeyEntry(friendlyName, new Org.BouncyCastle.Pkcs.AsymmetricKeyEntry(subjectKeyPair.Private), new[] { certificateEntry });





                System.IO.MemoryStream certificate_memory_stream = new System.IO.MemoryStream();

                try
                {
                    store.Save(certificate_memory_stream, password.ToCharArray(), random);
                    await certificate_memory_stream.FlushAsync();


                    System.IO.FileStream certificate_file_stream = System.IO.File.Create(server_certificate_name, (int)certificate_memory_stream.Length, System.IO.FileOptions.Asynchronous);
                    try
                    {
                        await certificate_file_stream.WriteAsync(certificate_memory_stream.ToArray());
                        await certificate_file_stream.FlushAsync();
                    }
                    catch
                    {
                        if (certificate_file_stream != null)
                        {
                            await certificate_file_stream.FlushAsync();
                            certificate_file_stream.Close();
                        }
                    }
                    finally
                    {
                        if (certificate_file_stream != null)
                        {
                            await certificate_file_stream.FlushAsync();
                            certificate_file_stream.Close();
                            await certificate_file_stream.DisposeAsync();
                        }
                    }
                }
                catch
                {
                    if (certificate_memory_stream != null)
                    {
                        await certificate_memory_stream.FlushAsync();
                        certificate_memory_stream.Close();
                    }
                }
                finally
                {
                    if (certificate_memory_stream != null)
                    {
                        await certificate_memory_stream.FlushAsync();
                        certificate_memory_stream.Close();
                        await certificate_memory_stream.DisposeAsync();
                    }
                }

                server_certificate_creation_successful = true;
            }
            catch
            {
                
            }


            return server_certificate_creation_successful;
        }


        protected static Task<bool> Delete_X509_Server_Certificate()
        {
            bool server_certificate_deletion_result_successful = false;


            try
            {
                if(System.IO.File.Exists(server_certificate_name) == true)
                {
                    System.IO.File.Delete(server_certificate_name);
                }

                server_certificate_deletion_result_successful = true;
            }
            catch{ }

            return Task.FromResult(server_certificate_deletion_result_successful);
        }


        protected static Task<bool> Load_Server_Certificate_In_Application_Memory(string password)
        {
            bool server_certificate_load_successful = false;

            if(System.IO.File.Exists(server_certificate_name) == true)
            {
                string file_path = String.Empty;

                if (System.OperatingSystem.IsWindows() == true)
                {
                    file_path = Environment.CurrentDirectory + "\\" + server_certificate_name;
                }
                else
                {
                    file_path = Environment.CurrentDirectory + "/" + server_certificate_name;
                }


                server_certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(file_path, password);
            }

            return Task.FromResult(server_certificate_load_successful);
        }


        protected static async Task<string> Scan_File_With_Cloudmersive(byte[] file)
        {
            string virus_scan_result = String.Empty;

            try
            {
                Configuration.Default.ApiKey.Add("Apikey", Cloudmersive_Api_Key);

                var Cloudmersive_Api = new ScanApi();
                
                Stream s = new MemoryStream(file);

                VirusScanResult Virus_Scan_Result = await Cloudmersive_Api.ScanFileAsync(s);
                Debug.WriteLine(Virus_Scan_Result);
            }
            catch(Exception E)
            {
                virus_scan_result = E.Message;
            }
            
            return virus_scan_result;
        }
    }
}
