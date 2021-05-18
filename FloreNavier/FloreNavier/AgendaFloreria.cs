using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FloreNavier
{
    public class AgendaFloreria : INotifyPropertyChanged
    {
        public AgendaFloreria()
        {
            Abrir();
            AgregarCommand = new Command(NuevoPedido);
            NavegarAgregarCommand = new Command(NavegarAgregar);
            FiltrarCommand = new Command(Filtrar);
            EditarCommand = new Command<Floreria>(NavegarEditar);
            GuardarCommand = new Command(EditarPedido);
            EliminarCommand = new Command<Floreria>(NavegarEliminar);
            RegresarCommand = new Command(Regresar);
            Filtrar();
        }



        public ObservableCollection<Floreria> Florerias { get; set; }
        public ObservableCollection<Floreria> Filtrada { get; set; } = new ObservableCollection<Floreria>();

        public ICommand AgregarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand NavegarAgregarCommand { get; set; }
        public ICommand FiltrarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand RegresarCommand { get; set; }

        private Floreria floreria;

        public Floreria Floreria
        {
            get { return floreria; }
            set
            {
                floreria = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Floreria)));
            }
        }

        public Array Categoria
        {
            get { return Enum.GetValues(typeof(Categorias)); }
        }

        private DateTime fecha = DateTime.Now;

        public DateTime Fecha
        {
            get { return fecha; }
            set
            {
                fecha = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Fecha)));
            }
        }


        private string error;

        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
            }
        }


        private void NavegarAgregar()
        {
            //Gasto de memoria, deberia instanciar solo en el primer uso
            AgregarFloreriaView view = new AgregarFloreriaView() { BindingContext = this };
            Floreria = new Floreria();
            Application.Current.MainPage.Navigation.PushAsync(view);
        }
        private void Regresar()
        {
           
            CategoriasFlor view = new CategoriasFlor() { BindingContext = this };
            Floreria = new Floreria();
            Application.Current.MainPage.Navigation.PushAsync(view);
        }
        EditarAgregarView viewEditar;
        int posPedidoOriginal;

        private void NavegarEditar(Floreria original)
        {
            //se instancia solamente el primer uso y se reutiliza a partir de ahi
            if (viewEditar == null)
            {
                viewEditar = new EditarAgregarView() { BindingContext = this };
            }
            Floreria clon = new Floreria()
            {
                Nombre = original.Nombre,
                Unidades = original.Unidades,
                Fecha = original.Fecha,
                Categorias = original.Categorias
            };
            Floreria = clon;
            //Indice del objeto original
            posPedidoOriginal = Florerias.IndexOf(original);

            Application.Current.MainPage.Navigation.PushAsync(viewEditar);

        }

        private async void NavegarEliminar(Floreria obj)
        {
            var resultado = await Application.Current.MainPage
                .DisplayActionSheet("Confirmación", "No", "Si", "¿Desea eliminar el pedido seleccionado");

            if (resultado == "Si")
            {

                Floreria = obj;
                Eliminar();
            }
        }

        public void NuevoPedido()
        {
            if (Floreria != null)
            {
                Error = "";
                //Validar
                if (string.IsNullOrWhiteSpace(Floreria.Nombre))
                {
                    Error += "- Escriba el nombre del cliente.\n";
                }
                if (Floreria.Fecha < DateTime.Now.Date)
                {
                    Error += "- La fecha del pedido es incorrecta.\n";
                }

                double partedecimal = Floreria.Unidades - (int)Floreria.Unidades;

                if (Floreria.Unidades <= 0 || (partedecimal != 0.0 && partedecimal != 0.5)) //1, 1.5, 2, 3.65
                {
                    Error += "- Especifique el precio del ramo.\n";
                }
                //Regresar (salir del metodo) si hubo error
                if (Error != "")
                {
                    return;
                }

                //No hubo ninguno error de validación

                var p = Florerias.FirstOrDefault(x => x.Nombre == Floreria.Nombre && x.Categorias == Floreria.Categorias && x.Fecha == Floreria.Fecha);

                if (p != null)
                {
                    p.Unidades += Floreria.Unidades;
                }
                else
                {
                    Florerias.Add(Floreria);
                    Filtrar();
                }

                Application.Current.MainPage.Navigation.PopAsync();

                Guardar();
            }
        }

        public void EditarPedido()
        {
            if (Floreria != null)
            {
                Error = "";
                //Validar
                if (Floreria.Fecha < DateTime.Now.Date)
                {
                    Error += "- La fecha del pedido es incorrecta.\n";
                }

                double partedecimal = Floreria.Unidades - (int)Floreria.Unidades;

                if (Floreria.Unidades <= 0 || (partedecimal != 0.0 && partedecimal != 0.5)) //1, 1.5, 2, 3.65
                {
                    Error += "- Especifique el precio del ramo\n";
                }

                if (Error == "")
                {
                    Florerias[posPedidoOriginal] = Floreria;
                    Guardar();
                    Filtrar();
                    Application.Current.MainPage.Navigation.PopAsync();
                }

            }

        }

        private void Filtrar()
        {
            Filtrada.Clear();
            var objetos = Florerias.Where(x => x.Fecha.Date == Fecha.Date).OrderBy(x => x.Nombre);

            foreach (var item in objetos)
            {
                Filtrada.Add(item);
            }
        }


        public void Eliminar()
        {
            if (Floreria != null)
            {
                if (Florerias.Remove(Floreria))
                {
                    Guardar();
                    Filtrar();
                }
            }
        }

        private void Guardar()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var archivo = File.CreateText(ruta + "/pedidos.json");
            string json = JsonConvert.SerializeObject(Florerias);
            archivo.Write(json);
            archivo.Close();
        }


        private void Abrir()
        {
            try
            {
                var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var archivo = File.OpenText(ruta + "/pedidos.json");
                string json = archivo.ReadToEnd();
                Florerias = JsonConvert.DeserializeObject<ObservableCollection<Floreria>>(json);
                archivo.Close();
            }
            catch
            {
                Florerias = new ObservableCollection<Floreria>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
