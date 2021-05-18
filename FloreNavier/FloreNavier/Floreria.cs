using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FloreNavier
{
    public enum Categorias {Ramo1, Ramo2, Ramo3, Ramo4 }

    public class Floreria : INotifyPropertyChanged
    {
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now.Date;
        private double unidades;

        public double Unidades
        {
            get { return unidades; }
            set
            {
                unidades = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Unidades)));
            }
        }

        public Categorias Categorias { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
