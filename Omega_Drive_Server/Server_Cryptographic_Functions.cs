using System;
using System.Diagnostics;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Cloudmersive.APIClient.NETCore.VirusScan.Api;
using Cloudmersive.APIClient.NETCore.VirusScan.Client;
using Cloudmersive.APIClient.NETCore.VirusScan.Model;

namespace Omega_Drive_Server
{
    class Server_Cryptographic_Functions:Server_Application_Variables
    {
        private static string client_certificate_name = "Client_Omega_Drive.crt";
        private static string server_certificate_name = "Server_Omega_Drive.crt";

        private static string salt = "dafjlsk";

        private static System.Random random_number_generator = new Random();

        private static char[] symbols = new char[] { '¬', '`', '¦', '!', '"', '£', '$', '%', '^', '&', '*', '(', ')', '€', '-', '_', '=', '+', '[', '{', ']', '}', ';', ':', '\'', '@', '#', '~', ',', '<', '.', '>', '/', '?', '|', '\\' };
        private static char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };







        internal async Task<bool> Create_X509_Server_Certificate(string password, int certificate_valid_time_period_in_days)
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





                    System.Security.Cryptography.X509Certificates.X509Certificate2 client_certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificate_memory_stream.ToArray(), password.ToCharArray());
                    byte[] client_certificate_binary_data = client_certificate.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Cert, new string(password.ToCharArray()));






                    System.IO.FileStream server_certificate_file_stream = System.IO.File.Create(server_certificate_name, (int)certificate_memory_stream.Length, System.IO.FileOptions.Asynchronous);
                    try
                    {
                        await server_certificate_file_stream.WriteAsync(certificate_memory_stream.ToArray());
                        await server_certificate_file_stream.FlushAsync();
                    }
                    catch
                    {
                        if (server_certificate_file_stream != null)
                        {
                            await server_certificate_file_stream.FlushAsync();
                            server_certificate_file_stream.Close();
                        }
                    }
                    finally
                    {
                        if (server_certificate_file_stream != null)
                        {
                            await server_certificate_file_stream.FlushAsync();
                            server_certificate_file_stream.Close();
                            await server_certificate_file_stream.DisposeAsync();
                        }
                    }


                    System.IO.FileStream client_certificate_file_stream = System.IO.File.Create(client_certificate_name, client_certificate_binary_data.Length, System.IO.FileOptions.Asynchronous);
                    try
                    {
                        await client_certificate_file_stream.WriteAsync(client_certificate_binary_data);
                        await client_certificate_file_stream.FlushAsync();
                    }
                    catch
                    {
                        if (client_certificate_file_stream != null)
                        {
                            await client_certificate_file_stream.FlushAsync();
                            client_certificate_file_stream.Close();
                        }
                    }
                    finally
                    {
                        if (client_certificate_file_stream != null)
                        {
                            await client_certificate_file_stream.FlushAsync();
                            client_certificate_file_stream.Close();
                            await client_certificate_file_stream.DisposeAsync();
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

        internal Task<bool> Load_Server_Certificate_In_Application_Memory()
        {
            bool server_certificate_load_successful = false;

            try
            {
                if (System.IO.File.Exists(server_certificate_name) == true)
                {
                    string file_path = String.Empty;

                    if (OperatingSystem.IsWindows() == true)
                    {
                        file_path = Environment.CurrentDirectory + "\\" + server_certificate_name;
                    }
                    else
                    {
                        file_path = Environment.CurrentDirectory + "/" + server_certificate_name;
                    }


                    server_certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(file_path, server_ssl_certificate_password);

                    server_certificate_load_successful = true;
                }
            }
            catch
            {

            }

            return Task.FromResult(server_certificate_load_successful);
        }




        internal async Task<string> Scan_File_With_Cloudmersive(byte[] file)
        {
            string virus_scan_result = String.Empty;

            try
            {
                Configuration.Default.ApiKey.Clear();
                Configuration.Default.ApiKey.Add("Apikey", Cloudmersive_Api_Key);

                var Cloudmersive_Api = new ScanApi();


                System.IO.Stream s = new System.IO.MemoryStream(file);

                VirusScanResult Virus_Scan_Result = await Cloudmersive_Api.ScanFileAsync(s);
               
                if(Virus_Scan_Result.CleanResult == true)
                {
                    virus_scan_result = "Clean result";
                }
                else
                {
                    virus_scan_result = "Virus found";
                }

                Debug.WriteLine(Virus_Scan_Result);
            }
            catch(Exception E)
            {
                virus_scan_result = E.Message;
            }

            return virus_scan_result;
        }




        internal Task<string> Random_Alphanumeric_Code_Generator()
        {

            string generated_random_code = String.Empty;


            for(int count = 0; count < 30; count++)
            {
                int generated_number = random_number_generator.Next(0, 3);


                switch (generated_number)
                {
                    case 0:
                        generated_random_code += symbols[random_number_generator.Next(0, symbols.Length)];
                        break;



                    case 1:

                        int generated_option = random_number_generator.Next(0, 2);

                        switch (generated_option)
                        {
                            case 0:
                                generated_random_code += letters[random_number_generator.Next(0, letters.Length)];
                                break;

                            case 1:
                                generated_random_code += letters[random_number_generator.Next(0, letters.Length)].ToString().ToLower();
                                break;
                        }
                        break;



                    case 2:
                        generated_random_code += numbers[random_number_generator.Next(0, numbers.Length)];
                        break;
                }
            }

            return Task.FromResult(generated_random_code);
        }




        internal async Task<string> Content_Hasher<Content>(Content content)
        {
            string hashed_content = String.Empty;

            byte[] extracted_content = new byte[] { };







            if (content.GetType() == extracted_content.GetType())
            {
                extracted_content = content as byte[];
            }
            else if (content.GetType() == typeof(string))
            {
                extracted_content = Encoding.UTF8.GetBytes(content as string);
            }
            else
            {
                hashed_content = "Error occured";
            }






            if(hashed_content != "Error occured")
            {
                extracted_content = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(extracted_content) + salt);


                System.Security.Cryptography.SHA256 content_hasher = System.Security.Cryptography.SHA256.Create();

                try
                {

                    System.IO.Stream stream = new System.IO.MemoryStream(extracted_content);

                    try
                    {
                        hashed_content = Encoding.UTF8.GetString(await content_hasher.ComputeHashAsync(stream));
                    }
                    catch
                    {
                        hashed_content = "Error occured";

                        if (stream != null)
                        {
                            stream.Close();
                        }
                    }
                    finally
                    {
                        if (stream != null)
                        {
                            stream.Close();
                            await stream.DisposeAsync();
                        }
                    }
                }
                catch
                {
                    hashed_content = "Error occured";

                    if (content_hasher != null)
                    {
                        content_hasher.Clear();
                    }
                }
                finally
                {
                    if (content_hasher != null)
                    {
                        content_hasher.Clear();
                        content_hasher.Dispose();
                    }
                }
            }


            return hashed_content;
        }
    }
}
