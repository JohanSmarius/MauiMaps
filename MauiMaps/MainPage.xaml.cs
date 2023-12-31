﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace MauiMaps;

public partial class MainPage : ContentPage
{
    private readonly IGeolocation _geolocation;
    int count = 0;

    public MainPage(IGeolocation geolocation)
    {
        _geolocation = geolocation;
        InitializeComponent();
        
        Location location = new Location(51.5840246, 4.7953861,17);
        Location locationPin = new Location(51.5840260, 4.7953861,17);
        MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
        MyMap.MoveToRegion(mapSpan);
        MyMap.IsShowingUser = true;
        MyMap.Pins.Add(new Pin() { Location = locationPin, Label = "My Home", Address = "My Address" });
        
        Circle circle = new Circle()
        {
            Center = location,
            Radius = Distance.FromMeters(50),
            StrokeColor = Colors.Aqua,
            StrokeWidth = 8,
            FillColor = Color.FromRgba(255, 0, 0, 64)
        };
        
        MyMap.MapElements.Add(circle);
        
        var dispatcher = Dispatcher.CreateTimer();
        dispatcher.Interval = TimeSpan.FromSeconds(30);
        dispatcher.Tick += async (sender, e) => await DisplayAlert("Alert", "Timer elapsed", "OK");;
        dispatcher.IsRepeating = true;
        //dispatcher.Start();
    }

    private async void StartListening(object sender, EventArgs e)
    {
        int accuracy = (int)GeolocationAccuracy.Default;
        var request = new GeolocationListeningRequest(GeolocationAccuracy.Default);
        
        _geolocation.LocationChanged += Geolocation_LocationChanged;
        var success = await Geolocation.StartListeningForegroundAsync(request);
    }
    
    void Geolocation_LocationChanged(object sender, GeolocationLocationChangedEventArgs e)
    {
        var newLocation = $"{e.Location.Latitude} {e.Location.Longitude}";
        var toast = Toast.Make(newLocation, ToastDuration.Short);
        toast.Show();
    }
}