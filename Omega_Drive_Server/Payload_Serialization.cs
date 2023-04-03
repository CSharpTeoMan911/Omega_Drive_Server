using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    internal class Payload_Serialization
    {
        internal async Task<byte[]> Serialize_Payload(byte[] payload)
        {
            byte[] serialized_payload = new byte[1024];

            if(payload == null)
            {
                payload = new byte[1024];
            }


            System.IO.StringWriter payload_stream = new StringWriter_Encoding();

            try
            {
                Server_WSDL_Payload server_WSDL_Payload = new Server_WSDL_Payload();
                server_WSDL_Payload.Server_Payload = payload;



                System.Xml.Serialization.XmlSerializer payload_serialiser = new System.Xml.Serialization.XmlSerializer(server_WSDL_Payload.GetType());
                payload_serialiser.Serialize(payload_stream, server_WSDL_Payload);



                serialized_payload = Encoding.UTF8.GetBytes(payload_stream.ToString());
                await payload_stream.FlushAsync();
            }
            catch (Exception E)
            {

                if (payload_stream != null)
                {
                    payload_stream.Close();
                }
            }
            finally
            {
                if (payload_stream != null)
                {
                    payload_stream.Close();
                    payload_stream.Dispose();
                }
            }


            return serialized_payload;
        }





        internal Task<Client_WSDL_Payload> Deserialize_Payload(byte[] payload)
        {
            Client_WSDL_Payload client_WSDL_Payload = new Client_WSDL_Payload();


            System.IO.TextReader payload_stream = new System.IO.StringReader(Encoding.UTF8.GetString(payload));

            try
            {
                System.Xml.Serialization.XmlSerializer payload_deserialiser = new System.Xml.Serialization.XmlSerializer(client_WSDL_Payload.GetType());
                client_WSDL_Payload = (Client_WSDL_Payload)payload_deserialiser?.Deserialize(payload_stream);

                client_WSDL_Payload.Function = Encoding.UTF8.GetString(Convert.FromBase64String(client_WSDL_Payload.Function));
                client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key = Encoding.UTF8.GetString(Convert.FromBase64String(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key));
            }
            catch (Exception E)
            {
                System.Diagnostics.Debug.WriteLine(E.Message);
                if (payload_stream != null)
                {
                    payload_stream.Close();
                }
            }
            finally
            {
                if (payload_stream != null)
                {
                    payload_stream.Close();
                    payload_stream.Dispose();
                }
            }

            return Task.FromResult(client_WSDL_Payload);
        }
    }
}
