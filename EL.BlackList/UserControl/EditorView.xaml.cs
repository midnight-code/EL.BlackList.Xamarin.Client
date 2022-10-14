using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EL.BlackList.UserControl
{
    public partial class EditorView : ContentView
    {
        public string Test { get; set; }
        public EditorView()
        {
            InitializeComponent();
            editMessage.Text = Test;
        }
    }
}