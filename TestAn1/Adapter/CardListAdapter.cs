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
    class CardListAdapter : BaseAdapter<Cards>
    {
        private readonly Context context;
        LayoutInflater inflater;
        List<Cards> cardList;

        public override int Count
        {
            get
            {
                return cardList.Count;
            }
        }

        public override Cards this[int position]
        {
            get
            {
                return cardList[position];
            }
        }

        public CardListAdapter(Context con ,List<Cards> c)
        {
            context = con;
            cardList = c;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View cViwe = null;

            try
            {
                
                if (convertView == null)
                {
                    cViwe = LayoutInflater.From(context).Inflate(Resource.Layout.CardItem, parent, false);
                }
                else
                {
                    cViwe = convertView;
                }

                var editName = cViwe.FindViewById<TextView>(Resource.Id.text_CardName);
                editName.Text = cardList[position].NAME;
                var editDate = cViwe.FindViewById<TextView>(Resource.Id.text_RegDate);
                editDate.Text = cardList[position].REGDATE;

                cViwe.Click += CViwe_Click;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return cViwe;

        }

        private void CViwe_Click(object sender, EventArgs e)
        {
            
        }
    }
}