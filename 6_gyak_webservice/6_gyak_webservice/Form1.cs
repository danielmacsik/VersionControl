using _6_gyak_webservice.Entities;
using _6_gyak_webservice.ServiceReference1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace _6_gyak_webservice
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();

        
        public Form1()
        {
            InitializeComponent();
            RefreshData();

            dataGridView1.DataSource = Rates;
        }
        private void RefreshData() 
        {
            Rates.Clear();
            string result = GetExchangeRates();
            ProcessXML(result);
            DisplayData();

        }

        private string GetExchangeRates() 
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString(),
            };
            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            return result;
        }
        private void ProcessXML(string result) 
        {
            var xml = new XmlDocument();
            xml.LoadXml(result);

            // Végigmegünk a dokumentum fő elemének gyermekein
            foreach (XmlElement element in xml.DocumentElement)
            {
                // Létrehozzuk az adatsort és rögtön hozzáadjuk a listához
                // Mivel ez egy referencia típusú változó, megtehetjük, hogy előbb adjuk a listához és csak később töltjük fel a tulajdonságait
                var rate = new RateData();
                Rates.Add(rate);

                // Dátum
                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                // Valuta
                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                // Érték
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }
        }
        private void DisplayData() 
        {
            //6.1 ELŐTTE
            //6.2
            chartRateData.DataSource = Rates;
            //6.3
            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line; //6.4
            series.XValueMember = "Date";
            series.YValueMembers = "Value"; //6.5
            series.BorderWidth = 2; //6.6a

            var legend = chartRateData.Legends[0]; //6.6
            legend.Enabled = false; //6.6b

            var chartArea = chartRateData.ChartAreas[0]; //6.6
            chartArea.AxisX.MajorGrid.Enabled = false; //6.6c
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false; //6.6d
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
