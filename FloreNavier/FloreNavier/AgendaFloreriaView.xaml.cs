using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloreNavier
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgendaFloreriaView : ContentPage
    {
        public AgendaFloreriaView()
        {
            InitializeComponent();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            ((AgendaFloreria)BindingContext).FiltrarCommand.Execute(null);
        }
    }
}