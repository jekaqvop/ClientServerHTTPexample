using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Server
{
    class Manager
    {
        public List<Card> cards { get; set; }
        public Manager()
        {
            string cardsStr = FileManager.GetDataFileAsync("cards.txt");
            cards = JsonConvert.DeserializeObject<List<Card>>(cardsStr);            
            cards = cards == null ? new List<Card>() : cards;
        }

        public bool RemoveElement(int id)
        {
            bool result = true;
            try
            {
                cards.Remove(cards.Find(x => x.Id == id));
                result = FileManager.WriteDataFile(JsonConvert.SerializeObject(cards));
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool AddElement(Card card)
        {
            bool result = true;
            try
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                card.Id = (uint)rnd.Next();
                cards.Add(card);
                result = FileManager.WriteDataFile(JsonConvert.SerializeObject(cards));
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool UpdateElement(Card card)
        {
            bool result = true;
            try
            {
                cards[cards.FindIndex(x => x.Id == card.Id)] = card;
                result = FileManager.WriteDataFile(JsonConvert.SerializeObject(cards));
            }
            catch
            {
                result = false;
            }
            return result;
        }

        
    }
}
