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
using ColourSplash.Models;
using Object = Java.Lang.Object;

namespace ColourSplash.Adapter
{
    public class HighScoreAdapter : BaseAdapter<HighScore>
    {
        List<HighScore> items;
        Activity context;

        public HighScoreAdapter(Activity context, List<HighScore> items)
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.HighScoreListView, null);
            view.FindViewById<TextView>(Resource.Id.name).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.score).Text = item.Score.ToString();
            return view;
        }

        public override int Count {
            get { return items.Count; }
        }
        public override HighScore this[int position]
        {
            get { return items[position]; }
        }
    }
}