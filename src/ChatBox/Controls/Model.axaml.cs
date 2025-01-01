using ChatBox.Service;
using System.Linq;

namespace ChatBox.Controls;

public partial class Model : UserControl
{
	private readonly TokenService _tokenService;

	public Model()
	{
		InitializeComponent();

		_tokenService = HostApplication.Services.GetService<TokenService>();
	}

	protected override void OnDataContextChanged(EventArgs e)
	{
		base.OnDataContextChanged(e);
		GetToken();
	}

	private ModelListViewModel ViewModel => (ModelListViewModel)DataContext;

	public void GetToken()
	{
		ViewModel.Models.Clear();
		foreach (var model in _tokenService.LoadModels())
		{
			ViewModel.Models.Add(model);
		}
		var settingService = HostApplication.Services.GetRequiredService<ISettingService>();
		ViewModel.ModelIdChanged += (model) =>
		{
			if (model != null)
			{
				var setting = settingService.LoadSetting();
				setting.ModelId = model.Id;
				settingService.SaveSetting(setting);
			}
		};

		if (ViewModel.Models.Count > 0)
		{
			var setting = settingService.LoadSetting();
			var item = ViewModel.Models.FirstOrDefault(x => x.Id == setting.ModelId);
			if (string.IsNullOrEmpty(setting.ModelId) || item == null)
			{
				ViewModel.ModelId = ViewModel.Models[0];
			}
			else
			{
				ViewModel.ModelId = item;
			}

		}
	}
}