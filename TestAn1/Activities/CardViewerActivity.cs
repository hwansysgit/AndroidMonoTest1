using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;

namespace TestAn1
{
    [Activity(Label = "CardViewerActivity")]
    public class CardViewerActivity : Activity
    {
        ImageView viewer;
        Button btn_Close;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CardViewer);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            viewer = FindViewById<ImageView>(Resource.Id.CardImageViewer);
            btn_Close = FindViewById<Button>(Resource.Id.btn_CloseViewer);
            btn_Close.Click += Btn_Close_Click;
            byte[] image = Intent.GetByteArrayExtra("ImageData");
            Bitmap bmp = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            viewer.SetImageBitmap(bmp);

            Title = Intent.GetStringExtra("ImageName"); 
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}