using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SnackBariOS
{
   
    public class SnackBar : NSObject
    {
        public float SnackbarHeight = 65;
        public UIColor BackgroundColor = UIColor.DarkGray;
        public UIColor TextColor = UIColor.White;
        public UIColor ButtonColor = UIColor.Cyan;
        public UIColor ButtonColorPressed = UIColor.Gray;
        public int Duartion = 3;
        public double AnimateDuration = 0.4;
        //private variables
        private UIWindow window = UIApplication.SharedApplication.KeyWindow;
        private UIView snackbarView = new UIView();
        private UILabel txt = new UILabel();
        private UIButton btn = new UIButton();
        private Action action = null;
        NSTimer timer = null;
        bool isShowing = false;

        private static SnackBar _instance { get; set; }
        public static SnackBar Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SnackBar();
                return _instance;
            }
        }

        public SnackBar()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(Self, new ObjCRuntime.Selector("rotate"), UIDevice.OrientationDidChangeNotification, null);
            txt.LineBreakMode = UILineBreakMode.WordWrap;
            txt.Lines = 2;
            
        }

        /// Show snackbar with text and button
        public void ShowSnackBar(string text, string actionTitle = "", Action action = null)
        {
            if (isShowing)
            {
                hide();
            }
            setupSnackbarView();
            txt.Text = text;
            txt.TextColor = TextColor;
            
            if (action == null)
                action = () => hide();

            if (!string.IsNullOrEmpty(actionTitle))
            {
                txt.Frame = new CGRect(window.Frame.Width * 5 / 100, 0, window.Frame.Width * 75 / 100, SnackbarHeight);
                this.action = action;
                btn.SetTitleColor(ButtonColor, UIControlState.Normal);
                btn.SetTitleColor(UIColor.Gray, UIControlState.Highlighted);
                btn.SetTitle(actionTitle, UIControlState.Normal);
                btn.Frame = new CGRect(window.Frame.Width * 73 / 100, 0, window.Frame.Width * 25 / 100, SnackbarHeight);
                btn.AddTarget(Self, new ObjCRuntime.Selector("actionButtonPress"), UIControlEvent.TouchUpInside);
                snackbarView.AddSubview(btn);
            }
            else
            {
                txt.Frame = new CGRect(window.Frame.Width * 5 / 100, 0, window.Frame.Width * 95 / 100, SnackbarHeight);
            }
            snackbarView.AddSubview(txt);
            
            show();
        }


        private void show()
        {
            animateBar(Duartion);
        }
        private void setupSnackbarView()
        {   
            window.AddSubview(snackbarView);
            snackbarView.Frame = new CGRect(0, window.Frame.Height, window.Frame.Width, SnackbarHeight);
            snackbarView.BackgroundColor = this.BackgroundColor;
        }
        private void animateBar(int timerLength)
        {
            isShowing = true;
            UIView.Animate(AnimateDuration, () =>
            {
                this.snackbarView.Frame = new CGRect(0, this.window.Frame.Height - this.SnackbarHeight, this.window.Frame.Width, this.SnackbarHeight);
                if(timer != null)
                {
                    timer.Invalidate();
                    timer = null;
                }
                    
                        
                timer = NSTimer.CreateScheduledTimer(timerLength, false, (NSTimer obj) =>
                {
                    hide();
                });
            });
        }

        [Foundation.Export("rotate")]
        private void rotate()
        {
            this.snackbarView.Frame = new CGRect(0, this.window.Frame.Height - this.SnackbarHeight, this.window.Frame.Width, this.SnackbarHeight);
            btn.Frame = new CGRect(window.Frame.Width * 73 / 100, 0, window.Frame.Width * 25 / 100, SnackbarHeight);
        }

        [Foundation.Export("actionButtonPress")]
        private void actionButtonPress()
        {
            if (action != null)
                action.Invoke();
            hide();
        }

        [Foundation.Export("hide")]
        private void hide()
        {
            isShowing = false;
            UIView.Animate(AnimateDuration, () => {
                this.snackbarView.Frame = new CGRect(0, this.window.Frame.Height, this.window.Frame.Width, this.SnackbarHeight);
            });
        }
    }
}

