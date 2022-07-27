using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static int pageViews = 0;
        public static int requestCount = 0;
      
        public static async Task HandleIncomingConnections()
        {
            try
            {
                bool runServer = true;
                while (runServer)
                {
                    // Will wait here until we hear from a connection
                    HttpListenerContext ctx = await listener.GetContextAsync();

                    // Peel out the requests and response objects
                    HttpListenerRequest req = ctx.Request;
                    HttpListenerResponse resp = ctx.Response;

                    var inputStream = req.InputStream;
                    List<byte> inputByteMess = new List<byte>();
                    int result = inputStream.ReadByte();
                    while (result != -1)
                    {
                        inputByteMess.Add(Convert.ToByte(result));
                        result = inputStream.ReadByte();
                    }

                    byte[] bytesMess = inputByteMess.ToArray();

                    Card card;
                    Manager manager = new Manager();
                    string message = "";
                    switch (req.HttpMethod)
                    {
                        case "GET":
                            message = JsonConvert.SerializeObject(manager.cards);
                            break;
                        case "POST":
                            string newCard = Encoding.UTF8.GetString(bytesMess);
                            card = JsonConvert.DeserializeObject<Card>(newCard);
                            if (manager.AddElement(card))
                                message = manager.cards[manager.cards.Count - 1].Id.ToString();
                            break;
                        case "DELETE":
                            string remId = req.Headers.Get("RemoveId");

                            int removeId = Convert.ToInt32(remId);
                            //card = (Card)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytesMess));
                            message = manager.RemoveElement(removeId).ToString();
                            break;
                        case "PUT":
                            card = JsonConvert.DeserializeObject<Card>(Encoding.UTF8.GetString(bytesMess));
                            message = manager.UpdateElement(card).ToString();
                            break;

                    }


                    Console.WriteLine("Request #: {0}", ++requestCount);
                    Console.WriteLine(req.Url.ToString());
                    Console.WriteLine(req.HttpMethod);
                    Console.WriteLine(req.UserHostName);
                    Console.WriteLine(req.UserAgent);
                    Console.WriteLine();

                    // Write the response info
                    string disableSubmit = !runServer ? "disabled" : "";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    resp.ContentType = "application/json";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = data.LongLength;

                    // Write out to the response stream (asynchronously), then close it
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                    resp.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!\n" +
                "Message: " + e.Message);
            }
        }


        public static void Main(string[] args)
        {
            try
            {
                // Create a Http server and start listening for incoming connections
                listener = new HttpListener();
                listener.Prefixes.Add(url);
                listener.Start();
                Console.WriteLine("Listening for connections on {0}", url);

                // Handle requests
                Task listenTask = HandleIncomingConnections();
                listenTask.GetAwaiter().GetResult();

                // Close the listener
                listener.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("\nException Caught!\n" +
                "Message: " + e.Message);
            }
           
        }
    }
}
