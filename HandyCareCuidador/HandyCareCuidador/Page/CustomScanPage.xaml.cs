﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace HandyCareCuidador.Page
{
    public partial class CustomScanPage : ContentPage
    {
        public CustomScanPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            BarcodeImageView.RemoveBinding(ZXingScannerView.IsScanningProperty);
            BarcodeImageView.IsScanning = false;

            base.OnDisappearing();
        }
    }
}
