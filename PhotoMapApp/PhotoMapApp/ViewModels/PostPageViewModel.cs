using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using PhotoMapApp.Models;
using Xamarin.Forms;
using PhotoMapApp.Services.Definitions;

namespace PhotoMapApp.ViewModels
{
    public class PostPageViewModel : ViewModelBase
    {
        private Post _post;
        public Post Post { get { return this._post; } set { SetProperty(ref this._post, value); }}
        public DelegateCommand DeletePostDelegate { get; private set; }
        public DelegateCommand<Post> EditPostDelegate { get; private set; }
        private IImageService _imageService;

        private ImageSource _bannerImageSource;
        public ImageSource BannerImageSource { get { return _bannerImageSource; } set { SetProperty(ref _bannerImageSource, value); } }
        public ImageSource EditButtonImageSource { get; private set; }
        public ImageSource DeleteButtonImageSource { get; private set; }

        private IPageDialogService _dialogService;
        public PostPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IImageService imageService) : base(navigationService)
        {
            this._imageService = imageService;
            this._dialogService = dialogService;
            this.DeletePostDelegate = new DelegateCommand(DeletePostCommand);
            this.EditPostDelegate = new DelegateCommand<Post>(EditPostCommand);

            // Resources
            this.EditButtonImageSource = this._imageService.GetSource("Icons.edit.png");
            this.DeleteButtonImageSource = this._imageService.GetSource("Icons.delete.png");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            this.Post = (Post)parameters["post"];
            this.Title = Post.Name;
            this.BannerImageSource = ImageSource.FromFile(Post.Image);
        }

        private async void DeletePostCommand()
        {
            var answer = await _dialogService.DisplayAlertAsync("Suppression de post", "Voulez-vous vraiment supprimer ce post ?", "Supprimer", "Annuler");
            if (answer == true)
            {
                // TODO : SUPPRIMER LE POST
                await _dialogService.DisplayAlertAsync("Alert", "OUI", "ok");
            }
        }

        private void EditPostCommand(Post post)
        {
            var navigationParam = new NavigationParameters { { "post", post } };
            base.NavigationService.NavigateAsync("NewPost", navigationParam);
        }
    }
}
