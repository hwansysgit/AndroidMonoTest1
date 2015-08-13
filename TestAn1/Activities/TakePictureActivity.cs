using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Provider;
using Android.Graphics;
using Java.IO;
using System.IO;

namespace TestAn1
{
    [Activity(Label = "TakePictureActivity")]
    public class TakePictureActivity : Activity
    {
        private string _imageUri;
        int TakePictureRequestCode = 0;
        Button btn_Save;
        EditText edit_Name;
        ImageView picture;
        byte[] bitmapData = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AddNewCard);
        }

        private Boolean isMounted
        {
            get
            {
                return Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted);
            }
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            edit_Name = FindViewById<EditText>(Resource.Id.edit_Name);
            picture = FindViewById<ImageView>(Resource.Id.ImageViewer);
            btn_Save = FindViewById<Button>(Resource.Id.btn_Save);
            btn_Save.Click += Btn_Save_Click;
            edit_Name.TextChanged += Edit_Name_TextChanged;

            var uri = ContentResolver.Insert(isMounted
                                             ? MediaStore.Images.Media.ExternalContentUri
                                             : MediaStore.Images.Media.InternalContentUri, new ContentValues());
            _imageUri = uri.ToString();

            var intent = new Intent(MediaStore.ActionImageCapture);
            
            intent.PutExtra(MediaStore.ExtraOutput, uri);
            StartActivityForResult(intent, TakePictureRequestCode);
        }

        private void Edit_Name_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            btn_Save.Enabled = !string.IsNullOrWhiteSpace(edit_Name.Text);
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {            
            DataBaseController.instance().AddCard(edit_Name.Text , BitConverter.ToString(bitmapData).Replace("-", ""));
            Finish();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(resultCode == Result.Ok && requestCode == TakePictureRequestCode)
            {
                try
                {
                    Android.Net.Uri _currentImageUri = Android.Net.Uri.Parse(_imageUri);
                    Bitmap bitmap = BitmapFactory.DecodeStream(ContentResolver.OpenInputStream(_currentImageUri));

                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                        bitmapData = stream.ToArray();
                    }

                    //이미지 지워야함...
                    
                    picture.SetImageBitmap(bitmap);
                    bitmap.Dispose();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                }
                
            }
        }
    }
}