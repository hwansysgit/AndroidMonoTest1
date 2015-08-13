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

namespace TestAn1
{
    [Activity(Label = "CreateAccountActivity")]
    public class CreateAccountActivity : Activity
    {
        Button btn_Create;
        EditText edit_Pwd;
        EditText edit_Pwd2;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CreateView);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            btn_Create = FindViewById<Button>(Resource.Id.btn_Create);
            edit_Pwd = FindViewById<EditText>(Resource.Id.edit_Password_C);
            edit_Pwd2 = FindViewById<EditText>(Resource.Id.edit_Password_C_Re);

            btn_Create.Click += Btn_Create_Click;
            edit_Pwd.TextChanged += Edit_Pwd_TextChanged;
            edit_Pwd2.TextChanged += Edit_Pwd2_TextChanged;
        }

        private void Edit_Pwd2_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            btn_Create.Enabled = edit_Pwd.Text.Equals(edit_Pwd2.Text);
        }

        private void Edit_Pwd_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            edit_Pwd2.Enabled = (!string.IsNullOrWhiteSpace(edit_Pwd.Text));
        }

        private void Btn_Create_Click(object sender, EventArgs e)
        {
            string uuid = Android.Provider.Settings.Secure.GetString(this.ContentResolver , Android.Provider.Settings.Secure.AndroidId);
            DataBaseController.instance().AddAccount(uuid , edit_Pwd.Text);
            Finish();
        }
    }
}