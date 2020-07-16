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
using CalorApp.Resources.Model;

namespace CalorApp.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView textMeal { get; set; }
        public TextView textKcal { get; set; }
        public TextView textDate { get; set; }

    }

    public class ListViewAdapter:BaseAdapter
    {
        private Activity activity;
        private List<Meal> listMeal;
        public ListViewAdapter(Activity activity, List<Meal> listMeal)
        {
            this.activity = activity;
            this.listMeal = listMeal;
        }

        public override int Count
        {
            get
            {
                return listMeal.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return listMeal[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view, parent, false);
            var textMeal = view.FindViewById<TextView>(Resource.Id.textMeal);
            var textKcal = view.FindViewById<TextView>(Resource.Id.textKcal);
            var textDate = view.FindViewById<TextView>(Resource.Id.textDate);

            textMeal.Text = listMeal[position].Name;
            textKcal.Text = Convert.ToString(listMeal[position].Kcal);
            textDate.Text = Convert.ToString(listMeal[position].Date);

            return view;
        }
    }


}