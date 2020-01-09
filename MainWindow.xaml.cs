using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EntityFramework
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private tema6Entities contexto;

        public MainWindow()
        {
            InitializeComponent();

            //Instanciamos el contexto
            contexto = new tema6Entities();

            //Cargamos el listado de clientes
            contexto.CLIENTES.Load();

            //Establecemos como DataContext de la ventana el listado de clientes
            this.DataContext = contexto.CLIENTES.Local;

            //El DataContext en la pestaña de inserción será un cliente nuevo
            InsertarStackPanel.DataContext = new CLIENTE();

            //Para la pestaña de filtrado, definimos y configuramos una CollectionViewSource
            CollectionViewSource vista = new CollectionViewSource();
            vista.Source = contexto.CLIENTES.Local;
            ClientesFiltrarDataGrid.DataContext = vista;
            vista.Filter += Vista_Filter;

        }
        
        private void Vista_Filter(object sender, FilterEventArgs e)
        {
            CLIENTE item = (CLIENTE)e.Item;

            if (FiltrarTextBox.Text.Trim() == "")
                e.Accepted = true;
            else
            {
                if (item.nombre.Contains(FiltrarTextBox.Text.Trim()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void InsertarButton_Click(object sender, RoutedEventArgs e)
        {
            InsertarButton.IsEnabled = false;
            contexto.CLIENTES.Add(InsertarStackPanel.DataContext as CLIENTE);
            contexto.SaveChanges();
            InsertarStackPanel.DataContext = new CLIENTE();
            InsertarButton.IsEnabled = true;
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            EliminarButton.IsEnabled = false;
            contexto.CLIENTES.Remove(EliminarComboBox.SelectedItem as CLIENTE);
            contexto.SaveChanges();
            EliminarButton.IsEnabled = true;
        }

        private void ModificarButton_Click(object sender, RoutedEventArgs e)
        {
            ModificarButton.IsEnabled = false;
            contexto.SaveChanges();
            ModificarButton.IsEnabled = true;
        }

        private void ActualizarButton_Click(object sender, RoutedEventArgs e)
        {
            ActualizarButton.IsEnabled = false;
            contexto.SaveChanges();
            ActualizarButton.IsEnabled = true;
        }

        private void FiltrarButton_Click(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource)ClientesFiltrarDataGrid.DataContext).View.Refresh();
        }
    }
}
