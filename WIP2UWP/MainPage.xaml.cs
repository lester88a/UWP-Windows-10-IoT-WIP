using Newtonsoft.Json;
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
        private ObservableCollection<Repair> Repairs;
        private int totalRows;
        private int pageSizePD = 20;

        //get data from json
        private ObservableCollection<Repair> items;

        public MainPage()
        {
            this.InitializeComponent();
            //create repairs table obejct
            Repairs = new ObservableCollection<Repair>();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //set progress ring Visible
            MyProgressRing.Visibility = Visibility.Visible;
            

            //load the prority devices
            await GetPD();

        }
        //get prority devices data method
        private async Task GetPD()
        {
            //load repair data
            items = await GetJsonRepair();

            //get total rows
            totalRows = GetTotalRowsPD();
            int totalPages = totalRows / pageSizePD;
            Debug.WriteLine("Total Rows:\n" + totalRows);
            Debug.WriteLine("total Pages:\n" + totalPages);
            //loop the prority devices
            for (int i = 0; i < (totalRows / pageSizePD)+1; i++)
            {
                GetPriorityDevices(pageSizePD * i, pageSizePD);
                await Task.Delay(10000);
            }
            //set progress ring Visible
            MyProgressRing.Visibility = Visibility.Visible;
            //load the method again to loop the method forever
            await GetPD();
        }



        //get json string of table repair
        private static async Task<ObservableCollection<Repair>> GetJsonRepair()
        {
            //initialize client to request JSON data
            var client = new HttpClient();
            //put request URL for JSON data
            HttpResponseMessage response = await client.GetAsync(new Uri("http://10.1.200.111/repair.json"));
            //get JsonString 
            var jsonString = await response.Content.ReadAsStringAsync();
            //conver json string to json obejct
            var items = JsonConvert.DeserializeObject<ObservableCollection<Repair>>(jsonString);
            return items;
        }
        //get priority devices
        private void GetPriorityDevices(int skip, int take)
        {
            //set progress ring Visible
            MyProgressRing.Visibility = Visibility.Visible;
            //clear table of prority devices before loaded
            Repairs.Clear();

            //set total rows and pages
            totalRows = GetTotalRowsPD();
            PDTotalRowsTextBlock.Text = "Total: " + totalRows;
            PDPageTextBlock.Text = "Page: " + (skip / pageSizePD + 1) + "/" + (totalRows / pageSizePD +1);

            //get data from json
            //ObservableCollection<Repair> items = await GetJsonRepair();
            //using linq query to get priority devices for * manufacturer
            var quesrySamsungPriority = (from i in items
                                         where i.Manufacturer.Contains("SAMSUNG") && i.Warranty == true &&
                                         (i.Status == "R" || i.Status == "A" || i.Status == "J")
                                         orderby i.LastTechnician ascending, i.RefNumber ascending, i.AGING descending
                                         select i).Skip(skip).Take(take);
            /*---------------------------------------------------
             * This foreach loop is used for get all queried data
             * --------------------------------------------------*/
            foreach (var item in quesrySamsungPriority)
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



            //progress ring
            MyProgressRing.Visibility = Visibility.Collapsed;

        }


        //get total rows for prority devices
        private int GetTotalRowsPD()
        {
            //get json string
            //ObservableCollection<Repair> items = await GetJsonRepair();
            //count total rows for prority devices
            var prorityDeviecsRowsCount = (from i in items where i.Manufacturer.Contains("SAMSUNG") && i.Warranty==true && (i.Status == "R" || i.Status == "A" || i.Status == "J") select i).Count();
            totalRows = Convert.ToInt32(prorityDeviecsRowsCount);

            return totalRows;
        }


        private string ConvertDate(string dateString)
        {
            //DateTime date = DateTime.ParseExact(dateString, "MMM dd", null);
            //string date = dateString.ToString("dd-MM-yyyy");
            DateTime dt = DateTime.Parse(dateString);
            string s2 = dt.ToString("dd-MM");
            DateTime dtnew = DateTime.Parse(s2);
            return dtnew.ToString();
        }

        private void GetPDDataButton_Click(object sender, RoutedEventArgs e)
        {
            //GetPriorityDevices();
        }
    }
}
