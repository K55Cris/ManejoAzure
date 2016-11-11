
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ManejoDatosAzure
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IMobileServiceTable<suscriptores> todoTable = App.MobileService.GetTable<suscriptores>();

        public MainPage()
        {
            this.InitializeComponent();
          
        }
        List<string> listaCompleta = new List<string>();
        private async void consultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            //  List<suscriptores> lista = new List<suscriptores>();

                listaCompleta = await todoTable.Select(sub => string.Format("{0}",sub.nombre)).ToListAsync();

            //  lista = await todoTable.ToListAsync();
              lis.ItemsSource = listaCompleta;
            //  lis.DisplayMemberPath = "nombre";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: '{0}'", ex);
            }
        }

        private async void enviar_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msgbox = new MessageDialog("Desea registrarlo",Getnombre.Text);

            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });

            var res = await msgbox.ShowAsync();

            if ((int)res.Id == 0)
            {
                var nuevo = new suscriptores
                {
                    nombre = Getnombre.Text
                };
                InsertNewMensaje(nuevo);
            }
        }
        private async void InsertNewMensaje(suscriptores elemento)
        {
            await todoTable.InsertAsync(elemento);
            MessageDialog Mensaje = new MessageDialog("enviado");
            Mensaje.Title = "correcto";
            var result = await Mensaje.ShowAsync();
        }



        private async void modificar_Click(object sender, RoutedEventArgs e)
        {

            // IMobileServiceTable<suscriptores> todoTable = App.MobileService.GetTable<suscriptores>();
            var person = await todoTable.Where(p => p.nombre == lis.SelectedValue.ToString()).ToListAsync();
           // var person2 = await todoTable.Where(p => p.id == lis.SelectedValue.ToString()).ToListAsync();

            foreach (var item in person)
            {
                item.nombre = Getnombre.Text;
                await todoTable.UpdateAsync(item);
            }

            MessageDialog Mensaje = new MessageDialog("Modificado");
            Mensaje.Title = "correcto";
            var result = await Mensaje.ShowAsync();
        }

        private async void eliminar_Click(object sender, RoutedEventArgs e)
        {
            var person = await todoTable.Where(p => p.nombre == lis.SelectedValue.ToString()).ToListAsync();

            MessageDialog msgbox = new MessageDialog("Desea Eliminar", lis.SelectedValue.ToString());
            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await msgbox.ShowAsync();
            if ((int)res.Id == 0)
            {
                foreach (var item in person)
                {
                    item.nombre = Getnombre.Text;
                    await todoTable.DeleteAsync(item);
                }
            }
            MessageDialog msgbox2 = new MessageDialog("Eliminado");
            await msgbox2.ShowAsync();

        }

        private void lis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lis.SelectedItem!=null)
            Getnombre.Text = lis.SelectedValue.ToString();
        }
    }
}
