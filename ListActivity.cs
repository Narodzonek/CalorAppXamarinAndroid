using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CalorApp.Resources;
using CalorApp.Resources.DataHelper;
using CalorApp.Resources.Model;

namespace CalorApp
{
    [Activity(Label = "Your Meal List", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ListActivity : Activity
    {
        ListView listData;
        List<Meal> listSource = new List<Meal>();
        DataBase db;

        TextView sumcalories;
        int sumaryCal = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set view
            SetContentView(Resource.Layout.activity_list);

            //Create DataBase
            db = new DataBase();
            db.createDataBase();
            

            //List Content Header and Back Button
            string username = Intent.GetStringExtra("userlogin" ?? "Not recv");
            var helloName = FindViewById<TextView>(Resource.Id.helloUserName);
            var btnLogout = FindViewById<Button>(Resource.Id.btnBack);
            helloName.Text = username;
            
            //Logout
            btnLogout.Click += delegate
            {
                Intent mainActivity = new Intent(this, typeof(MainActivity));
                StartActivity(mainActivity);
            };

            //Meal list editor
            listData = FindViewById<ListView>(Resource.Id.listContentView);
            var editName = FindViewById<EditText>(Resource.Id.foodName);
            var editKcal = FindViewById<EditText>(Resource.Id.kcalNumber);
            var editDate = FindViewById<EditText>(Resource.Id.dateValue);
            var btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            var btnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            var btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
            sumcalories = FindViewById<TextView>(Resource.Id.sumofcal);

            //Load Data
            LoadData();

            //Event
            btnAdd.Click += delegate
            {
                Meal meal = new Meal()
                {
                    Name = editName.Text,
                    Kcal = int.Parse(editKcal.Text),
                    Date = editDate.Text
                };
                sumCalories(meal.Kcal);
                db.insertIntoTableMeal(meal);
                LoadData();
                editName.Text = "";
                editKcal.Text = "";
                editDate.Text = "";
            };

            btnEdit.Click += delegate
            {
                Meal meal = new Meal()
                {
                    Id = int.Parse(editName.Tag.ToString()),
                    Name = editName.Text,
                    Kcal = int.Parse(editKcal.Text),
                    Date = editDate.Text
                };
                
                db.updateTableMeal(meal);
                LoadData();
                updateCalories();
                editName.Text = "";
                editKcal.Text = "";
                editDate.Text = "";
            };

            btnDelete.Click += delegate
            {
                Meal meal = new Meal()
                {
                    Id = int.Parse(editName.Tag.ToString()),
                    Name = editName.Text,
                    Kcal = int.Parse(editKcal.Text),
                    Date = editDate.Text
                };
                delCalories(meal.Kcal);
                db.deleteTableMeal(meal);
                LoadData();
                
                editName.Text = "";
                editKcal.Text = "";
                editDate.Text = "";
            };

            listData.ItemClick += (s,e) => {
                for(int i = 0; i < listData.Count; i++)
                {
                    if(e.Position == i)
                    {
                        listData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.LightGray);
                    }
                    else
                    {
                        listData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
                    }
                }

                var textName = e.View.FindViewById<TextView>(Resource.Id.textMeal);
                var textKcal = e.View.FindViewById<TextView>(Resource.Id.textKcal);
                var textDate = e.View.FindViewById<TextView>(Resource.Id.textDate);

                editName.Text = textName.Text;
                editName.Tag = e.Id;
                editKcal.Text = textKcal.Text;
                editDate.Text = textDate.Text;
            };

            updateCalories();
        }

        private void LoadData()
        {
            listSource = db.selectTableMeal();
            var adapter = new ListViewAdapter(this, listSource);
            listData.Adapter = adapter;
            
        }

        private void sumCalories(int kal)
        {
            sumaryCal += kal;
            sumcalories.Text = sumaryCal.ToString();
        }

        private void delCalories(int kal)
        {
            sumaryCal -= kal;
            sumcalories.Text = sumaryCal.ToString();
        }

        private void updateCalories()
        {
            sumaryCal = 0;
            for (int i = 0; i < listSource.Count; i++)
            {
                var meal = listSource[i];
                sumCalories(meal.Kcal);

            }
        }
    }
}