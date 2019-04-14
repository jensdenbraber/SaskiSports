using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SaskiSports
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private TimeSpan totalWorkOutTime;
        private TimeSpan selectedTime;
        private int numberOfRounds;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ButtonStart_Clicked { get; }
        public ICommand ButtonStop_Clicked { get; }

        private bool IsRunning { get; set; }

        public MainViewModel()
        {
            TotalWorkOutTime = new TimeSpan();

            ButtonStart_Clicked = new Command(Init);

            ButtonStop_Clicked = new Command(InitStop);

            AppBackgroundColor = Color.Purple;
        }

        private void Init()
        {
            if (!IsRunning)
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick);

                SelectedTime = new TimeSpan(0, 2, 0);
            }
            IsRunning = true;
        }

        private void InitStop()
        {
            IsRunning = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TimeSpan TotalWorkOutTime

        {
            get
            {
                return totalWorkOutTime;
            }
            set
            {
                if (totalWorkOutTime != null)
                {
                    totalWorkOutTime = value;

                    OnPropertyChanged("TotalWorkOutTime");
                }
            }
        }

        public TimeSpan SelectedTime
        {
            get
            {
                return selectedTime;
            }
            set
            {
                if (selectedTime != value)
                {
                    selectedTime = value;

                    OnPropertyChanged("SelectedTime");
                }
            }
        }

        public int NumberOfRounds
        {
            get
            {
                return numberOfRounds;
            }
            set
            {
                numberOfRounds = value;

                OnPropertyChanged("NumberOfRounds");
            }
        }

        private Color appBackgroundColor;

        public Color AppBackgroundColor
        {
            get
            {
                return appBackgroundColor;
            }
            set
            {
                appBackgroundColor = value;

                OnPropertyChanged("AppBackgroundColor");
            }
        }

        public bool OnTimerTick()
        {
            TotalWorkOutTime = TotalWorkOutTime.Add(new TimeSpan(0, 0, 1));
            SelectedTime = SelectedTime.Subtract(new TimeSpan(0, 0, 1));

            if (SelectedTime.Equals(new TimeSpan(0, 0, 3)))
            {
                AppBackgroundColor = Color.Green;
            }

            if (SelectedTime.Equals(new TimeSpan(0, 0, 2)))
            {
                AppBackgroundColor = Color.Purple;
            }

            if (SelectedTime.Equals(new TimeSpan(0, 0, 1)))
            {
                AppBackgroundColor = Color.Green;
            }

            if (SelectedTime.Equals(new TimeSpan(0, 0, 0)))
            {
                AppBackgroundColor = Color.Purple;

                try
                {
                    DependencyService.Get<IAudio>().PlayAudioFile("MySong.mp3");
                    Vibration.Vibrate(2000);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                }

                NumberOfRounds++;
                SelectedTime = new TimeSpan(0, 2, 0);
            }

            return IsRunning;
        }
    }
}