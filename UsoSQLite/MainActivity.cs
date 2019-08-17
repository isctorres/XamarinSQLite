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

            var pathBaseDeDatos = Path.Combine(FilesDir.AbsolutePath, "sqlitechat.sql");
            using (var connection = new SQLiteConnection(pathBaseDeDatos))
            {
                connection.DropTable<Mensaje>();
                connection.CreateTable<Mensaje>();

                if (connection.Table<Mensaje>().Count() == 0)
                {
                    List<Mensaje> mensajes = new List<Mensaje> {
                        new Mensaje {
                            usuario  = "Rubensin",
                            mensaje  = "Hola 1",
                            recibido = true
                        },
                        new Mensaje {
                            usuario  = "Zaid",
                            mensaje  = "Hola 2",
                            recibido = true
                        },
                        new Mensaje {
                            usuario  = "Rubensin",
                            mensaje  = "Hola 3",
                            recibido = true
                        },
                        new Mensaje {
                            usuario  = "Zaid",
                            mensaje  = "Hola 4",
                            recibido = false
                        },
                        new Mensaje {
                            usuario  = "Rubensin",
                            mensaje  = "Hola 5",
                            recibido = false
                        },
                        new Mensaje {
                            usuario  = "Zaid",
                            mensaje  = "Hola 6",
                            recibido = true
                        },
                        new Mensaje {
                            usuario  = "Rubensin",
                            mensaje  = "Hola 7",
                            recibido = true
                        },
                        new Mensaje {
                            usuario  = "Zaid",
                            mensaje  = "Hola 8",
                            recibido = true
                        },
                        new Mensaje {
                            usuario  = "Rubensin",
                            mensaje  = "Hola 9",
                            recibido = false
                        },
                        new Mensaje {
                            usuario  = "Zaid",
                            mensaje  = "Hola 10",
                            recibido = false
                        },
                    };
                    connection.InsertAll(mensajes);
                }

                List<Mensaje> mensajeTodos = connection.Table<Mensaje>.ToList();
                connection.Close();
                connection.Dispose();

                ListView ltvMensajes = FindViewById<ListView>(Resource.Id.ltvMensajes);
                ltvMensajes.Adater = new AdaptadorMensaje(this, mensajeTodos);
                ltvMensajes.ItemClick += (sender, e) =>
                {
                    Toast.MakeText(this, mensajeTodos[e.Position].mensaje, ToastLength.Short).show();
                };
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}