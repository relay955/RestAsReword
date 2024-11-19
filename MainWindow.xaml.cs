using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using RestAsReward.utils;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace RestAsReward;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window,INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;
    public TimeSpan LeftTime { get; set; } = TimeSpan.FromSeconds(5);
    public bool IsPaused { get; set; } = false;
    public Visibility InWorkVisibility { get; set; } = Visibility.Hidden;
    
    private long _tick;
    
    public MainWindow() {
        InitializeComponent();
        var screenHeight = SystemParameters.PrimaryScreenHeight;
        var screenWidth = SystemParameters.PrimaryScreenWidth;
        Top = screenHeight - Height - 80;
        Left = screenWidth - Width - 10;

        new Thread(o => {
            while (true) {
                if (!IsPaused) {
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                        if (LeftTime < TimeSpan.FromMilliseconds(1)) {
                            LeftTime = TimeSpan.FromMilliseconds(-1);
                            return;
                        }
                        LeftTime = LeftTime.Subtract(TimeSpan.FromMilliseconds(100));
                    }));
                } else {
                    if(_tick % 10 == 0) {
                        Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            InWorkVisibility = InWorkVisibility == Visibility.Visible ? 
                                Visibility.Hidden : Visibility.Visible;
                        }));
                    }
                }
                _tick += 1;
                Thread.Sleep(100);
            }
        }).Start();
    }

    private void StartStopButton_OnMouseDown(object sender, MouseButtonEventArgs e) {
        IsPaused = !IsPaused;
        InWorkVisibility = IsPaused ? Visibility.Visible : Visibility.Hidden;
    }

    
    private void m10Button_OnMouseDown(object sender, MouseButtonEventArgs e) {
        LeftTime = LeftTime.Subtract(TimeSpan.FromMinutes(10));
        if(LeftTime < TimeSpan.FromMilliseconds(1)) LeftTime = TimeSpan.FromMilliseconds(-1);
    }
    
    private void p50Button_OnMouseDown(object sender, MouseButtonEventArgs e) {
        LeftTime = LeftTime.Add(TimeSpan.FromMinutes(50));
    }
    
    private void p10Button_OnMouseDown(object sender, MouseButtonEventArgs e) {
        LeftTime = LeftTime.Add(TimeSpan.FromMinutes(10));
    }
    
    private void p5Button_OnMouseDown(object sender, MouseButtonEventArgs e) {
        LeftTime = LeftTime.Add(TimeSpan.FromMinutes(5));
    }
    
    private void p1Button_OnMouseDown(object sender, MouseButtonEventArgs e) {
        LeftTime = LeftTime.Add(TimeSpan.FromMinutes(1));
    }
    
}

public class LeftTimeToTextConverter : SimpleConverter<TimeSpan> {
    public override object Convert(TimeSpan value) => value.ToString("hh':'mm':'ss'.'f");
}

public class LeftTimeTextColorConverter : SimpleConverter<TimeSpan> {
    public override object Convert(TimeSpan value) {
        if(value < TimeSpan.FromMilliseconds(1)) return Brushes.Red;
        if(value < TimeSpan.FromMinutes(5)) return Brushes.Orange;
        return Brushes.White;
    }
}

public class StartStopButtonTextConverter : SimpleConverter<bool> {
    public override object Convert(bool value) => value ? "Start" : "Stop";
}