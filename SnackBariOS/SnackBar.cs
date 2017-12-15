using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SnackBariOS
{
    public enum SBAnimationLength
    {
        Short = 0,
        Long = 1,
    }
    public class SnackBar : NSObject
    {
        public float SnackbarHeight = 65;
        public UIColor BackgroundColor = UIColor.DarkGray;
        public UIColor TextColor = UIColor.White;
        public UIColor ButtonColor = UIColor.Cyan;
        public UIColor ButtonColorPressed = UIColor.Gray;
        public SBAnimationLength sbLength = SBAnimationLength.Short;
        public double AnimateDuration = 0.4;

        //private variables
        private UIWindow window = UIApplication.SharedApplication.KeyWindow;
        private UIView snackbarView = new UIView();


        private UILabel txt = new UILabel();
        private UIButton btn = new UIButton();


        private Action action = null;

        public SnackBar()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(Self, new ObjCRuntime.Selector("rotate"), UIDevice.OrientationDidChangeNotification, null);
        }

        /// Show snackbar with text and button
        public void ShowSnackBar(string text, string actionTitle = "", Action action = null)
        {
            setupSnackbarView();
            txt.Text = text;
            txt.TextColor = TextColor;
            if (!string.IsNullOrEmpty(actionTitle) && action != null)
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
            switch (sbLength)
            {
                case SBAnimationLength.Short:
                    animateBar(2);
                    break;
                case SBAnimationLength.Long:
                    animateBar(3);
                    break;
            }
        }
        private void setupSnackbarView()
        {
            window.AddSubview(snackbarView);
            snackbarView.Frame = new CGRect(0, window.Frame.Height, window.Frame.Width, SnackbarHeight);
            snackbarView.BackgroundColor = this.BackgroundColor;
        }
        private void animateBar(float timerLength)
        {
            UIView.Animate(AnimateDuration, () =>
            {
                this.snackbarView.Frame = new CGRect(0, this.window.Frame.Height - this.SnackbarHeight, this.window.Frame.Width, this.SnackbarHeight);
                NSTimer.CreateScheduledTimer(timerLength, false, (NSTimer obj) =>
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
            UIView.Animate(AnimateDuration, () => {
                this.snackbarView.Frame = new CGRect(0, this.window.Frame.Height, this.window.Frame.Width, this.SnackbarHeight);
            });
        }
    }
}

