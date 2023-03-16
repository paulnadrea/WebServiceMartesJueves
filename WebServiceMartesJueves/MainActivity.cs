using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace WebServiceMartesJueves
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            EditText txtId = FindViewById<EditText>(Resource.Id.txtId);
            EditText txtNombre = FindViewById<EditText>(Resource.Id.txtNombre);
            EditText txtDescripcion = FindViewById<EditText>(Resource.Id.txtDescripcion);
            Button btnConsultar = FindViewById<Button>(Resource.Id.btnConsultar);

            
            string uriServicio = "https://jsonplaceholder.typicode.com/posts";

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            btnConsultar.Click += async (sender, e) =>
            {
                try
                {
                    Cliente cliente = new Cliente();
                    if (!string.IsNullOrWhiteSpace(txtId.Text))
                    {
                        int t = 0;
                        if (int.TryParse(txtId.Text.Trim(), out t))
                        {
                            var resultado = await cliente.Get<Entrada>(uriServicio + "/" + txtId.Text);
                            if (cliente.codigoHTTP == 200) 
                            {
                                txtNombre.Text = resultado.title;
                                txtDescripcion.Text = resultado.body;
                                Toast.MakeText(this, "Consulta realizada con éxito", ToastLength.Long).Show();
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
                }
            };




        }

     
    }

    public class Entrada
    {
        //Es el contructor para inicializar los valores
        public Entrada()
        {
            userId = 1;
            id = 0;
            title = "";
            body = "";
        }

        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    public class Cliente
    {
        public Cliente()
        {
            codigoHTTP = 200;
        }
        public int codigoHTTP { get; set; }

        //Get
        public async Task<T> Get<T>(string url)
        {
            HttpClient cliente = new HttpClient();
            var response = await cliente.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            codigoHTTP = (int)response.StatusCode;
            return JsonConvert.DeserializeObject<T>(json);
        }

        //Post
        public async Task<T> Post<T>(Entrada item, string url)
        {
            HttpClient cliente = new HttpClient();
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await cliente.PostAsync(url, content);
            json = await response.Content.ReadAsStringAsync();
            codigoHTTP = (int)response.StatusCode;
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}