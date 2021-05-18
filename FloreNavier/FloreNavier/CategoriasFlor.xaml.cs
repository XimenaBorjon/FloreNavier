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
    public partial class CategoriasFlor : ContentPage
    {
        public CategoriasFlor()
        {
            InitializeComponent();
            Conte1();
            Conte2();
            Conte3();
            Conte4();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TipoFloresView());
        }

        void Conte1()
        {
            CLICKIMAGEN1.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new NavigationPage(new TipoFloresView());

                })
            });
        }

        void Conte2()
        {
            CLICK1magen2.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                   Application.Current.MainPage = new NavigationPage(new Bibliotecaview());

               })

            });
        }

        void Conte3()
        {
            CLICKmagen3c.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new NavigationPage(new paqfloresview());
                })
            });
        }
        void Conte4()
        {
            CLICK1magen4c.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new NavigationPage(new AgendaFloreriaView());
                })
            });
        }


    }
}