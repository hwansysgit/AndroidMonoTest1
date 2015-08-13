using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TestAn1
{
    [Activity(Label = "Secret Card Manager", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button btn_Login;
        EditText edit_Password;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            btn_Login = FindViewById<Button>(Resource.Id.btn_Login);
            edit_Password = FindViewById<EditText>(Resource.Id.edit_Password);
            edit_Password.TextChanged += Edit_Password_TextChanged;
            btn_Login.Click += Btn_Login_Click;

            DataBaseController.instance().Open();
            
            int count = DataBaseController.instance().GetUserInfo();
            
            if (count == 0)
            {
                Intent createAccoutnIntent = new Intent(this, typeof(CreateAccountActivity));
                StartActivity(createAccoutnIntent);
            }
            
        }

        private void Edit_Password_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            btn_Login.Enabled = (!string.IsNullOrWhiteSpace(edit_Password.Text));
        }

        private void Btn_Login_Click(object sender, EventArgs e)
        {
            string uuid = Android.Provider.Settings.Secure.GetString(this.ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            if(DataBaseController.instance().Login(uuid , edit_Password.Text) == false)
            {
                AlertDialog.Builder dlg = new AlertDialog.Builder(this);
                dlg.SetTitle("Password is not Correct");
                dlg.SetMessage(edit_Password.Text);
                dlg.SetNeutralButton("OK", (send, arg) => { dlg.Dispose(); });
                dlg.Show();
            }else
            {
                Intent cardListIntent = new Intent(this, typeof(CardListViewActivity));
                StartActivity(cardListIntent);
            }
        }
    }
}

