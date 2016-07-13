﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WIP2UWP;
using WIP2UWP.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WIP2UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //instance variable to get data from json file
        private ObservableCollection<Repair> JsonData;

        //instance variable for prority devices
        private ObservableCollection<Repair> Repairs;
        //instance variable for back roder summary
        private ObservableCollection<Repair> BackOrders;
        private int totalRowsPD;
        private int totalRowsBK;
        private int pageSizePD = 20;
        private int pageSizeBK = 15;
        private int pagePD;
        private int pageBK;
        //set manufacturer
        private string Manufacturer = "LG";
        
        public MainPage()
        {
            this.InitializeComponent();
            //create repairs table obejct for prority devices
            Repairs = new ObservableCollection<Repair>();
            //create repairs table obejct for back order
            BackOrders = new ObservableCollection<Repair>();

            
        }
        //load main form
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //set progress ring Visible
            MyProgressRing.Visibility = Visibility.Visible;

            //first to load the repair json data when application starts
            JsonData = await GetJsonRepair();
            //loop the loadJson method every X minutes
            LoadJson();
            //loop prority devices data
            GetPD();
            //loop back order data
            GetBK();
        }
        //call load json data method
        private void LoadJson()
        {
            LoadJsonData();
        }
        //load json data method
        private async void LoadJsonData()
        {
            while (true)
            {
                //set progress ring Visible
                MyProgressRing.Visibility = Visibility.Visible;
                //load repair json data every x*60 seconds
                JsonData = await GetJsonRepair();
                await Task.Delay(5 * 60 * 1000); //x*60 seconds
            }
        }

        private async void GetPD()
        {
            await GetDataPD();
        }
        //get data method for prority devices
        private async Task GetDataPD()
        {
            //get total rows for prority devices
            totalRowsPD = GetTotalRowsPD();
            Debug.WriteLine("totalRowsPD: " + totalRowsPD);
            pagePD = (totalRowsPD / pageSizePD) + 1;
            Debug.WriteLine("----------loopTimesPD: " + pagePD);
            //loop the prority devices
            for (int i = 0; i < pagePD; i++)
            {
                DataManagerPD(pageSizePD * i, pageSizePD);
                await Task.Delay(5000);
            }
           
            //load the method again to loop the method forever
            await GetDataPD();
        }

        private async void GetBK()
        {
            await GetDataBK();
        }
        //get data method for back order
        private async Task GetDataBK()
        {
            //get total rows for prority devices
            totalRowsBK = GetTotalRowsBK();
            Debug.WriteLine("totalRowsBK: " + totalRowsBK);
            pageBK = (totalRowsBK / pageSizeBK) + 1;
            Debug.WriteLine("----------pagesBK: " + pageBK);
            //loop the back order
            for (int i = 0; i < pageBK; i++)
            {
                DataManagerBK(pageSizeBK * i, pageSizeBK);
                await Task.Delay(5000);
            }

            //load the method again to loop the method forever
            await GetDataBK();
        }

        //get json string of table repair
        private async Task<ObservableCollection<Repair>> GetJsonRepair()
        {
           
            //initialize client to request JSON data
            var client = new HttpClient();
            //put request URL for JSON data
            HttpResponseMessage response = await client.GetAsync(new Uri("http://10.1.200.111/repair.json"));
            //get JsonString 
            var jsonString = await response.Content.ReadAsStringAsync();
            //conver json string to json obejct
            var items = JsonConvert.DeserializeObject<ObservableCollection<Repair>>(jsonString);
            //progress ring
            MyProgressRing.Visibility = Visibility.Collapsed;

            return items;
        }
        //Data manager for prority devices
        private void DataManagerPD(int skip, int take)
        {
            //set current time
            DigitalClockTextBlock.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm");
            /*---------------------------------------------------------------------------------------------------
            * 
            *-------------------------------------  Prority Devices Setion --------------------------------------
            * 
            * --------------------------------------------------------------------------------------------------*/
            //clear table of prority devices before loaded
            Repairs.Clear();
            //set total rows and pages
            PDTotalRowsTextBlock.Text = "Total: " + totalRowsPD;
            PDPageTextBlock.Text = "Page: " + (skip / pageSizePD + 1) + "/" + pagePD;

            //get data from json
            //using linq query to get priority devices for * manufacturer
            var quesryPriority = (from i in JsonData
                                         where i.Manufacturer==(Manufacturer) && i.Warranty == true &&
                                         (i.Status == "R" || i.Status == "A" || i.Status == "J")
                                         orderby i.LastTechnician ascending, i.RefNumber ascending, i.AGING descending
                                         select i).Skip(skip).Take(take);
            /*---------------------------------------------------
             * This foreach loop is used for get all queried data
             * --------------------------------------------------*/
            foreach (var item in quesryPriority)
            {
                Repairs.Add(new Repair
                {
                    RefNumber = item.RefNumber,
                    AGING = item.AGING,
                    DateIn = item.DateIn.ToString().Substring(0, 10).Remove(0, 5),
                    FuturetelLocation = item.FuturetelLocation,
                    LastTechnician = item.LastTechnician
                });
            }

            /*---------------------------------------------------
            //* This for loop is used for get number of queried data
            //* --------------------------------------------------*/
            //Repair[] item = quesrySamsungPriority.ToArray();
            //for (int i = 0; i < 20; i++)
            //{
            //    Repairs.Add(new Repair
            //    {
            //        RefNumber = item[i].RefNumber,
            //        AGING = item[i].AGING,
            //        DateIn = item[i].DateIn.ToString().Substring(0, 10).Remove(0, 5),
            //        FuturetelLocation = item[i].FuturetelLocation,
            //        LastTechnician = item[i].LastTechnician
            //    });
            //}
            
        }
        //get total rows for prority devices
        private int GetTotalRowsPD()
        {
            //count total rows for prority devices
            var prorityDeviecsRowsCount = (from i in JsonData where i.Manufacturer==(Manufacturer) && i.Warranty==true && (i.Status == "R" || i.Status == "A" || i.Status == "J") select i).Count();
            totalRowsPD = Convert.ToInt32(prorityDeviecsRowsCount);

            return totalRowsPD;
        }
        
        //Data manager for back order
        private void DataManagerBK(int skip, int take)
        {

            /*---------------------------------------------------------------------------------------------------
            * 
            * ------------------------------------ BackOrder Summary Setion -------------------------------------
            * 
            * --------------------------------------------------------------------------------------------------*/
            //clear table of backOrder before loaded
            BackOrders.Clear();
            //set total rows and pages
            BKTotalRowsTextBlock.Text = "Total: " + totalRowsBK;
            BKPageTextBlock.Text = "Page: " + (skip / pageSizeBK + 1) + "/" + pageBK;
            //get data from json
            //using linq query to get priority devices for * manufacturer
            var quesryBackOrder = (from i in JsonData
                                   where i.Manufacturer == (Manufacturer) &&
                                   (i.Status == "B")
                                   orderby i.RefNumber ascending, i.AGING descending
                                   select i).Skip(skip).Take(take);
            /*---------------------------------------------------
             * This foreach loop is used for get all queried data
             * --------------------------------------------------*/
            foreach (var item in quesryBackOrder)
            {
                BackOrders.Add(new Repair
                {
                    RefNumber = item.RefNumber,
                    AGING = item.AGING,
                    LastTechnician = item.LastTechnician
                });
            }
            
        }
        //get total rows for backOrder
        private int GetTotalRowsBK()
        {
            //count total rows for prority devices
            var prorityDeviecsRowsCount = (from i in JsonData where i.Manufacturer == (Manufacturer) && (i.Status == "B") select i).Count();
            totalRowsPD = Convert.ToInt32(prorityDeviecsRowsCount);

            return totalRowsPD;
        }



    }
}
