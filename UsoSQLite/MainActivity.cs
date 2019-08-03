using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

using SQLite;
using System.IO;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using UsoSQLite.Modelo;
using SQLiteNetExtensions.Extensions;
using Android.Preferences;
using Android.Content;

namespace UsoSQLite
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Context mContext = this.CreatePackageContext("com.companyname.PracticaSharedPreferences", Android.Content.PackageContextFlags.IgnoreSecurity);

            Android.Content.ISharedPreferences preferences = mContext.GetSharedPreferences("PreferenciasUsuario", Android.Content.FileCreationMode.Private);
            //Android.Content.ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            string preferenciaUsuario = preferences.GetString("Usuario", "isctorres");
            string preferenciaCorreo = preferences.GetString("Correo", "isctorres@gmail.com");

            EditText usuario = FindViewById<EditText>(Resource.Id.edtUsuario);
            usuario.Text = preferenciaUsuario;
            EditText correo = FindViewById<EditText>(Resource.Id.edtCorreo);
            correo.Text = preferenciaCorreo;

            var pathBaseDeDatos = Path.Combine(FilesDir.AbsolutePath, "libros.sql");
            using (var connection = new SQLiteConnection(pathBaseDeDatos))
            {
                connection.CreateTable<Libro>();
                connection.CreateTable<Autor>();
                connection.CreateTable<Editorial>();

                if (connection.Table<Autor>().Count() == 0)
                {
                    List<Autor> autores = new List<Autor> {
                        new Autor {
                            nombre1 = "Luis",
                            nombre2 = "Moises",
                            nombre3 = null,
                            apellidoPaterno = "Burgara",
                            apellidoMaterno = "Lopez" },
                        new Autor { nombre1 = "Luis",
                            nombre2 = null,
                            nombre3 = null,
                            apellidoPaterno = "Leithold",
                            apellidoMaterno = null }
                    };
                    connection.InsertAll(autores);
                }


                if (connection.Table<Editorial>().Count() == 0)
                {
                    List<Editorial> editoriales = new List<Editorial> {
                        new Editorial {
                            nombreEditorial = "Springer",
                            pais = "USA" },
                    new Editorial {
                            nombreEditorial = "Limusa",
                            pais = "Mexico" },
                    };

                    connection.InsertAll(editoriales);
                }

                var editorialess = connection.Table<Editorial>().Where(r => r.nombreEditorial == "Springer").ToList()[0];
                var autoress = connection.Table<Autor>().Where(r => r.apellidoPaterno == "Burgara").ToList()[0];

                if (connection.Table<Libro>().Count() == 0)
                {
                    List<Libro> libros = new List<Libro> {
                    new Libro {
                        titulo = "Calculo",
                        EditorialId = connection.Table<Editorial>().Where(r=>r.nombreEditorial=="Springer").ToList()[0].id,
                        editorial = connection.Table<Editorial>().Where(r=>r.nombreEditorial=="Springer").ToList()[0],
                        ciudad = "New York",
                        pais = "USA",
                        autores = new List<Autor>{connection.Table<Autor>().Where(r=>r.apellidoPaterno=="Leithold").ToList()[0]}
                    },
                    new Libro {
                        titulo = "Algebra",
                        EditorialId = connection.Table<Editorial>().Where(r=>r.nombreEditorial=="Limusa").ToList()[0].id,
                        editorial = connection.Table<Editorial>().Where(r=>r.nombreEditorial=="Limusa").ToList()[0],
                        ciudad = "Ciudad de Mexico",
                        pais = "Mexico",
                        autores = new List<Autor>{connection.Table<Autor>().Where(r=>r.apellidoPaterno=="Burgara").ToList()[0]}
                    }
                };

                    connection.InsertAll(libros);
                    libros[0].autores = new List<Autor> { connection.Table<Autor>().Where(r => r.apellidoPaterno == "Leithold").ToList()[0] };
                    connection.UpdateWithChildren(libros[0]);

                    libros[1].autores = new List<Autor> { connection.Table<Autor>().Where(r => r.apellidoPaterno == "Burgara").ToList()[0] };
                    connection.UpdateWithChildren(libros[1]);
                }
                List<Libro> librosTodos = connection.GetAllWithChildren<Libro>();

                var editoriale = connection.Table<Editorial>().Where(r => r.nombreEditorial == "Springer").ToList()[0];
                librosTodos[0].editorial = editoriale;
                connection.Update(librosTodos[0]);
                librosTodos = connection.Table<Libro>().ToList();
                connection.Close();
                connection.Dispose();
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}