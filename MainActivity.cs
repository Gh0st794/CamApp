using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;

namespace CamApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnCaptureImg;
        ImageView imgView;        

        readonly string[] permisos =
        {
            Android.Manifest.Permission.ReadExternalStorage,
            Android.Manifest.Permission.WriteExternalStorage,
            Android.Manifest.Permission.Camera
        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //Referencias a los elementos de la interfaz
            btnCaptureImg = (Button)FindViewById(Resource.Id.captureButtonImg);
            imgView = (ImageView)FindViewById(Resource.Id.imageView1);           

            btnCaptureImg.Click += BtnCaptureImg_Click;           
            RequestPermissions(permisos, 0);
        }

        private void BtnCaptureImg_Click(object sender, System.EventArgs e)
        {
            captureImg();
        }

        async void captureImg()
        {
            await Plugin.Media.CrossMedia.Current.Initialize();

            var img = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "CamAppImg.jpg",
                Directory = "CamAppImages",
                SaveToAlbum = true
            });

            if (img == null)
            {
                return;
            }

            //Convertir imagen capturada en un arreglo de bytes y retornar el bitmap al imageView

            byte[] imgArray = System.IO.File.ReadAllBytes(img.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imgArray, 0, imgArray.Length);
            imgView.SetImageBitmap(bitmap);

        }        

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}