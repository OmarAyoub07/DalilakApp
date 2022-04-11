using DalilakAPI.Models;
using DalilakApp.Models.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DalilakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulPage : ContentPage
    {
        private Services.DalilakapiService api = new Services.DalilakapiService();
        private List<Schedules> schedules = new List<Schedules>();
        private List<SchdlsListForm> schdlsList = new List<SchdlsListForm>();

        private List<string> placesIDs = new List<string>();

        private Schedules view_schedule = new Schedules();
        private Schedules db_schedule = new Schedules();


        private int index = 0;
        public SchedulPage()
        {
            InitializeComponent();
            GetScheduals();
            btn_back.IsVisible = false;
        }

        private void btn_view_Schdl(object sender, EventArgs e)
        {
            var button = sender as Button;
            btn_back.IsVisible = true;
            index = button.TabIndex;

            var temp_View = new List<SchdlViewForm>();
            Headr.Text = "Trip Schedule to "+schedules[index].city_id;
            foreach (var day in schedules[index].days)
            {
                foreach(var hour in day.hours)
                {
                    temp_View.Add(new SchdlViewForm { date=day.date, distination = hour.place_id+" - At "+hour.time});
                }
            }
            //BindableLayout.SetItemTemplate(form_ViewOneSchdl, table);
            BindableLayout.SetItemsSource(table, temp_View);

            form_listOfSchdls.IsVisible = false;
            form_ViewOneSchdl.IsVisible = true;
        }

        private void btn_viewList(object sender, EventArgs e)
        {
            back();
        }

        private async void btn_Add_Schdl(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("How do you want to add your trip Schedule ?",
                "Cancel", null, "Manual Adding", "Auto Adding (AI-Demo)");

            try 
            { 
            if (action.Contains("Manual"))
            {
                form_AddManualSchdl.IsVisible = true;
                btn_back.IsVisible = true;
                lbl_slctPlace.IsVisible = true;
                lbl_slctTime.IsVisible = true;
                pkr_time.IsVisible = true;
                placesPicker.IsVisible = true;



                form_listOfSchdls.IsVisible = false;
                pkr_date2.IsVisible = false;
                lbl_toDate.IsVisible = false;
                rdlst_VisitsRate.IsVisible = false;

                btn_savePlan.Text = "Add this record to your plan";
                //Headr.Text = "Manage New Trip Plan";

                db_schedule.city_id = App.cityID;
                db_schedule.user_id = App.user.id;

                for(int i = 0; i < 3; i++)
                {
                    string type = i == 0 ? "NAT" : i == 1 ? "HIS" : "VNT";
                    var places = await api.GetPlaces(App.cityID, type);
                    foreach (var p in places)
                    {
                        placesPicker.Items.Add(p.name);
                        placesIDs.Add(p.id);
                    }
                }
            }
            else if (action.Contains("Auto"))
            {
                form_AddManualSchdl.IsVisible = true;
                btn_back.IsVisible = true;
                pkr_date2.IsVisible = true;
                rdlst_VisitsRate.IsVisible = true;
                lbl_toDate.IsVisible = true;


                lbl_slctPlace.IsVisible = false;
                form_listOfSchdls.IsVisible = false;
                placesPicker.IsVisible = false;
                lbl_slctTime.IsVisible = false;
                pkr_time.IsVisible = false;

                Headr.Text = "Recommender Expert System";
                btn_savePlan.Text = "Generate Schedule";
            }
            }
            catch (Exception ex)
            {
                
            }

        }

        private async void btn_addTo_plan(object sender, EventArgs e)
        {
            if (!pkr_date2.IsVisible)
            {
                if (placesPicker.SelectedItem.ToString().Length > 13)
                    UpdatePlan_View(pkr_date1.Date.ToString("dd/MM/yyyy"), pkr_time.Time.ToString(), placesPicker.SelectedItem.ToString().Substring(0, 13));

                else
                    UpdatePlan_View(pkr_date1.Date.ToString("dd/MM/yyyy"), pkr_time.Time.ToString(), placesPicker.SelectedItem.ToString());

                UpdateScheduel_db(pkr_date1.Date.ToString("dd/MM/yyyy"), pkr_time.Time.ToString(), placesIDs[placesPicker.SelectedIndex]);
            }
            else
            {
                float visitsRate = 0f;
                if (rdbtn_high.IsChecked)
                    visitsRate = float.Parse((string)rdbtn_high.Value);
                else if (rdbtn_low.IsChecked)
                    visitsRate = float.Parse((string)rdbtn_low.Value);
                else if (rdbtn_lght.IsChecked)
                    visitsRate = float.Parse((string)rdbtn_lght.Value);
                else
                    visitsRate = float.Parse((string)rdbtn_norm.Value);

                var list = await api.GenerateSchdl(App.user.id, App.cityID,
                                                   pkr_date1.Date.ToString("dd/MM/yyyy"),
                                                   pkr_date2.Date.ToString("dd/MM/yyyy"), visitsRate);
                view_schedule = list[0];
                db_schedule = list[1];

                var temp_View = new List<SchdlViewForm>();
                foreach (var d in view_schedule.days)
                {
                    foreach (var h in d.hours)
                    {
                        temp_View.Add(new SchdlViewForm { date=d.date, distination = h.place_id+"-At "+h.time });
                    }
                }
                BindableLayout.SetItemsSource(newPlantable, temp_View);
            }
        }

        private void btn_remove_fromPlan(object sender, EventArgs e)
        {
            
        }

        private async void btn_save_plan(object sender, EventArgs e)
        {
            bool isPosted = await api.PostSchedules(App.user.id, db_schedule);
            if (isPosted)
            {
                db_schedule = new Schedules();
                view_schedule = new Schedules();
                back();
                GetScheduals();
                displayList();
            }
        }

        private void back()
        {
            form_listOfSchdls.IsVisible = true;
            form_ViewOneSchdl.IsVisible = false;
            form_AddManualSchdl.IsVisible=false;

            btn_back.IsVisible = false;

            Headr.Text = "Manage Trips";
        }
        private async void GetScheduals()
        {
            schedules = await api.GetScheduals(App.user.id);
            displayList();

        }

        private void displayList()
        {
            form_ViewOneSchdl.IsVisible = false;
            form_AddManualSchdl.IsVisible=false;

            int counter = 0;

            foreach (var schdl in schedules)
            {
                string date = schdl.days[0].date;
                schdlsList.Add(new SchdlsListForm() { city = schdl.city_id, index=counter, date=date });
                counter++;
            }
            BindableLayout.SetItemsSource(list, schdlsList);
        }

        private void UpdatePlan_View(string date, string time, string placeName)
        {
            TripDay day = new TripDay();
            if(view_schedule.days == null)
                view_schedule.days = new List<TripDay>();

            if(!view_schedule.days.Any(d => d.date == date))
            {
                view_schedule.days.Add(new TripDay { date = date, hours = new List<TripTime>() });
                day = view_schedule.days.Single(d => d.date == date);
                day.hours.Add(new TripTime { time = time, place_id = placeName });
            }
            else
            {
                day = view_schedule.days.Single(d => d.date == date);

                if (day.hours.Any(plc => plc.place_id == placeName))
                    day.hours.Single(plc => plc.place_id == placeName).time = time;
                
                else 
                    day.hours.Add(new TripTime { time = time, place_id = placeName });  
            }

            var temp_View = new List<SchdlViewForm>();
            foreach (var d in view_schedule.days)
            {
                foreach (var h in d.hours)
                {
                    temp_View.Add(new SchdlViewForm { date=d.date, distination = h.place_id+"-At "+h.time });
                }
            }
            BindableLayout.SetItemsSource(newPlantable, temp_View);
        }

        private void UpdateScheduel_db(string date, string time, string PlaceID)
        {
            TripDay day = new TripDay();
            if (db_schedule.days == null)
                db_schedule.days = new List<TripDay>();

            if (!db_schedule.days.Any(d => d.date == date))
            {
                db_schedule.days.Add(new TripDay { date = date, hours = new List<TripTime>() });
                day = db_schedule.days.Single(d => d.date == date);
                day.hours.Add(new TripTime { time = time, place_id = PlaceID });
            }
            else
            {
                day = db_schedule.days.Single(d => d.date == date);

                if (day.hours.Any(plc => plc.place_id == PlaceID))
                    day.hours.Single(plc => plc.place_id == PlaceID).time = time;

                else
                    day.hours.Add(new TripTime { time = time, place_id = PlaceID });
            }
        }

    }
}