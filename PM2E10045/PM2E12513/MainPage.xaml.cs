using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PM2E12513.Controller;
using PM2E12513.Models;
using Plugin.Geolocator;
using Xamarin.Essentials;
using Plugin.Media;
using Xamarin.Forms.Xaml;
using System.IO;

namespace PM2E12513
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        string base64ball = "";
        public MainPage()
        {
            InitializeComponent();
        }
        private async void btncargarimg_Clicked(object sender, EventArgs e)
        {
            var tomarfoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "miApp",
                Name = "Image.jpg"

            });

            if (tomarfoto != null)
            {
                imagen.Source = ImageSource.FromStream(() => { return tomarfoto.GetStream(); });
            }

            Byte[] imagenByte = null;

            using (var stream = new MemoryStream())
            {
                tomarfoto.GetStream().CopyTo(stream);
                tomarfoto.Dispose();
                imagenByte = stream.ToArray();

                base64ball = Convert.ToBase64String(imagenByte);

            }

        }
        public async void GetLocation()
        {
            Location Location = await Geolocation.GetLastKnownLocationAsync();

            if (Location == null)
            {
                Location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                }); ;
            }

            txtlatitud.Text = Location.Latitude.ToString();
            txtlongitud.Text = Location.Longitude.ToString();


        } 

        

        private void btn01_Clicked(object sender, EventArgs e)
        {
            GetLocation();
        }

        private async void btn02_Clicked(object sender, EventArgs e)
        {
          
            await Navigation.PushAsync(new Views.ubicacionesPage());
        }

        private async void btnguardar_Clicked(object sender, EventArgs e)
        {
            
            try
            {
                var ubicaciones = new Models.Ubicaciones
                {
                    latitud = Convert.ToDouble(this.txtlatitud.Text),
                    longitud = Convert.ToDouble(this.txtlongitud.Text),
                    descripcion = this.txtdescripcion.Text,
                    DescripcionCorta = this.txtdcorta.Text,
                    base64 = base64ball

                };


                var resultado = await App.Basedatos02.GrabarUbicacion(ubicaciones);


                if (resultado == 1)
                {
                    await DisplayAlert("Mensaje", "Ingresada Exitosamente", "OK");
                }
                else
                {
                    await DisplayAlert("Mensaje", "Error, No se logro guardar Ubicacion", "OK");

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Mensaje", ex.Message.ToString(), "OK");

            }
        }
    }
}
