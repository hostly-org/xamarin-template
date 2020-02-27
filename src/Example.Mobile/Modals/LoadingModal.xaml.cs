
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Example.Mobile.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingModal : ContentPage
    {
        private Thickness _layoutPadding;
        public Thickness LayoutPadding { 
            get => _layoutPadding; 
            set
            {
                _layoutPadding = value;
                LoadingModalLayout.Padding = value;
            }
        }
        public LoadingModal()
        {
            InitializeComponent();
        }
    }
}
