﻿using System;
using System.Threading;
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
        public double AnimateDuration = 0.5;
        //private variables
        private UIWindow window = UIApplication.SharedApplication.KeyWindow;
        private UIView snackbarView = null;
        private UILabel txt = new UILabel();
        private UIButton btn = new UIButton();
        private Action action = null;
        public nfloat Alpha = 100;
        NSTimer timer = null;
        AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        public int Duration = 3;
        private bool isShowing
        {
            get
            {
                return snackbarView != null;
            }
        }
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
                hideNow();
            //Unblock thread
            autoResetEvent.Set();
            new Thread(() =>
            {
                autoResetEvent.WaitOne();
                autoResetEvent.Reset();
                InvokeOnMainThread(() =>
                {
                    snackbarView = new UIView();
                    float y = (float)window.Bounds.Size.Height - SnackbarHeight - (float)window.SafeAreaInsets.Bottom;
                    snackbarView.Frame = new CGRect(0, y, window.Frame.Width, SnackbarHeight + (float)window.SafeAreaInsets.Bottom);
                    snackbarView.BackgroundColor = this.BackgroundColor;
                    snackbarView.Alpha = Alpha;
                    txt.Text = text;
                    txt.TextColor = TextColor;

                    if (action == null)
                        action = () => hide();
                    float margin = 8;

                    if (!string.IsNullOrEmpty(actionTitle))
                    {
                        txt.Frame = new CGRect(margin, 0, window.Frame.Width * 0.75, SnackbarHeight);
                        this.action = action;
                        btn.SetTitleColor(ButtonColor, UIControlState.Normal);
                        btn.SetTitleColor(UIColor.Gray, UIControlState.Highlighted);
                        btn.SetTitle(actionTitle, UIControlState.Normal);
                        btn.Frame = new CGRect(window.Frame.Width * 0.75 - margin, 0, window.Frame.Width * 0.25 + margin, SnackbarHeight);
                        btn.AddTarget(Self, new ObjCRuntime.Selector("actionButtonPress"), UIControlEvent.TouchUpInside);
                        snackbarView.AddSubview(btn);
                    }
                    else
                    {
                        txt.Frame = new CGRect(margin, 0, window.Frame.Width - margin, SnackbarHeight);
                    }
                    snackbarView.AddSubview(txt);
                    show(Duration);
                });
  
            }).Start();
        }


        private void show(int Duration)
        {
            animateBar(Duration);
        }
        private void setupSnackbarView()
        {   
            window.AddSubview(snackbarView);
            snackbarView.Frame = new CGRect(0, window.Frame.Height, window.Frame.Width, SnackbarHeight);
            snackbarView.BackgroundColor = this.BackgroundColor;
        }
        private void animateBar(int timerLength)
        {
            UIView.Animate(AnimateDuration, () =>
            {
                window.AddSubview(snackbarView);
            }, () =>
            {
                if (timer != null)
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
            if (snackbarView != null)
            {
                float y = (float)window.Bounds.Size.Height - SnackbarHeight - (float)window.SafeAreaInsets.Bottom;
                snackbarView.Frame = new CGRect(0, y, window.Frame.Width, SnackbarHeight + window.SafeAreaInsets.Bottom);
                btn.Frame = new CGRect(window.Frame.Width * 73 / 100, 0, window.Frame.Width * 25 / 100, SnackbarHeight);
            }
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
                if (snackbarView != null)
                    snackbarView.Alpha = nfloat.Parse("0");
            }, () =>
            {
                if (snackbarView != null)
                {
                    snackbarView.RemoveFromSuperview();
                    snackbarView = null;
                }
                autoResetEvent.Set();
            });
        }
        private void hideNow()
        {
            if (snackbarView != null)
            {
                snackbarView.RemoveFromSuperview();
                snackbarView = null;
            }
            autoResetEvent.Set();
        }
    }
    
}

