namespace MAUIMediaCapture
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void CapturePhotoBtn_Clicked(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult? fileResult = await MediaPicker.Default.CapturePhotoAsync();
                if (fileResult is null)
                {
                    // User canceled
                    await DisplayAlert("Alert", "No photo captured", "Ok");
                    return;
                }
                var photoStream = await fileResult.OpenReadAsync();

                // You can use this photoStream to perform different options
                // 1. Show it on the UI
                // 2. Save it in the SQLIte database
                // 3. Send this photo to API
                // 4. Resize/crop or edit operation on this image

                ShowPhotoStreamOnUI(photoStream);
                return;

                // save this image on device
                await SavePhotoToDeviceAsync(photoStream, fileResult.FileName);
                return;

                await SavePhotoStreamToLocalDbAsync(photoStream);
                return;

                await SendPhotoStreamToAPIAsync(photoStream);
                return;

                await SavePhotoStreamToLocalDbAsync(photoStream);
                return;

                ResizePhoto(photoStream);
                return;

                AdvancedImageEditing(photoStream);
            }
            else
            {
                // show some alert or toast message
            }
        }

        private void ShowPhotoStreamOnUI(Stream photoStream)
        {
            Img.Source = ImageSource.FromStream(() => photoStream);
        }
        private async Task SavePhotoToDeviceAsync(Stream photoStream, string fileName)
        {
            var path = Path.Combine(FileSystem.CacheDirectory, fileName);
            using var fs = File.Create(path);
            await photoStream.CopyToAsync(fs);
        }

        private async Task SendPhotoStreamToAPIAsync(Stream photoStream)
        {
            // REFIT - //StreamPart = photoStream;
            // HTTPClient
            //StreamContent 
            //ByteArrayContent
            //MultipartFormDataContent
            var ms = new MemoryStream();
            photoStream.CopyTo(ms);            
            var byteArray = ms.ToArray();
        }

        private async Task SavePhotoStreamToLocalDbAsync(Stream photoStream)
        {
            // in SQLite we can have byte[] column to save the binary data
            var ms = new MemoryStream();
            photoStream.CopyTo(ms);
            var byteArray = ms.ToArray();

            // Save this byteArray to the database
        }

        private void ResizePhoto(Stream photoStream)
        {
            // BAsic resizing we can do with GraphicsView in .NET MAUI
        }

        private void AdvancedImageEditing(Stream photoStream)
        {
            // Use some thrid party compnnet/control
            // for example Syncfusion ImageEditor control
        }

        private async void PickPhotoBtn_Clicked(object sender, EventArgs e)
        {
            var options = new MediaPickerOptions
            {
                Title = "Select Photo"
            };
            FileResult? fileResult = await MediaPicker.Default.PickPhotoAsync(options);
            if(fileResult is null)
            {
                await DisplayAlert("Not Selected", "No photo selected", "OK");
                return;
            }

            var pickedPhotoPath =  fileResult.FullPath;
            // Show it on the ui
            Img.Source = pickedPhotoPath;

            // if video
            // FileResult? fileResult = await MediaPicker.Default.PickVideoAsync(options);
            //  Display the video using Community Toolkit MAUI Media Elements
        }
    }

}
