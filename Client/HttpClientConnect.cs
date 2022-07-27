using Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    class HttpClientConnect<T>
    {
        static readonly HttpClient httpclient = new HttpClient();
        public static int AddElement(ObservableCollection<T> cards, Card newCard)
        {
            int result = -1;
            try
            {
                string json = JsonConvert.SerializeObject(newCard);
                HttpContent content = new StringContent(json);                    
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                HttpResponseMessage responce = httpclient.PostAsync("http://localhost:8000/", content).Result;
                var context = responce.Content.ReadAsStringAsync().Result;
                if (context != (-1).ToString())
                    result = Convert.ToInt32(context);
                Console.WriteLine(context);
                
            }
            catch (HttpRequestException e)
            {
                result = -1;
                MessageBox.Show("\nServer error!\n" +
                "Message: " + e.Message);
            }
            catch (AggregateException e)
            {
                result = -1;
                MessageBox.Show("\nServer error!\n" +
                "Message: " + e.Message);
            }
            catch (Exception e)
            {
                result = -1;
                MessageBox.Show("\nUnexpected error!\n" +
                "Message: " + e.Message);
            }
            return result;
        }

        public static bool RemoveElement(Card card)
        {
            bool result = true;
            try
            {                              
                httpclient.DefaultRequestHeaders.Add("RemoveId", card.Id.ToString());
                //content.Headers.Add("RemoveId", card.Id.ToString());               
                HttpResponseMessage responce = httpclient.DeleteAsync("http://localhost:8000/").Result;
                var context = responce.Content.ReadAsStringAsync().Result;
                if (context == "true")
                    result = true;
                Console.WriteLine(context);
            }
            catch (HttpRequestException e)
            {
                result = false;
                MessageBox.Show("\nServer error!\n" +
                "Message: " + e.Message);
            }
            catch (AggregateException e)
            {
                result = false;
                MessageBox.Show("\nServer error!\n" +
                "Message: " + e.Message);
            }
            catch (Exception e)
            {
                result = false;
                MessageBox.Show("\nUnexpected error!\n" +
                "Message: " + e.Message);
            }
            return result;
        }

        public static bool UpdateElement(Card newCard)
        {
            bool result = true;
            try
            {
                string json = JsonConvert.SerializeObject(newCard);
                HttpContent content = new StringContent(json);           
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                HttpResponseMessage responce = httpclient.PutAsync("http://localhost:8000/", content).Result;
                var context = responce.Content.ReadAsStringAsync().Result;
                if (context == "true")
                    result = true;
                Console.WriteLine(context);               
            }
            catch (HttpRequestException e)
            {
                result = false;
                MessageBox.Show("\nException Caught!\n" +
                "Message: " + e.Message);
            }
            catch (AggregateException e)
            {
                result = false;
                MessageBox.Show("\nException Caught!\n" +
                "Message: " + e.Message);
            }
            catch (Exception e)
            {
                result = false;
                MessageBox.Show("\nUnexpected error!\n" +
                "Message: " + e.Message);
            }
            return result;
        }

        public static ObservableCollection<T> GetElements()
        {
            ObservableCollection<T> cards = new ObservableCollection<T>();
            try
            {                                  
                    HttpResponseMessage responce = httpclient.GetAsync("http://localhost:8000/").Result;
                    var context = responce.Content.ReadAsStringAsync().Result;
                    if(context != "" && context != "null")
                        cards = JsonConvert.DeserializeObject<ObservableCollection<T>>(context);
                    Console.WriteLine(context);                    
                
            }
            catch (HttpRequestException e)
            {               
                MessageBox.Show("\nServer error!\n" +
                "Message: " + e.Message);
            }
            catch (AggregateException e)
            {                
                MessageBox.Show("\nServer error!\n" +
                "Message: " + e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("\nUnexpected error!\n" +
                "Message: " + e.Message);
            }
            return cards;
        }
    }
}

