namespace MauiApp5
{
    public partial class MainPage : ContentPage
    {
        private string currentUrl = "https://piccindia.com/?s=mobile&o=android&v=1.0.0"; // Your default site

        public MainPage()
        {
            InitializeComponent();
            LoadWebContent();
        }


        private void LoadWebContent()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                MyWebView.Source = currentUrl;
            }
            else
            {
                var htmlSource = new HtmlWebViewSource
                {
                    Html = @"
                    <html>
                    <head>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <script>
                            document.addEventListener('DOMContentLoaded', function() {
                                document.getElementById('status').innerText = 'Offline Mode';
                            });
                        </script>
                    </head>

                    <body style='font-family:sans-serif; text-align:center; padding:20px;'>
                        <h1 style='color: #2c4964;'>Indian Academy of Pediatrics Intensive Care Chapter</h1>
                        <h2 id='status'>Checking...</h2>
                        <p>You are currently offline. Please check your internet connection.</p>
                    </body>
                    </html>"
                };
                MyWebView.Source = htmlSource;
            }
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            if (MyWebView.CanGoBack)
                MyWebView.GoBack();
        }

        private void OnForwardClicked(object sender, EventArgs e)
        {
            if (MyWebView.CanGoForward)
                MyWebView.GoForward();
        }

        private void OnReloadClicked(object sender, EventArgs e)
        {
            LoadWebContent(); // Re-checks connectivity
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            LoadWebContent(); // Same logic as reload
            RefreshControl.IsRefreshing = false;
        }

        private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            // Optional: update currentUrl if you want to preserve navigation
            if (e.Result == WebNavigationResult.Success && e.Url != null)
            {
                currentUrl = e.Url;
            }
        }

        //private async void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        //{
        //    var url = e.Url;

        //    // Example: allow your own domain to load internally
        //    if (!url.Contains("piccindia.com"))
        //    {
        //        e.Cancel = true; // stop loading in WebView

        //        // open externally in default browser
        //        await Launcher.Default.OpenAsync(url);
        //    }
        //}

        private async void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            var url = e.Url;

            // Example: allow your own domain to load internally
            if (!url.Contains("piccindia.com"))
            {
                e.Cancel = true; // stop loading in WebView

                // Ensure Launcher runs on main UI thread
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        await Launcher.Default.OpenAsync(new Uri(url));
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Could not open link: {ex.Message}", "OK");
                    }
                });
            }
        }


    }
}
