using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;

namespace Client.ViewModels
{
    class MainViewModel : PropertysChanged
    {
        public MainViewModel()
        {
            Cards = HttpClientConnect<Card>.GetElements();          
        }

        private ObservableCollection<Card> cards;

        public ObservableCollection<Card> Cards { get => cards; set => Set(ref cards, value); }

        public int SelectIndex { get; set; }

        private Card card = new Card();

        public Card Card { 
            get => card;
            set
            {
                if(HttpClientConnect<Card>.UpdateElement(value))
                    Set(ref card, value);                 
            }
        }

        Card newCard = new Card();
        public Card NewCard { get => newCard; 
            set => Set(ref newCard, value); }

        private DelegateCommand changeImage;

        public ICommand ChangeImage
        {
            get
            {
                if (changeImage == null)
                {
                    changeImage = new DelegateCommand(PerformChangeImage);
                }
                return changeImage;
            }
        }

        public ICommand RemoveElement
        {
            get => new DelegateCommand((obj) =>
            {
                if (HttpClientConnect<Card>.RemoveElement((Card)obj)) 
                    Cards.Remove((Card)obj);
            });
        } 
        
        public ICommand GetImage
        {
            get => new DelegateCommand((obj) =>
            {
                string path = OpenFileDialog();
                if(path != null && path != "")
                    this.NewCard.ImageSourse = getImage(path);                
                OnPropertyChanged("NewCard");
            });
        }
        
        public ICommand SortElements
        {
            get => new DelegateCommand((obj) =>
            {
                IOrderedEnumerable<Card> cards = Cards.OrderBy(card => card.Title);
                Cards = new ObservableCollection<Card>(cards);
                OnPropertyChanged("Cards");
            });
        }

        public ICommand AddElement
        {
            get => new DelegateCommand((obj) =>
            {                
                int id = 0;
                if (NewCard != null && NewCard.Title != null && NewCard.Title != "" && NewCard.ImageSourse != null)
                {
                    id = HttpClientConnect<Card>.AddElement(cards, NewCard);
                    NewCard.Id = (uint)id;
                    if (id != -1)
                        Cards.Add(NewCard);
                    NewCard = new Card();
                }                  
                else
                    MessageBox.Show("Please, input a title and select an image.");                
            });
        }

        private void PerformChangeImage(object commandParameter)
        {
            Card tempCard = (Card)commandParameter;
            Card NewCard = new Card { Id = tempCard.Id, Title = tempCard.Title };
            string path = OpenFileDialog();
            NewCard.ImageSourse = getImage(path);
            if (HttpClientConnect<Card>.UpdateElement(NewCard))
                cards[cards.IndexOf(tempCard)] = NewCard;                
        }        

        public string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter =
                "JPEG File(*.jpg)|*.jpg|" +                     
                "PNG File(*.png)|*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        private byte[] getImage(string url = "D:/Рабочий стол/Temp/s.jpg")
        {           
           
            try
            {
                Image image = Image.FromFile(url);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                byte[] PathImage = ms.ToArray();

                return PathImage;
            }
            catch (Exception ex)
            {               
                return null;
            }
           
        }   

    }
    
}
