# SnackbarXamariniOS
For Xamarin iOS snackbar which is similar to Android. 
This project will not update from now, please refer to Xamarin Forms one.

#### Xamarin Forms version
https://github.com/JimmyPun610/Plugin.XF.Controls

#### Screen sample
<table>
  <tr>
    <td><img src="https://github.com/JimmyPun610/SnackbarXamariniOS/blob/master/Screen1.PNG" width="250"></td>
    <td><img src="https://github.com/JimmyPun610/SnackbarXamariniOS/blob/master/Screen2.PNG" width="250"></td>
  </tr>
</table>

#### Basic Usage 

1. Add the snackbar to your iOS project as reference.

2. To use
```C#
var snackbar = SnackBariOS.SnackBar.Instance;

//You may customizey snackbar theme here, here is the example
snackbar.SnackbarHeight = 65;
snackbar.TextColor = UIColor.White;
snackbar.Duartion = duration / 1000;
Color XFBackgroundColor = Color.FromHex("CCD94A33");
snackbar.BackgroundColor = UIColor.FromRGBA(nfloat.Parse(XFBackgroundColor.R.ToString()), nfloat.Parse(XFBackgroundColor.G.ToString()), nfloat.Parse(XFBackgroundColor.B.ToString()), nfloat.Parse(XFBackgroundColor.A.ToString()));
Color XFActionTextColor = Color.FromHex("44f8ff");
snackbar.ButtonColor = UIColor.FromRGB(nfloat.Parse(XFActionTextColor.R.ToString()), nfloat.Parse(XFActionTextColor.G.ToString()), nfloat.Parse(XFActionTextColor.B.ToString()));

//show it
snackbar.ShowSnackBar(text, actionText, action);
```

3. Things you can customize and their default value
```C#
public float SnackbarHeight = 65;
public UIColor BackgroundColor = UIColor.DarkGray;
public UIColor TextColor = UIColor.White;
public UIColor ButtonColor = UIColor.Cyan;
public UIColor ButtonColorPressed = UIColor.Gray;
public int Duartion = 3;
public double AnimateDuration = 0.4;
```


