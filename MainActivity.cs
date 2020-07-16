using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using CalorApp.WebReference;
using System;

namespace CalorApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        Service1 clientwcf;
        static bool checkValue = false;
        TextView txtUserName;
        TextView txtPassword;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //LogIn Form
            txtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            txtPassword = FindViewById<TextView>(Resource.Id.txtPassword);
            Button login_Button = FindViewById<Button>(Resource.Id.btnLogin);


            //WCF Client initialize after Click LogIn - sprawdzenie czy użytkownik ma konto w serwisie
            clientwcf = new Service1();
            login_Button.Click += delegate
            {
                clientwcf.CheckUserAsync(txtUserName.Text, txtPassword.Text);
                clientwcf.CheckUserCompleted += Clientwcf_CheckUserCompleted;
                System.Threading.Thread.Sleep(1000);

            };
        }

        private void Clientwcf_CheckUserCompleted(object sender, CheckUserCompletedEventArgs e)
        {
            
            SetCheckValue(e.CheckUserResult);
            var check = "Result of checking user: " + checkValue;
            Toast.MakeText(this, check, ToastLength.Short).Show();
            if (checkValue == true)
            {
                Intent listActivity = new Intent(this, typeof(ListActivity));
                listActivity.PutExtra("userlogin", txtUserName.Text);
                StartActivity(listActivity);
                txtUserName.Text = "";
                txtPassword.Text = "";

            }
        }

        static void SetCheckValue(bool x)
        {
            checkValue = x;
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}