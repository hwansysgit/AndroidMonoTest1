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
using Android.Provider;
using System.IO;
using Android.Graphics;

namespace TestAn1
{
    [Activity(Label = "CardListViewActivity")]
    public class CardListViewActivity : Activity
    {
        GridView cardViwer;
        List<Cards> cardList;
        Button btn_AddCard;       

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CardListView);
            
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            cardViwer = FindViewById<GridView>(Resource.Id.grid_CardList);
            btn_AddCard = FindViewById<Button>(Resource.Id.btn_AddCard);
            btn_AddCard.Click += Btn_AddCard_Click;
            cardViwer.ItemClick += CardViwer_ItemClick;
            GetList();

        }

        private void CardViwer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string selectedName = e.View.FindViewById<TextView>(Resource.Id.text_CardName).Text;
            byte[] image = DataBaseController.instance().GetCardImage(selectedName);

            Intent cardViewIntent = new Intent(this, typeof(CardViewerActivity));
            cardViewIntent.PutExtra("ImageData", image);
            StartActivity(cardViewIntent);
        }

        private void Btn_AddCard_Click(object sender, EventArgs e)
        {
            Intent takeIntent = new Intent(this, typeof(TakePictureActivity));            
            StartActivityForResult(takeIntent, 999);            
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(requestCode == 999)
            {
                GetList();
            }
        }

        void GetList()
        {
            cardList = DataBaseController.instance().GetCardList();
            CardListAdapter adapter = new CardListAdapter(this, cardList);
            cardViwer.Adapter = adapter;
        }
    }
}