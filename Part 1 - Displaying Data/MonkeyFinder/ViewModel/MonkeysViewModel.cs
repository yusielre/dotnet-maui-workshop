using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = [];

    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
    }

    [RelayCommand]
    async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null) return;

        //One Way =>// await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?id={monkey.Name}");
        //Other Way =>//
        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}",true,
            new Dictionary<string, object>
            {
                {"Monkey",monkey }
            });
    }

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if(IsBusy) return;

        try
        {
            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();
            if(monkeys.Count != 0)
                Monkeys.Clear();

            foreach(var monkey in monkeys)
                Monkeys.Add(monkey);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys:{ex.Message}","OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
