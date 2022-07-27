using Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.Models
{
    class Card 
    {

        public uint Id { get; set; }
        public string Title { get; set; }
        public byte[] ImageSourse { get; set; }

    }
}
